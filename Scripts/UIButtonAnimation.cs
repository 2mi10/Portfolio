using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class UIButtonAnimation : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    private Vector3 defaultScale;

    void Start()
    {
        defaultScale = transform.localScale;
    }

    // 拡大
    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.DOScale(defaultScale * 1.1f, 0.2f).SetEase(Ease.OutQuad);
    }

    // 元に戻る
    public void OnPointerExit(PointerEventData eventData)
    {
        transform.DOScale(defaultScale, 0.2f).SetEase(Ease.OutQuad);
    }

    // 縮む
    public void OnPointerDown(PointerEventData eventData)
    {
        transform.DOScale(defaultScale * 0.95f, 0.1f)
                 .SetEase(Ease.OutQuad)
                 .OnComplete(() => transform.DOScale(defaultScale * 1.1f, 0.1f));
    }
}
