using System.Collections.Generic;

namespace DM
{
    public partial class UIController
    {
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

            bool isAdd = RunAddLayer();
            bool isRemove = RunRemoveLayer();

            if (!isRemove && !isAdd)
            {
                return;
            }

            RunRefreshLayer();

            if (isRemove && m_FadeController.IsHidden())
            {
                Unload();
            }

            if (m_AddingLayerList.Count != 0 || m_RemovingLayerList.Count != 0)
            {
                return;
            }

            RunPlayBgm();
            m_FadeController.FadeOut(Remove);
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
            bool isInsert = false;

            if (m_AddingLayerList.Count <= 0)
            {
                return false;
            }

            List<UIBaseLayer> list = m_AddingLayerList;
            m_AddingLayerList = new List<UIBaseLayer>();
            bool isFadeIn = m_FadeController.IsFadeIn();
            foreach (var layer in list)
            {
                if (!isFadeIn && layer.State == BaseLayerState.InFading)
                {
                    StartCoroutine(layer.Load());
                }

                if (layer.IsNotYetLoaded() || (isFadeIn && !layer.Base.IsActiveWithoutFade()))
                {
                    m_AddingLayerList.Add(layer);
                    continue;
                }

                if (layer.Activate())
                {
                    isInsert = true;
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

            bool isEject = false;
            bool isLoading = m_AddingLayerList.Exists(layer => (layer.IsNotYetLoaded()));

            List<UIBaseLayer> list = m_RemovingLayerList;
            m_RemovingLayerList = new List<UIBaseLayer>();
            bool isFadeIn = m_FadeController.IsFadeIn();
            foreach (var layer in list)
            {
                if (!isFadeIn && layer.State == BaseLayerState.OutFading)
                {
                    layer.Remove();
                }

                if (layer.State != BaseLayerState.Removing || isLoading)
                {
                    m_RemovingLayerList.Add(layer);
                    continue;
                }

                m_LayerController.Remove(layer);
                layer.Destroy();
                isEject = true;
            }

            return isEject;
        }

        private void RunRefreshLayer()
        {
            bool isVisible = true;
            bool isTouchable = true;
            UIBaseLayer frontLayer = null;
            int index = m_UiLayers.childCount - 1;

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

        private void RunPlayBgm()
        {
            if (m_Implements.Sounder == null)
            {
                return;
            }

            string bgm = "";
            m_LayerController.ForEachAnything(l =>
            {
                if (!StateFlags.s_Map[l.State].m_IsVisible)
                {
                    return;
                }

                if (!string.IsNullOrEmpty(bgm))
                {
                    return;
                }

                if (!string.IsNullOrEmpty(l.Base.Bgm))
                {
                    bgm = l.Base.Bgm;
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