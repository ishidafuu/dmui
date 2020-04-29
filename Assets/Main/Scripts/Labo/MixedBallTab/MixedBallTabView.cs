using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using UnityEngine.UI.ProceduralImage;

public class MixedBallTabView : MonoBehaviour
{
    [SerializeField] public MixedBallItemView[] m_ButtonViews;
    [SerializeField] public ProceduralImage m_Cursor;

    private void Reset()
    {
        m_Cursor = transform.Find("Cursor")?.GetComponent<ProceduralImage>();
        m_ButtonViews = GetComponentsInChildren<MixedBallItemView>();
        
        if (m_ButtonViews.Length <= 0)
        {
            return;
        }

        var cursorRect = m_Cursor.transform as RectTransform;
        var buttonRect = m_ButtonViews[0].transform as RectTransform;
        cursorRect.sizeDelta = new Vector3(buttonRect.sizeDelta.x, cursorRect.sizeDelta.y, 0);
    }
}