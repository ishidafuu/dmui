﻿namespace DM
{
    public partial class UIController
    {
        private void LateUpdate()
        {
            m_LayerController.ForEachOnlyActive(layer =>
            {
                if (layer.Base.IsScheduleUpdate)
                {
                    layer.Base.OnLateUpdate();
                }
            });
        }

        private void Update()
        {
            m_LayerController.ForEachOnlyActive(layer =>
            {
                if (layer.Base.IsScheduleUpdate)
                {
                    layer.Base.OnUpdate();
                }
            });

            m_TouchController.CallTouchEvents(FindUntouchableIndex(), IsScreenTouchable(), Implements.Sounder);
            m_DispatchController.CallDispatchedEvents(m_LayerController);

            bool isInsert = RunAddLayer();
            bool isEject = RunRemoveLayer();

            if (isEject || isInsert)
            {
                RunRefresh(isEject);
            }
        }

        private void RunRefresh(bool isEject)
        {
            RefreshLayer();

            if (IsUnloadTiming(isEject))
            {
                Unload();
            }

            if (AnyChangingLayer())
            {
                return;
            }

            PlayBgm();
            m_FadeController.FadeOut(Remove);
        }

        private bool AnyChangingLayer()
        {
            return m_AddingLayerList.Count != 0 || m_RemovingLayerList.Count != 0;
        }

        private bool IsUnloadTiming(bool isEject)
        {
            return isEject && m_FadeController.IsHidden() && m_RemovingLayerList.Count == 0;
        }


        private int FindUntouchableIndex()
        {
            int index = -1;
            m_LayerController.ForEachOnlyActive(layer =>
            {
                if (index >= 0)
                {
                    return;
                }

                if (layer.Base.IsSystemUntouchable())
                {
                    index = layer.SiblingIndex - 1;
                }
            });

            return index;
        }

        private bool IsScreenTouchable()
        {
            return m_RayCasterComponents.Count != 0 && m_RayCasterComponents[0].enabled;
        }

        private bool RunAddLayer()
        {
            if (m_AddingLayerList.Count <= 0)
            {
                return false;
            }

            bool isInsert = InsertLayer();

            return isInsert;
        }

        private bool InsertLayer()
        {
            bool isFadeIn = m_FadeController.IsFadeIn();
            foreach (var layer in m_AddingLayerList)
            {
                if (!isFadeIn && layer.State == EnumLayerState.InFading)
                {
                    StartCoroutine(layer.Load());
                }
            }

            bool isInsert = false;
            int listCount = m_AddingLayerList.Count;
            int index = 0;
            for (int i = 0; i < listCount; i++)
            {
                var layer = m_AddingLayerList[index];

                // ロード中・ロード待ち、もしくはフェード待ち
                if (layer.IsNotYetLoaded() || (isFadeIn && !layer.Base.IsActiveWithoutFade()))
                {
                    index++;
                }
                else
                {
                    if (layer.SetActivate())
                    {
                        isInsert = true;
                        m_AddingLayerList.RemoveAt(index);
                    }
                    else
                    {
                        index++;
                    }
                }
            }

            return isInsert;
        }

        private bool RunRemoveLayer()
        {
            if (m_RemovingLayerList.Count <= 0)
            {
                return false;
            }

            bool isEject = EjectLayer();

            return isEject;
        }


        private bool EjectLayer()
        {
            bool isFadeIn = m_FadeController.IsFadeIn();
            foreach (var layer in m_RemovingLayerList)
            {
                if (!isFadeIn && layer.State == EnumLayerState.OutFading)
                {
                    layer.Remove();
                }
            }

            bool isLoading = m_AddingLayerList.Exists(layer => (layer.IsNotYetLoaded()));
            bool isEject = false;
            int listCount = m_AddingLayerList.Count;
            int index = 0;
            for (int i = 0; i < listCount; i++)
            {
                var layer = m_RemovingLayerList[index];

                if (layer.State != EnumLayerState.Removing || isLoading)
                {
                    index++;
                }
                else
                {
                    m_RemovingLayerList.RemoveAt(index);
                    m_LayerController.Remove(layer);
                    layer.Destroy();
                    isEject = true;
                }
            }

            return isEject;
        }

        private void RefreshLayer()
        {
            bool isVisible = true;
            bool isTouchable = true;
            UIBaseLayer frontLayer = null;
            int index = m_UILayers.childCount - 1;

            m_LayerController.ForEachAnything(layer =>
            {
                if (layer.IsNotYetLoaded())
                {
                    return;
                }

                bool preVisible = layer.IsVisible();
                bool preTouchable = layer.IsTouchable();
                layer.SetVisible(isVisible);
                layer.SetTouchable(isTouchable);

                if (!preVisible && isVisible)
                {
                    layer.Base.OnReVisible();
                }

                if (!preTouchable && isTouchable)
                {
                    layer.Base.OnReTouchable();
                }

                isVisible &= layer.Base.IsBackVisible();
                isTouchable &= layer.Base.IsBackTouchable();

                layer.SiblingIndex = index--;

                if (frontLayer != null)
                {
                    frontLayer.BackLayer = layer;
                    frontLayer.CallSwitchBack();
                }

                layer.FrontLayer = frontLayer;
                layer.CallSwitchFront();

                layer.BackLayer = null;
                frontLayer = layer;
            });
        }

        private void PlayBgm()
        {
            if (m_Implements.Sounder == null)
            {
                return;
            }

            string bgm = "";
            m_LayerController.ForEachAnything(layer =>
            {
                if (!StateFlags.IsVisible(layer.State))
                {
                    return;
                }

                if (!string.IsNullOrEmpty(bgm))
                {
                    return;
                }

                if (!string.IsNullOrEmpty(layer.Base.Bgm))
                {
                    bgm = layer.Base.Bgm;
                }
            });

            if (string.IsNullOrEmpty(bgm))
            {
                m_Implements.Sounder.StopBGM();
            }
            else
            {
                m_Implements.Sounder.PlayBGM(bgm);
            }
        }
    }
}