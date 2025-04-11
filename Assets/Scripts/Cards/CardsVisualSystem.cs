using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Object = UnityEngine.Object;

namespace DefaultNamespace
{
    public class CardsVisualSystem : ICardsVisualSystem
    {
        private readonly DisposableBag _disposableBag = new();
        private readonly Transform _cardsParent;
        private readonly CardView _cardViewPrefab;

        private ICardViewController[] _viewControllers;

        public CardsVisualSystem(Transform cardsParent, CardView cardViewPrefab)
        {
            _cardsParent = cardsParent;
            _cardViewPrefab = cardViewPrefab;
        }

        public event Action<CardModel> OnCardTap;

        public async UniTask SpawnInitViews(List<CardModel> cardModels, CancellationToken token)
        {
            _viewControllers =
                await UniTask.WhenAll(cardModels.Select(model => LoadSpawnView(model, token)));

            await InitializeViews();
        }
        
        private async UniTask InitializeViews()
        {
            foreach (var viewController in _viewControllers)
            {
                viewController.InitObserving();
            }
        }

        private async UniTask<ICardViewController> LoadSpawnView(CardModel model, CancellationToken token)
        {
            ICardView view = Object.Instantiate(_cardViewPrefab, _cardsParent);
            var viewController = new CardViewController();

            model.View = view;
            viewController.InitOnCreate(view, model);
            _disposableBag.Add(viewController);

            viewController.OnTap += OnViewTap;

            return viewController;
        }

        private void OnViewTap(CardModel model)
        {
            OnCardTap?.Invoke(model);
        }

        public void Dispose()
        {
            _disposableBag?.Dispose();
        }
    }
}