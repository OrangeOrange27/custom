using TMPro;
using UnityEngine;

namespace DefaultNamespace
{
    public class ScoreView : MonoBehaviour
    {
        [SerializeField] TMP_Text _scoreText;

        public void SetScore(int score)
        {
            _scoreText.text = $"Score: {score}";
        }
    }
}