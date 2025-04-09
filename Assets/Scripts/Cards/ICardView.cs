using System;
using UnityEngine;

namespace DefaultNamespace
{
    public interface ICardView : IDisposable
    {
        event Action OnTap;

        void SetImage(Sprite sprite);
    }
}