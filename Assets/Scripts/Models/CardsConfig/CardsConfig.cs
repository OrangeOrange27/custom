using System;
using UnityEngine;

namespace DefaultNamespace.Models.CardsConfig
{
    [Serializable]
    public class CardsConfig
    {
        public CardsConfigEntry[] Cards;
    }
    
    [Serializable]
    public class CardsConfigEntry
    {
        public int Id;
        public Sprite Sprite;
    }
}