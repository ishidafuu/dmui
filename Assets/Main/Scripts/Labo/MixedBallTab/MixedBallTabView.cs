using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using UnityEngine.UI.ProceduralImage;

public class MixedBallTabView : MonoBehaviour
{
    [SerializeField] public MixedBallItemView[] m_ButtonViews;
    [SerializeField] public GameObject m_DraggingLayer;
    
    private void Reset()
    {
        m_ButtonViews = GetComponentsInChildren<MixedBallItemView>();
    }
    
    private void Start()
    {
        for (int i = 0; i < m_ButtonViews.Length; i++)
        {
            m_ButtonViews[i].Slot = i;
            m_ButtonViews[i].SetText(0);
        }
    }
    
    public void ChangeMixedBall(int index)
    {
        foreach (var item in m_ButtonViews)
        {
            item.SetText(index);
        }
    }
}