using System;
using System.Collections;
using System.Collections.Generic;
using EnhancedUI.EnhancedScroller;
using UnityEngine;

namespace DM
{
    public class HomeScrollerView : MonoBehaviour
    {
        [SerializeField] public EnhancedScroller m_EnhancedScroller;
        [SerializeField] public TabView m_TabView;

        private float m_ScrollWidth;
        private Transform m_SelectorTransform;
        private void Start()
        {
            m_ScrollWidth = (m_TabView.transform as RectTransform).rect.width / 5 * 4;
            m_SelectorTransform = m_TabView.m_Selector.transform;
        } 

        private void Update()
        {
            var localPosition = m_SelectorTransform.localPosition;
            var horizontalNormalizedPosition = m_EnhancedScroller.ScrollRect.horizontalNormalizedPosition;
            float newX = (horizontalNormalizedPosition - 0.5f) * m_ScrollWidth;

            if (Math.Abs(newX - localPosition.x) < 0.01f)
            {
                return;
            }

            Vector3 newPosition = localPosition;
            newPosition.x = newX;
            m_SelectorTransform.localPosition = newPosition;

        }
    }
}