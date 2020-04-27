using System;
using EnhancedUI.EnhancedScroller;
using UnityEngine;
using UnityEngine.Serialization;

namespace DM
{
    public class HomeScrollerView : MonoBehaviour
    {
        [SerializeField] public EnhancedScroller m_EnhancedScroller;
        [FormerlySerializedAs("m_TabView")] [SerializeField] public TabObject m_TabObject;

        private float m_ScrollWidth;
        private Transform m_CursorTransform;

        private void Start()
        {
            m_ScrollWidth = (m_TabObject.transform as RectTransform).rect.width / 5 * 4;
            m_CursorTransform = m_TabObject.m_Cursor.transform;
        }
        
        private void Update()
        {
            // カーソル位置調整
            var localPosition = m_CursorTransform.localPosition;
            var horizontalNormalizedPosition = m_EnhancedScroller.ScrollRect.horizontalNormalizedPosition;
            float newX = (horizontalNormalizedPosition - 0.5f) * m_ScrollWidth;

            if (Math.Abs(newX - localPosition.x) < 0.01f)
            {
                return;
            }

            Vector3 newPosition = localPosition;
            newPosition.x = newX;
            m_CursorTransform.localPosition = newPosition;
        }
    }
}