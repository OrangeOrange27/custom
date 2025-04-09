using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace DefaultNamespace
{
    public interface ICardsVisualSystem : IDisposable
    {
        event Action<CardModel> OnCardTap;
        
        UniTask SpawnInitViews(List<CardModel> cardModels, CancellationToken token);
    }
}