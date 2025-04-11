using DefaultNamespace.Utils;
using UnityEngine;

namespace DefaultNamespace
{
    [System.Serializable]
    public class CardModel
    {
        public ICardView View;
        public int MatchNumber;
        public Sprite CardImage;
        public ReactiveProperty<bool> IsInteractable;
    }
}