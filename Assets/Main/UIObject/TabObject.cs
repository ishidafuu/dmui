using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using UnityEngine.UI.ProceduralImage;

public class TabObject : MonoBehaviour
{
    [SerializeField] public ButtonObject[] m_ButtonViews;
    [SerializeField] public ProceduralImage m_Cursor;

    private void Reset()
    {
        m_Cursor = transform.Find("Cursor")?.GetComponent<ProceduralImage>();
        m_ButtonViews = GetComponentsInChildren<ButtonObject>();
        
        if (m_ButtonViews.Length <= 0)
        {
            return;
        }

        var cursorRect = m_Cursor.transform as RectTransform;
        var buttonRect = m_ButtonViews[0].transform as RectTransform;
        cursorRect.sizeDelta = new Vector3(buttonRect.sizeDelta.x, cursorRect.sizeDelta.y, 0);
    }

    // public void Init(int width)
    // {
    //     var cursorRect = m_Cursor.transform as RectTransform;
    //     cursorRect.sizeDelta = new Vector3(width, cursorRect.sizeDelta.y, 0);
    // }
    //
    // public void SetIndex(int index)
    // {
    //     // if (index < 0
    //     //     || index >= m_ButtonViews.Length
    //     //     || m_Index == index)
    //     // {
    //     //     return;
    //     // }
    //     //
    //     // m_Index = index;
    //     //
    //     // m_Selector.transform.DOMoveX(m_ButtonViews[index].transform.position.x, 0.25f)
    //     //     .SetEase(Ease.OutQuad);
    //     // Debug.Log(m_ButtonViews[index].transform.position.x);
    // }
}