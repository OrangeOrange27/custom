using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using DefaultNamespace.Models.CardsConfig;
using UnityEngine;

namespace DefaultNamespace
{
    public class GameplayController : MonoBehaviour
    {
        [SerializeField] private CardsConfigSO _cardsConfig;
        [SerializeField] private CardView _cardViewPrefab;
        [SerializeField] private Transform _gameArea;
        
        private ICardsVisualSystem _cardsVisualSystem; 
        private CardsConfigProvider _cardsConfigProvider; 
        private List<CardModel> _cards;

        private void Awake()
        {
            _cardsVisualSystem = new CardsVisualSystem(_cardsConfigProvider, _gameArea, _cardViewPrefab);
            _cardsConfigProvider = new CardsConfigProvider(_cardsConfig.Config);
            
            _cardsVisualSystem.OnCardTap += RegisterCardClick;
            
            //debug
            InitiateGameplay(new GameConfig() { CardsCount = 6 }).Forget();
        }

        public async UniTask InitiateGameplay(GameConfig gameConfig)
        {
            CreateCardModels(gameConfig);
            await _cardsVisualSystem.SpawnInitViews(_cards, CancellationToken.None);
        }

        private void RegisterCardClick(CardModel model)
        {

        }

        private void CreateCardModels(GameConfig gameConfig)
        {
            _cards = new List<CardModel>();

            for (var i = 0; i < gameConfig.CardsCount / 2; i++)
            {
                _cards.Add(CreateCard(i));
            }

            foreach (var card in _cards)
            {
                var matchingCard = CreateMatchingPair(card);
                _cards.Add(matchingCard);
            }

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
                MatchNumber = matchNumber
            };

            return card;
        }
    }
}