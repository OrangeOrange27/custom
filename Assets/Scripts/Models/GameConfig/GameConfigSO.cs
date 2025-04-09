using System;
using UnityEngine;

namespace DefaultNamespace
{
    [Serializable]
    [CreateAssetMenu(fileName = "GameConfigSO", menuName = "Config/GameConfig")]
    public class GameConfigSO : ScriptableObject
    {
        [SerializeField] private GameConfig _config;
        
        public GameConfig Config => _config;
    }
}