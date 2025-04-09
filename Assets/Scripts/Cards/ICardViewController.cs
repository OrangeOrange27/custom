using System;

namespace DefaultNamespace
{
    public interface ICardViewController : IDisposable
    {
        event Action<CardModel> OnTap;
        void InitOnCreate(ICardView view, CardModel model);
        void InitObserving();
    }
}