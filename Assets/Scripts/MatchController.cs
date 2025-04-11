using Cysharp.Threading.Tasks;

namespace DefaultNamespace
{
    public class MatchController
    {
        private CardModel _selectedCard;
        private int _score;

        public void SelectCard(CardModel model)
        {
            model.View.Uncover();
            if (_selectedCard == null)
            {
                _selectedCard = model;
                return;
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
                model.View.Cover().Forget();
                _selectedCard.View.Cover().Forget();
            }
        }
        
        private async UniTask Match(CardModel model)
        {
            _score++;
            model.View.Match();
            _selectedCard.View.Match();
        }
    }
}