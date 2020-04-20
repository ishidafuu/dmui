using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using JoshH.UI;
using UnityEngine.UI.ProceduralImage;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class ButtonEffect : MonoBehaviour,
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
    [SerializeField] private ProceduralImage m_Shadow;
    [SerializeField] private UIGradient m_Gradient;
    [SerializeField] public Button m_Button;
    [SerializeField] public AudioSource m_AudioSource;
    private Tween m_TweenBase;
    private Tween m_TweenEffect;
    private Tween m_TweenShadow;
    private Sequence m_Sequence;

    private RectTransform m_BaseRect;
    private RectTransform m_EffectRect;
    private void Reset()
    {
        m_Base = GetComponent<ProceduralImage>();
        m_Effect = transform.Find("Effect")?.GetComponent<ProceduralImage>();
        m_Shadow = transform.Find("Shadow")?.GetComponent<ProceduralImage>();
        m_Gradient = GetComponent<UIGradient>();
        m_Button = GetComponent<Button>();
        m_AudioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        m_BaseRect = (m_Base.transform as RectTransform);
        m_EffectRect = (m_Effect.transform as RectTransform);
    }

    public void SetIntractable(bool isActive)
    {
        m_Button.interactable = isActive;

        float endValue = (isActive)
            ? 0
            : 0.25f;
        m_TweenShadow?.Kill();
        m_TweenShadow = DOTween.ToAlpha(() => m_Shadow.color,
                (x) => m_Shadow.color = x, endValue, 0.2f)
            .SetEase(Ease.InQuart);
        
    }
    
    public void OnPointerUp(PointerEventData eventData)
    {
        Debug.Log("OnPointerUp");
        if (!m_Button.IsInteractable())
        {
            return;
        }
        
        m_TweenEffect?.Kill();
        m_TweenEffect = DOTween.ToAlpha(() => m_Effect.color,
                (x) => m_Effect.color = x, 0, 0.2f)
            .SetEase(Ease.InQuart);
        
        // m_Sequence?.Kill();
        // m_Sequence = DOTween.Sequence();
        // m_Sequence.Append
        // (
        //     DOTween.ToAlpha(() => m_Effect.color,
        //             (x) => m_Effect.color = x, 0, 0.2f)
        //         .SetEase(Ease.InQuart) 
        // );
        // m_Sequence.Play();
    }
    
    // public void OnPointerExit(PointerEventData eventData)
    // {
    //     Debug.Log("OnPointerExit");
    //     
    //     m_TweenEffect?.Kill();
    //     m_TweenEffect = DOTween.ToAlpha(() => m_Effect.color,
    //             (x) => m_Effect.color = x, 0, 0.2f)
    //         .SetEase(Ease.InQuart);
    // }
    //
    public void OnPointerDown(PointerEventData eventData)
    {
        if (!m_Button.IsInteractable())
        {
            return;
        }
        
        Debug.Log("OnPointerDown");
        // transform
        //     .DOScale(endValue: new Vector3(2.0f, 1f, 1), duration: 0.5f)
        //     .SetEase(Ease.OutQuart);
        // m_TweenBase.Kill();
        // m_Base.BorderWidth = (transform as RectTransform).sizeDelta.y;
        // m_TweenBase = DOTween.To(() => m_Base.BorderWidth,
        //         (x) => m_Base.BorderWidth = x, 3, 0.5f)
        //     .SetEase(Ease.OutQuart);
        
        m_Effect.color = new Color(1,1,1,0.5f);
        m_Effect.BorderWidth = m_BaseRect.sizeDelta.y;
        m_TweenEffect?.Kill();
        m_Sequence?.Kill();
        m_Sequence = DOTween.Sequence();
        m_EffectRect.position = eventData.pressPosition;
        // effectRect.sizeDelta = new Vector2(baseRect.sizeDelta.x / 10, baseRect.sizeDelta.y);
        m_EffectRect.localScale = Vector3.one;
        var endValue = new Vector3(12,12,1);
        // m_Sequence.Append
        // (
        //     DOTween.To(() => effectRect.sizeDelta,
        //             (x) => effectRect.sizeDelta = x, endValue, 0.5f)
        //         .SetEase(Ease.OutQuart) 
        // );
        m_Sequence.Append
        (
            DOTween.To(() => m_EffectRect.localScale,
                    (x) => m_EffectRect.localScale = x, endValue, 0.5f)
                .SetEase(Ease.OutQuad) 
        );
        // m_Sequence.Join
        // (
        //     DOTween.ToAlpha(() => m_Effect.color,
        //             (x) => m_Effect.color = x, 0, 0.5f)
        //         .SetEase(Ease.InQuart) 
        // );
        m_Sequence.Play();
    }

    
    // public void OnPointerClick(PointerEventData eventData)
    // {
    //     button.OnPointerClick(eventData);
    // }
    // public void OnSubmit(BaseEventData eventData)
    // {
    //     button.OnSubmit(eventData);
    // }

    // public void OnDeselect(BaseEventData eventData)
    // {
    //     Debug.Log("OnDeselect");
    // }
    //
    // public void OnMove(AxisEventData eventData)
    // {
    //     Debug.Log("OnMove");
    // }
    

    // public void OnPointerEnter(PointerEventData eventData)
    // {
    //     Debug.Log("OnPointerEnter");
    // }
    //



    // public void OnSelect(BaseEventData eventData)
    // {
    //     Debug.Log("OnSelect");
    // }
}