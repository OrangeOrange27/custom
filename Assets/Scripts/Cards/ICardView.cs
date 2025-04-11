using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace DefaultNamespace
{
    public interface ICardView : IDisposable
    {
        event Action OnTap;

        void SetImage(Sprite sprite);
        void SetInteractable(bool interactable);
        void Reveal();
        
        UniTask Cover();
        UniTask Uncover();
        UniTask Match();

        void Kill();
    }
}