using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI.ProceduralImage;

public class MenuButtonObject : MonoBehaviour,
    IPointerDownHandler,
    IPointerUpHandler
{
    [SerializeField] private ProceduralImage m_Base;
    private Sequence m_Sequence;

    private void Reset()
    {
        m_Base = GetComponent<ProceduralImage>();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        DOTween.ToAlpha(() => m_Base.color, x => m_Base.color = x, 0, 0.2f)
            .SetEase(Ease.OutQuart);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        DOTween.ToAlpha(() => m_Base.color, x => m_Base.color = x, 0.25f, 0.2f)
            .SetEase(Ease.OutQuart);
    }
}