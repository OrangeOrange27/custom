using System;
using Cysharp.Threading.Tasks;

namespace DefaultNamespace
{
    public class MatchController : IMatchController
    {
        private CardModel _selectedCard;

        public event Action OnMatchSuccess;
        public event Action OnMatchFail;

        public void SelectCard(CardModel model)
        {
            model.View.Uncover();
            if (_selectedCard == null)
            {
                _selectedCard = model;
            }
            else
            {
                TryMatch(model);
                _selectedCard = null;
            }
        }

        private void TryMatch(CardModel model)
        {
            if (_selectedCard.MatchNumber == model.MatchNumber && _selectedCard != model)
            {
                Match(model).Forget();
            }
            else
            {
                FailMatch(model).Forget();
            }
        }

        private async UniTask Match(CardModel model)
        {
            OnMatchSuccess?.Invoke();
            await UniTask.WhenAll(model.View.Match(), _selectedCard.View.Match());
        }

        private async UniTask FailMatch(CardModel model)
        {
            OnMatchFail?.Invoke();
            await UniTask.WhenAll(model.View.Cover(), _selectedCard.View.Cover());
        }
    }
}