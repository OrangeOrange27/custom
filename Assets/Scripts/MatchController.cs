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
                Match(model);
            }
            else
            {
                model.View.Cover();
                _selectedCard.View.Cover();
            }
        }
        
        private void Match(CardModel model)
        {
            _score++;
            model.View.Match();
            _selectedCard.View.Match();
        }
    }
}