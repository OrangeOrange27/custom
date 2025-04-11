using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using DefaultNamespace.Models.CardsConfig;
using DefaultNamespace.SoundSystem;
using UnityEngine;

namespace DefaultNamespace
{
    public class GameplayController : MonoBehaviour
    {
        [SerializeField] private float _revealTime;
        [SerializeField] private CardsConfigSO _cardsConfig;
        [SerializeField] private GameConfigSO _gameConfig;
        [SerializeField] private CardView _cardViewPrefab;
        [SerializeField] private Transform _gameArea;
        
        //todo: move out of GameplayController
        [SerializeField] private ScoreView _scoreView;
        [SerializeField] private GameObject _winPanel;

        private ICardsVisualSystem _cardsVisualSystem;
        private ICardsConfigProvider _cardsConfigProvider;
        private IMatchController _matchController;
        private IScoreSystem _scoreSystem;
        private List<CardModel> _cards;

        private void Awake()
        {
            _cardsVisualSystem = new CardsVisualSystem(_gameArea, _cardViewPrefab);
            _cardsConfigProvider = new CardsConfigProvider(_cardsConfig.Config);
            _matchController = new MatchController();
            _scoreSystem = new ScoreSystem(_matchController);

            _cardsVisualSystem.OnCardTap += RegisterCardClick;
            _scoreSystem.OnScoreChanged += _scoreView.SetScore;
            _matchController.OnMatchSuccess += OnMatch;

            InitiateGameplay(_gameConfig.Config).Forget();
        }

        private async UniTask InitiateGameplay(GameConfig gameConfig)
        {
            CreateCardModels(gameConfig);
            await _cardsVisualSystem.SpawnInitViews(_cards, CancellationToken.None);
            
            foreach (var card in _cards)
            {
                card.IsInteractable.Value = false;
            }
            
            await UniTask.WaitForSeconds(_revealTime);
            
            foreach (var card in _cards)
            {
                card.IsInteractable.Value = true;
                card.View.Cover();
            }
        }

        private void RegisterCardClick(CardModel model)
        {
            _matchController.SelectCard(model);
        }

        private void CreateCardModels(GameConfig gameConfig)
        {
            _cards = new List<CardModel>();
            for (var i = 0; i < gameConfig.CardsCount / 2; i++)
            {
                _cards.Add(CreateCard(i));
            }

            var matchingCards = _cards.Select(CreateMatchingPair).ToList();
            _cards.AddRange(matchingCards);

            _cards.Shuffle();
        }

        private CardModel CreateMatchingPair(CardModel card)
        {
            return CreateCard(card.MatchNumber);
        }

        private CardModel CreateCard(int matchNumber)
        {
            var card = new CardModel()
            {
                MatchNumber = matchNumber,
                CardImage = _cardsConfigProvider.GetSpriteById(matchNumber)
            };

            return card;
        }

        private void OnMatch(CardModel card1, CardModel card2)
        {
            card1.IsInteractable.Value = false;
            card2.IsInteractable.Value = false;
            
            _cards.Remove(card1);
            _cards.Remove(card2);
            
            if(_cards.Count<=0)
                GameOver();
        }

        private void GameOver()
        {
            SoundManager.Instance.Play(SoundType.Victory);
            _winPanel.SetActive(true);
        }

        private void OnDestroy()
        {
            _cardsVisualSystem.OnCardTap -= RegisterCardClick;
            _scoreSystem.OnScoreChanged -= _scoreView.SetScore;
            _matchController.OnMatchSuccess -= OnMatch;
        }
    }
}