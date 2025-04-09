using System;
using UnityEngine;

namespace DefaultNamespace.Models.CardsConfig
{
    [Serializable]
    [CreateAssetMenu(fileName = "CardsConfigSO", menuName = "Config/CardsConfig")]
    public class CardsConfigSO : ScriptableObject
    {
        [SerializeField] private CardsConfig _config;
        
        public CardsConfig Config => _config;
    }
}