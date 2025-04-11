using System;

namespace DefaultNamespace
{
    public interface IMatchController
    {
        event Action<CardModel, CardModel> OnMatchSuccess; 
        event Action<CardModel, CardModel> OnMatchFail;

        void SelectCard(CardModel model);
    }
}