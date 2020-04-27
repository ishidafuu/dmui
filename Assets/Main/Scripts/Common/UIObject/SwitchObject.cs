using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using JoshH.UI;
using UnityEngine.UI.ProceduralImage;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class SwitchObject : MonoBehaviour,
        // IMoveHandler,
        // IPointerExitHandler,
        IPointerDownHandler,
        IPointerUpHandler
    // IPointerEnterHandler,
    // ISelectHandler,
    // IDeselectHandler
{
    [SerializeField] private ProceduralImage m_Base;
    [SerializeField] private ProceduralImage m_Effect;
    [SerializeField] private ProceduralImage m_Top;
    [SerializeField] private ProceduralImage m_Bottom;
    [SerializeField] public Button m_Button;
    private Tween m_TweenBase;
    private Tween m_TweenEffect;
    private Tween m_TweenShadow;
    private Sequence m_Sequence;

    private RectTransform m_BaseRect;
    private RectTransform m_EffectRect;
    private bool m_IsSelected;

    private const float DURATION = 0.25f;

    private void Reset()
    {
        m_Base = GetComponent<ProceduralImage>();
        m_Top = transform.Find("Top")?.GetComponent<ProceduralImage>();
        m_Effect = m_Top?.transform.Find("Effect")?.GetComponent<ProceduralImage>();
        m_Bottom = transform.Find("Bottom")?.GetComponent<ProceduralImage>();
        m_Button = GetComponent<Button>();
    }

    private void Start()
    {
        // m_BaseRect = (m_Base.transform as RectTransform);
        // m_EffectRect = (m_Effect.transform as RectTransform);
    }

    public void SetIntractable(bool isActive)
    {
        m_Button.interactable = isActive;

        // float endValue = (isActive)
        //     ? 0
        //     : 0.25f;
        // m_TweenShadow?.Kill();
        // m_TweenShadow = DOTween.ToAlpha(() => m_Shadow.color,
        //         (x) => m_Shadow.color = x, endValue, 0.2f)
        //     .SetEase(Ease.InQuart);
    }

    public void Switching()
    {
        // if (m_IsSelected == isSelect)
        // {
        //     return;
        // }

        m_IsSelected = !m_IsSelected;

        const float MOVE_X = 8;
        
        var topEndValue = (m_IsSelected)
            ? new Color(0, 0, 1, 1)
            : new Color(1, 1, 1, 1);
        var bottomEndValue = (m_IsSelected)
            ? new Color(topEndValue.r + 0.8f, topEndValue.g + 0.8f, topEndValue.b + 0.8f, 1)
            :new Color(topEndValue.r - 0.5f, topEndValue.g - 0.5f, topEndValue.b - 0.5f, 1);
        var moveX = (m_IsSelected)
            ? +8
            : -8;
        
        m_Top.transform.DOLocalMoveX(moveX, DURATION)
            .SetEase(Ease.OutQuad);

        DOTween.To(() => m_Top.color, x => m_Top.color = x, topEndValue, DURATION)
            .SetEase(Ease.OutQuart);
        DOTween.To(() => m_Bottom.color, x => m_Bottom.color = x, bottomEndValue, DURATION)
            .SetEase(Ease.OutQuart);

        m_Effect.color = new Color(topEndValue.r, topEndValue.g, topEndValue.b, m_Effect.color.a);

        // float endValue = (isActive)
        //     ? 0
        //     : 0.25f;
        // m_TweenShadow?.Kill();
        // m_TweenShadow = DOTween.ToAlpha(() => m_Shadow.color,
        //         (x) => m_Shadow.color = x, endValue, 0.2f)
        //     .SetEase(Ease.InQuart);
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