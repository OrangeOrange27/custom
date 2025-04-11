using System.Collections.Generic;
using DefaultNamespace.Models.CardsConfig;
using UnityEngine;

namespace DefaultNamespace
{
    public class CardsConfigProvider : ICardsConfigProvider
    {
        private readonly Dictionary<int, Sprite> _spritesDictionary;
        
        public CardsConfigProvider(CardsConfig cardsConfig)
        {
            _spritesDictionary = new Dictionary<int, Sprite>();
            
            foreach (var card in cardsConfig.Cards)
            {
                _spritesDictionary.Add(card.Id, card.Sprite);
            }
        }
        
        public Sprite GetSpriteById(int id)
        {
            return _spritesDictionary[id];
        }
    }
}