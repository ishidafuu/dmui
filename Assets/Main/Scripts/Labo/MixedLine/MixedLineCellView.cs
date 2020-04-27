﻿using EnhancedScrollerDemos.CellEvents;
using UnityEngine;
using UnityEngine.UI;
using EnhancedUI.EnhancedScroller;
using UnityEngine.Serialization;

namespace DM
{
    public class MixedLineCellView : EnhancedScrollerCellView
    {
        [SerializeField] public MixedLineScrollerView m_MixedLineScrollerView;
        [SerializeField] public BallScrollerController m_MixedLineScrollerController;
        [SerializeField] public ButtonObject m_LaboButtonObject;
        [HideInInspector] public HomeScrollerView m_HomeScrollerView;
       
        public void SetData(Data data)
        {
            // m_HomeScrollerController = homeScrollerController;
            // _data = data;
            // someTextText.text = (_data.hour == 0 ? "Midnight" : string.Format("{0} 'o clock", _data.hour.ToString()));
        }
    }
}