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
            while (m_TouchEvents.Count > 0)
            {
                TouchEvent touch = m_TouchEvents.Dequeue();

                if (!CheckTouchable(untouchableIndex, isScreenTouchable, touch))
                {
                    continue;
                }
                
                bool ret = false;
                
                switch (touch.Type)
                {
                    case TouchType.Click:
                        ret = OnClick(sounder, touch);
                        break;
                    case TouchType.Down:
                        ret = touch.Listener.Part.OnTouchDown(touch);
                        break;
                    case TouchType.Up:
                        ret = touch.Listener.Part.OnTouchUp(touch);
                        break;
                    case TouchType.Drag:
                        ret = touch.Listener.Part.OnDrag(touch);
                        break;
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

        private static bool OnClick(ISounder sounder, TouchEvent touch)
        {
            UISound se = new UISound();
            bool ret = touch.Listener.Part.OnClick(touch, se);
            
            if (!ret || sounder == null)
            {
                return ret;
            }

            PlayClickSound(sounder, se);

            return true;
        }

        private static void PlayClickSound(ISounder sounder, UISound se)
        {
            if (!string.IsNullOrEmpty(se.m_PlayName))
            {
                sounder.PlayClickSE(se.m_PlayName);
            }
            else
            {
                sounder.PlayDefaultClickSE();
            }
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

        public void SetScreenTouchable(UIBaseLayer layer, bool enable,
            IEnumerable<BaseRaycaster> rayCasterComponents)
        {
            if (layer == null)
            {
                return;
            }

            if (enable)
            {
                SetTouchableEnable(layer, rayCasterComponents);
            }
            else
            {
                SetTouchableDisable(layer, rayCasterComponents);
            }
        }

        private void SetTouchableEnable(UIBaseLayer layer, IEnumerable<BaseRaycaster> rayCasterComponents)
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

        private void SetTouchableDisable(UIBaseLayer layer, IEnumerable<BaseRaycaster> rayCasterComponents)
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