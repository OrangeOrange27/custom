using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using DefaultNamespace.Models.CardsConfig;
using DefaultNamespace.SaveSystem;
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
        private ISaveSystem _saveSystem;
        private List<CardModel> _cards;

        private void Awake()
        {
            _cardsVisualSystem = new CardsVisualSystem(_gameArea, _cardViewPrefab);
            _cardsConfigProvider = new CardsConfigProvider(_cardsConfig.Config);
            _matchController = new MatchController();
            _scoreSystem = new ScoreSystem(_matchController);
            _saveSystem = new PlayerPrefsSaveSystem();

            _cardsVisualSystem.OnCardTap += RegisterCardClick;
            _scoreSystem.OnScoreChanged += _scoreView.SetScore;
            _matchController.OnMatchSuccess += OnMatch;

            InitiateGameplay(_gameConfig.Config).Forget();
        }

        private async UniTask InitiateGameplay(GameConfig gameConfig)
        {
            var loaded = await TryLoadGame();
            if (loaded)
                return;

            CreateCardModels(gameConfig);
            await _cardsVisualSystem.SpawnInitViews(_cards, CancellationToken.None);

            foreach (var card in _cards)
            {
                card.IsInteractable.Value = false;
                card.View.Reveal();
            }

            await UniTask.WaitForSeconds(_revealTime);

            foreach (var card in _cards)
            {
                card.IsInteractable.Value = true;
                card.IsRevealed.Value = false;
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

            var unmatchedCards =
                _cards.Count(c => c.IsInteractable.Value);
            if (unmatchedCards <= 0)
                GameOver();
        }

        private void GameOver()
        {
            SoundManager.Instance.Play(SoundType.Victory);
            _winPanel.SetActive(true);
            _saveSystem.Clear();
        }

        private void SaveGame()
        {
            var isGameOver = true;
            foreach (var card in _cards)
            {
                if (card.IsInteractable.Value)
                    isGameOver = false;
            }

            if (isGameOver)
            {
                _saveSystem.Clear();
                return;
            }
            
            var data = new GameSaveData
            {
                Score = _scoreSystem.Score,
                Cards = _cards.Select(card => new CardSaveData
                {
                    MatchNumber = card.MatchNumber,
                    IsMatched = !card.IsInteractable.Value,
                    IsRevealed = card.IsRevealed.Value
                }).ToList()
            };

            _saveSystem.Save(data);
        }

        private async UniTask<bool> TryLoadGame()
        {
            if (!_saveSystem.HasSave())
                return false;

            var save = _saveSystem.Load();

            _cards = save.Cards.Select(c => new CardModel
            {
                MatchNumber = c.MatchNumber,
                CardImage = _cardsConfigProvider.GetSpriteById(c.MatchNumber)
            }).ToList();

            _scoreSystem.ResetScore(save.Score);

            await _cardsVisualSystem.SpawnInitViews(_cards, CancellationToken.None);

            for (int i = 0; i < _cards.Count; i++)
            {
                var card = _cards[i];
                card.IsInteractable.Value = !save.Cards[i].IsMatched;
                if (card.IsInteractable.Value == false)
                    card.View.Kill();
            }

            _matchController.SelectCard(_cards.FirstOrDefault(c => c.IsInteractable.Value));

            return true;
        }

        private void OnApplicationPause(bool pause)
        {
            if (pause)
                SaveGame();
        }

        private void OnDestroy()
        {
            SaveGame();

            _cardsVisualSystem.OnCardTap -= RegisterCardClick;
            _scoreSystem.OnScoreChanged -= _scoreView.SetScore;
            _matchController.OnMatchSuccess -= OnMatch;
        }
    }
}