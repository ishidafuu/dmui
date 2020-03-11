// ----------------------------------------------------------------------
// DMUIFramework
// Copyright (c) 2018 Takuya Nishimura (tnishimu)
//
// This software is released under the MIT License.
// https://opensource.org/licenses/mit-license.php
// ----------------------------------------------------------------------

using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace DM
{
    public class UILayerTouchListener : UITouchListener, ICanvasRaycastFilter
    {
        private Vector2 m_ScreenPoint = Vector2.zero;
        private bool m_Pressed = false;
        private bool m_RayCasted = false;

        public bool IsRaycastLocationValid(Vector2 screenPoint, Camera eventCamera)
        {
            m_ScreenPoint = screenPoint;
            m_RayCasted = true;
            return false;
        }

        public void Update()
        {
            if (!m_RayCasted || !Layer.IsTouchable())
            {
                m_Pressed = false;
                return;
            }

            if (Input.touchCount > 0)
            {
                switch (Input.touches[0].phase)
                {
                    case TouchPhase.Began:
                        m_Pressed = true;
                        OnPointerDown(CreatePointerEventData());
                        return;
                    case TouchPhase.Ended when !m_Pressed:
                        return;
                    case TouchPhase.Ended:
                        m_Pressed = false;
                        OnPointerUp(CreatePointerEventData());
                        return;
                    case TouchPhase.Moved:
                        break;
                    case TouchPhase.Stationary:
                        break;
                    case TouchPhase.Canceled:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            if (Input.GetMouseButtonDown(0))
            {
                m_Pressed = true;
                OnPointerDown(CreatePointerEventData());
                return;
            }

            if (Input.GetMouseButtonUp(0))
            {
                if (!m_Pressed)
                {
                    return;
                }

                m_Pressed = false;
                OnPointerUp(CreatePointerEventData());
                return;
            }

            if (m_Pressed)
            {
                OnDrag(CreatePointerEventData());
            }
        }

        public void LateUpdate()
        {
            m_RayCasted = false;
        }

        private PointerEventData CreatePointerEventData()
        {
            PointerEventData data = new PointerEventData(null) {position = m_ScreenPoint};
            return data;
        }
    }
}