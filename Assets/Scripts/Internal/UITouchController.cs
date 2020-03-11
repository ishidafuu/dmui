using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace DM
{
    public class UITouchController
    {
        private readonly Queue<TouchEvent> m_TouchEvents;
        // ON/OFFは回数カウントされているので、OFF2回入ったら、ONを2回呼ばないとタッチ可能に戻らない
        private int m_TouchOffCount;

        public UITouchController()
        {
            m_TouchEvents = new Queue<TouchEvent>();
            
        }

        public void CallTouchEvents(int untouchableIndex, bool isScreenTouchable, ISounder sounder)
        {
            if (m_TouchEvents.Count == 0)
            {
                return;
            }

            while (m_TouchEvents.Count > 0)
            {
                TouchEvent touch = m_TouchEvents.Dequeue();

                if (!CheckTouchable(untouchableIndex, isScreenTouchable, touch))
                {
                    continue;
                }

                UIPart part = touch.Listener.Part;
                GameObject listenerObject = touch.Listener.gameObject;
                bool ret = false;
                
                switch (touch.Type)
                {
                    case TouchType.Click:
                    {
                        ret = OnClick(sounder, part, touch, listenerObject);
                        break;
                    }
                    case TouchType.Down:
                    {
                        ret = part.OnTouchDown(touch.Listener.gameObject.name, listenerObject, touch.Pointer);
                        break;
                    }
                    case TouchType.Up:
                    {
                        ret = part.OnTouchUp(touch.Listener.gameObject.name, listenerObject, touch.Pointer);
                        break;
                    }
                    case TouchType.Drag:
                    {
                        ret = part.OnDrag(touch.Listener.gameObject.name, listenerObject, touch.Pointer);
                        break;
                    }
                    case TouchType.None:
                        break;
                    default: break;
                }

                if (!ret)
                {
                    continue;
                }

                m_TouchEvents.Clear();
                break;
            }
        }

        private static bool OnClick(ISounder sounder, UIPart part, TouchEvent touch, GameObject listenerObject)
        {
            UISound se = new UISound();
            bool ret = part.OnClick(touch.Listener.gameObject.name, listenerObject, touch.Pointer, se);
            
            if (!ret || sounder == null)
            {
                return ret;
            }

            if (!string.IsNullOrEmpty(se.m_PlayName))
            {
                sounder.PlayClickSE(se.m_PlayName);
            }
            else
            {
                sounder.PlayDefaultClickSE();
            }

            return true;
        }

        private static bool CheckTouchable(int untouchableIndex, bool isScreenTouchable, TouchEvent touch)
        {
            if (touch.Listener.Layer == null)
            {
                return false;
            }

            return isScreenTouchable
                             && touch.Listener.Layer.IsTouchable()
                             && untouchableIndex < touch.Listener.Layer.SiblingIndex;
        }

        public void SetScreenTouchableByLayer(UIBaseLayer layer, bool enable,
            IEnumerable<BaseRaycaster> rayCasterComponents)
        {
            if (layer == null)
            {
                return;
            }

            if (enable)
            {
                if (m_TouchOffCount <= 0)
                {
                    return;
                }

                m_TouchOffCount--;
                layer.ScreenTouchOffCount--;
                if (m_TouchOffCount != 0)
                {
                    return;
                }

                foreach (BaseRaycaster rayCaster in rayCasterComponents)
                {
                    rayCaster.enabled = true;
                }
            }
            else
            {
                if (m_TouchOffCount == 0)
                {
                    foreach (BaseRaycaster rayCaster in rayCasterComponents)
                    {
                        rayCaster.enabled = false;
                    }
                }

                m_TouchOffCount++;
                layer.ScreenTouchOffCount++;
            }
        }
        
        public void Enqueue(UITouchListener listener, TouchType type, PointerEventData pointer)
        {
            if (listener.Layer == null)
            {
                return;
            }

            m_TouchEvents.Enqueue(new TouchEvent(listener, type, pointer));
        }
    }
}