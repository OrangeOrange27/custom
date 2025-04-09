using System;

namespace DefaultNamespace
{
    public class CardViewController : ICardViewController
    {
        private CardModel _model;
        private ICardView _view;
        
        public event Action<CardModel> OnTap;
        
        public void InitOnCreate(ICardView view, CardModel model)
        {
            _view = view;
            _model = model;
            
            _view.SetImage(_model.CardImage);
        }

        public void InitObserving()
        {
            _view.OnTap += OnViewTap;
        }

        public void Dispose()
        {
            _view.OnTap -= OnViewTap;
        }

        private void OnViewTap()
        {
            OnTap?.Invoke(_model);
        }
    }
}