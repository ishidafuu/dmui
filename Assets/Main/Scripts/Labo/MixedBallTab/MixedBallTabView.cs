using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using UnityEngine.UI.ProceduralImage;

public class MixedBallTabView : MonoBehaviour
{
    [SerializeField] public MixedBallItemView[] m_ButtonViews;
    
    private void Reset()
    {
        m_ButtonViews = GetComponentsInChildren<MixedBallItemView>();
    }
}