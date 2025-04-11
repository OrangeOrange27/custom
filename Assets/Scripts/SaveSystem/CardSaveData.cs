using System.Collections.Generic;

namespace DefaultNamespace.SaveSystem
{
    [System.Serializable]
    public class GameSaveData
    {
        public List<CardSaveData> Cards;
        public int Score;
    }

    [System.Serializable]
    public class CardSaveData
    {
        public int MatchNumber;
        public bool IsMatched;
        public bool IsRevealed;
    }
}