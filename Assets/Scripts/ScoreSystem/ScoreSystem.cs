using System;

namespace DefaultNamespace
{
    public class ScoreSystem : IScoreSystem
    {
        private readonly IMatchController _matchController;
        
        private int _currentScore;
        private int _streak;

        public int Score
        {
            get => _currentScore;
            private set
            {
                _currentScore = value;
                OnScoreChanged?.Invoke(value);
            }
        }

        public event Action<int> OnScoreChanged;
        
        public ScoreSystem(IMatchController matchController)
        {
            _matchController = matchController;
            
            _matchController.OnMatchSuccess += OnMatchSuccess;
            _matchController.OnMatchFail += OnMatchFail;
        }
        
        public void ResetScore(int score)
        {
            Score = score;
        }

        private void OnMatchSuccess(CardModel card1, CardModel card2)
        {
            _streak++;
            Score += _streak;
        }

        private void OnMatchFail(CardModel card1, CardModel card2)
        {
            _streak = 0;
        }
    }
}