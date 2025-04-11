using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using DefaultNamespace.Models.CardsConfig;
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
        
        private ICardsVisualSystem _cardsVisualSystem; 
        private CardsConfigProvider _cardsConfigProvider; 
        private MatchController _matchController;
        private List<CardModel> _cards;
        
        private void Awake()
        {
            _cardsVisualSystem = new CardsVisualSystem(_cardsConfigProvider, _gameArea, _cardViewPrefab);
            _cardsConfigProvider = new CardsConfigProvider(_cardsConfig.Config);
            _matchController = new MatchController();
            
            _cardsVisualSystem.OnCardTap += RegisterCardClick;
            
            InitiateGameplay(_gameConfig.Config).Forget();
        }

        private async UniTask InitiateGameplay(GameConfig gameConfig)
        {
            CreateCardModels(gameConfig);
            await _cardsVisualSystem.SpawnInitViews(_cards, CancellationToken.None);
            await UniTask.WaitForSeconds(_revealTime);
            foreach (var card in _cards)
            {
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
    }
}