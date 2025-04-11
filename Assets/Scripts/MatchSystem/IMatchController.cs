using System;

namespace DefaultNamespace
{
    public interface IMatchController
    {
        event Action OnMatchSuccess; 
        event Action OnMatchFail;

        void SelectCard(CardModel model);
    }
}