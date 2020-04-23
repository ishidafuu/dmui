using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using UnityEngine.UI.ProceduralImage;

public class TabView : MonoBehaviour
{
    [SerializeField] public ButtonView[] m_ButtonViews;
    [SerializeField] public ProceduralImage m_Selector;
    // private int m_Index = 0;

    private void Reset()
    {
        m_Selector = transform.Find("Selector")?.GetComponent<ProceduralImage>();
        m_ButtonViews = GetComponentsInChildren<ButtonView>();
        
        if (m_ButtonViews.Length <= 0)
        {
            return;
        }

        var selectorRect = (m_Selector.transform as RectTransform);
        var buttonRect = (m_ButtonViews[0].transform as RectTransform);
        selectorRect.sizeDelta = new Vector3(buttonRect.sizeDelta.x, selectorRect.sizeDelta.y, 0);
    }

    public void Init(int width)
    {
        // m_Index = 2;
        var selectorRect = (m_Selector.transform as RectTransform);
        selectorRect.sizeDelta = new Vector3(width, selectorRect.sizeDelta.y, 0);
        
        // m_Selector.transform.localPosition = new Vector3(
        //     m_ButtonViews[2].transform.position.x, 
        //     m_Selector.transform.localPosition.y, 
        //     0);
    }

    public void SetIndex(int index)
    {
        // if (index < 0
        //     || index >= m_ButtonViews.Length
        //     || m_Index == index)
        // {
        //     return;
        // }
        //
        // m_Index = index;
        //
        // m_Selector.transform.DOMoveX(m_ButtonViews[index].transform.position.x, 0.25f)
        //     .SetEase(Ease.OutQuad);
        // Debug.Log(m_ButtonViews[index].transform.position.x);
    }
}