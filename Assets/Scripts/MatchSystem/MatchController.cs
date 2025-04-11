using System;
using Cysharp.Threading.Tasks;
using DefaultNamespace.SoundSystem;

namespace DefaultNamespace
{
    public class MatchController : IMatchController
    {
        private CardModel _selectedCard;

        public event Action<CardModel, CardModel> OnMatchSuccess;
        public event Action<CardModel, CardModel> OnMatchFail;

        public void SelectCard(CardModel model)
        {
            if(model==null)
                return;
            
            model.IsRevealed.Value = true;
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
            SoundManager.Instance.Play(SoundType.Match);
            OnMatchSuccess?.Invoke(model, _selectedCard);
            await UniTask.WhenAll(model.View.Match(), _selectedCard.View.Match());
        }

        private async UniTask FailMatch(CardModel model)
        {
            SoundManager.Instance.Play(SoundType.Error);
            OnMatchFail?.Invoke(model, _selectedCard);
            await UniTask.WhenAll(model.View.Cover(), _selectedCard.View.Cover());
        }
    }
}