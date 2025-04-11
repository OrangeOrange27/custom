using UnityEngine;

namespace DefaultNamespace
{
    public interface ICardsConfigProvider
    {
        Sprite GetSpriteById(int id);
    }
}