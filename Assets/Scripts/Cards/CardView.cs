using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DefaultNamespace;
using DefaultNamespace.SoundSystem;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardView : MonoBehaviour, ICardView, IPointerDownHandler
{
    [SerializeField] private Image _image;
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private float _matchDuration;
    [SerializeField] private float _flipDuration;
    
    public event Action OnTap;

    private CancellationTokenSource _cancellationTokenSource = new();
    
    public void SetImage(Sprite sprite)
    {
        _image.sprite = sprite;
    }

    public async UniTask Cover()
    {
        _cancellationTokenSource.Cancel();
        _cancellationTokenSource = new CancellationTokenSource();
        await Flip(false, _cancellationTokenSource.Token);
    }

    public async UniTask Uncover()
    {
        _cancellationTokenSource.Cancel();
        _cancellationTokenSource = new CancellationTokenSource();
        await Flip(true, _cancellationTokenSource.Token);
    }

    public async UniTask Match()
    {
        _canvasGroup.interactable = false;
        _canvasGroup.blocksRaycasts = false;
        
        var sequence = DOTween.Sequence();
        sequence.Append(transform.DOScale(1.2f, _matchDuration).SetEase(Ease.InOutSine));
        sequence.Join(_canvasGroup.DOFade(0f, _matchDuration));
        sequence.AppendInterval(0.1f);

        await sequence.AsyncWaitForCompletion();
        
        Destroy(gameObject);
    }

    private async UniTask Flip(bool isFrontVisible, CancellationToken token)
    {
        try
        {
            var halfDuration = _flipDuration / 2f;
            
            SoundManager.Instance.Play(SoundType.Flip);

            await transform.DOScaleX(0f, halfDuration).SetEase(Ease.InQuad).ToUniTask(cancellationToken: token);

            _image.gameObject.SetActive(isFrontVisible);

            await transform.DOScaleX(1f, halfDuration).SetEase(Ease.OutQuad).ToUniTask(cancellationToken: token);
        }
        catch (OperationCanceledException e)
        {
            transform.localScale = Vector3.one;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        OnTap?.Invoke();
    }

    public void Dispose()
    {
    }
}
