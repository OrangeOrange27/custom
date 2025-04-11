using System;

namespace DefaultNamespace
{
    public interface IScoreSystem
    {
        int Score { get; }
        
        event Action<int> OnScoreChanged; 
    }
}