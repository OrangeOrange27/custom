using System;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardView : MonoBehaviour, ICardView, IPointerDownHandler
{
    [SerializeField] private Image _image;
    
    public event Action OnTap;
    
    public void SetImage(Sprite sprite)
    {
        _image.sprite = sprite;
    }

    public void Cover()
    {
        _image.gameObject.SetActive(false);
    }

    public void Uncover()
    {
        _image.gameObject.SetActive(true);
    }

    public void Match()
    {
        Destroy(gameObject);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        OnTap?.Invoke();
    }

    public void Dispose()
    {
    }
}
