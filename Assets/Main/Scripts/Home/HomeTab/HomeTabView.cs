using System;
using UnityEngine;

namespace DM
{
    public class HomeTabView : TabObject
    {
        private float m_ScrollWidth;
        private Transform m_CursorTransform;

        private void Start()
        {
            m_ScrollWidth = (transform as RectTransform).rect.width / 5 * 4;
            m_CursorTransform = m_Cursor.transform;
        }

        public void UpdateCursorPosition(float horizontalNormalizedPosition)
        {
            // カーソル位置調整
            Vector3 localPosition = m_CursorTransform.localPosition;
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