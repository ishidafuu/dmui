﻿using System.Collections.Generic;
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
                
                // Trueが帰ってきたらTargetLayerにも伝播
                // 一旦無し
                switch (touch.Type)
                {
                    case EnumTouchType.Click:
                        ret = OnClick(sounder, touch);
                        break;
                    case EnumTouchType.Down:
                        ret = touch.Listener.Part.OnTouchDown(touch);
                        // if (ret) touch.Listener.Part.TargetLayer?.OnTouchDown(touch);
                        break;
                    case EnumTouchType.Up:
                        ret = touch.Listener.Part.OnTouchUp(touch);
                        // if (ret) touch.Listener.Part.TargetLayer?.OnTouchUp(touch);
                        break;
                    case EnumTouchType.Drag:
                        ret = touch.Listener.Part.OnDrag(touch);
                        // if (ret) touch.Listener.Part.TargetLayer?.OnDrag(touch);
                        break;
                    case EnumTouchType.BeginDrag:
                        ret = touch.Listener.Part.OnBeginDrag(touch);
                        // if (ret) touch.Listener.Part.TargetLayer?.OnBeginDrag(touch);
                        break;
                    case EnumTouchType.EndDrag:
                        ret = touch.Listener.Part.OnEndDrag(touch);
                        // if (ret) touch.Listener.Part.TargetLayer?.OnEndDrag(touch);
                        break;
                    case EnumTouchType.Drop:
                        ret = touch.Listener.Part.OnDrop(touch);
                        // if (ret) touch.Listener.Part.TargetLayer?.OnDrop(touch);
                        break;
                    case EnumTouchType.None:
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
            
            // if (ret) touch.Listener.Part.TargetLayer?.OnClick(touch, se);
            
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

        public void Enqueue(UITouchListener listener, EnumTouchType type, PointerEventData pointer)
        {
            if (listener.Layer == null)
            {
                return;
            }

            m_TouchEvents.Enqueue(new TouchEvent(listener, type, pointer));
        }
    }
}