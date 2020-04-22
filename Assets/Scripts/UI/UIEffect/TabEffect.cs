using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.ProceduralImage;

public class TabEffect : MonoBehaviour
{
    [SerializeField] private Button[] m_Buttons;
    [SerializeField] private ProceduralImage m_Selector;
    private int m_Index = 0;

    private void Reset()
    {
        m_Selector = transform.Find("Selector")?.GetComponent<ProceduralImage>();
        m_Buttons = GetComponentsInChildren<Button>();
        
        if (m_Buttons.Length <= 0)
        {
            return;
        }

        var selectorRect = (m_Selector.transform as RectTransform);
        var buttonRect = (m_Buttons[0].transform as RectTransform);
        selectorRect.sizeDelta = new Vector3(buttonRect.sizeDelta.x, selectorRect.sizeDelta.y, 0);
    }

    public void Init(int width)
    {
        m_Index = 2;
        var selectorRect = (m_Selector.transform as RectTransform);
        selectorRect.sizeDelta = new Vector3(width, selectorRect.sizeDelta.y, 0);
        
        m_Selector.transform.localPosition = new Vector3(
            m_Buttons[2].transform.position.x, 
            m_Selector.transform.localPosition.y, 
            0);
    }

    public void SetIndex(int index)
    {
        if (index < 0
            || index >= m_Buttons.Length
            || m_Index == index)
        {
            return;
        }

        m_Index = index;

        m_Selector.transform.DOMoveX(m_Buttons[index].transform.position.x, 0.25f)
            .SetEase(Ease.OutQuad);
        Debug.Log(m_Buttons[index].transform.position.x);
    }
}