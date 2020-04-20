using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UI.ProceduralImage;

public class CheckEffect : MonoBehaviour,
    IPointerDownHandler,
    IPointerUpHandler
{
    [SerializeField] private ProceduralImage m_Effect;
    [SerializeField] private ProceduralImage m_Frame;
    [SerializeField] public Button m_Button;
    private bool m_IsSelected;

    private const float DURATION = 0.25f;

    private void Reset()
    {
        m_Frame = transform.Find("Frame")?.GetComponent<ProceduralImage>();
        m_Effect = transform.Find("Effect")?.GetComponent<ProceduralImage>();
        m_Button = GetComponent<Button>();
    }

    public void SetIntractable(bool isActive)
    {
        m_Button.interactable = isActive;
    }

    public void Switching()
    {
        m_IsSelected = !m_IsSelected;

        var colorEndValue = (m_IsSelected)
            ? new Color(0, 0, 1, 1)
            : new Color(0.5f, 0.5f, 0.5f, 1);

        var borderWidthEndValue = (m_IsSelected)
            ? 16
            : 2;

        DOTween.To(() => m_Frame.color, x => m_Frame.color = x, colorEndValue, DURATION)
            .SetEase(Ease.OutQuart);
        DOTween.To(() => m_Frame.BorderWidth, x => m_Frame.BorderWidth = x, borderWidthEndValue, DURATION)
            .SetEase(Ease.OutQuart);

        m_Effect.color = new Color(colorEndValue.r, colorEndValue.g, colorEndValue.b, m_Effect.color.a);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Debug.Log("OnPointerUp");
        if (!m_Button.IsInteractable())
        {
            return;
        }

        DOTween.ToAlpha(() => m_Effect.color, x => m_Effect.color = x, 0, DURATION)
            .SetEase(Ease.OutQuart);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!m_Button.IsInteractable())
        {
            return;
        }

        DOTween.ToAlpha(() => m_Effect.color, x => m_Effect.color = x, 0.25f, DURATION)
            .SetEase(Ease.OutQuart);
    }
}