using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

namespace DM
{
    public class UIController : MonoBehaviour
    {
        public const string LAYER_TOUCH_AREA_NAME = "LayerTouchArea";
        private Transform m_UiLayers;
        public Transform m_View3D;
        private List<BaseRaycaster> m_RayCasterComponents;
        private List<UIBaseLayer> m_AddingLayerList;
        private List<UIBaseLayer> m_RemovingLayerList;
        private UIBaseLayerList m_LayerList;
        private Queue<TouchEvent> m_TouchEvents;
        private Queue<DispatchedEvent> m_DispatchedEvents;
        private int m_TouchOffCount;

        private UIFadeController m_FadeController;

        private UIImplements m_Implements;
        public static UIImplements Implements => Instance.m_Implements;

        public static void SetImplement(IPrefabLoader prefabLoader, ISounder sounder, IFadeCreator fadeCreator)
        {
            Instance.m_Implements = new UIImplements(prefabLoader, sounder, fadeCreator);
        }

        private static UIController s_Instance;

        public UIController(UIImplements implements)
        {
            m_Implements = implements;
        }

        public static UIController Instance
        {
            get
            {
                if (s_Instance != null)
                {
                    return s_Instance;
                }

                s_Instance = FindObjectOfType<UIController>();

                s_Instance.m_AddingLayerList = new List<UIBaseLayer>();
                s_Instance.m_RemovingLayerList = new List<UIBaseLayer>();
                s_Instance.m_LayerList = new UIBaseLayerList();
                s_Instance.m_TouchEvents = new Queue<TouchEvent>();
                s_Instance.m_DispatchedEvents = new Queue<DispatchedEvent>();
                s_Instance.m_FadeController = new UIFadeController();

                if (s_Instance.m_RayCasterComponents == null
                    || s_Instance.m_UiLayers == null
                    || s_Instance.m_View3D == null)
                {
                    s_Instance.FindUIControllerItems();
                }

                UIBackAble.Sort();

                return s_Instance;
            }
        }

        public void FindUIControllerItems()
        {
            m_UiLayers = FindObjectOfType<UILayers>().transform;
            m_View3D = FindObjectOfType<View3D>().transform;

            BaseRaycaster[] rayCasters = FindObjectsOfType<BaseRaycaster>();
            m_RayCasterComponents = new List<BaseRaycaster>();
            foreach (BaseRaycaster item in rayCasters)
            {
                m_RayCasterComponents.Add(item);
            }
        }

        public void AddFront(UIBase ui)
        {
            if (ui == null)
            {
                return;
            }

            UIBaseLayer layer = new UIBaseLayer(ui, m_UiLayers);

            if (layer.Base.IsLoadingWithoutFade())
            {
                StartCoroutine(layer.Load());
            }

            if (m_FadeController.ShouldFadeByAdding(ui, m_LayerList))
            {
                m_FadeController.FadeIn(Implements, m_AddingLayerList, AddFront);
            }

            m_AddingLayerList.Add(layer);
            m_LayerList.AddOrInsert(layer);
        }

        public void Remove(UIBase uiBase)
        {
            if (uiBase == null)
            {
                return;
            }

            UIBaseLayer layer = m_LayerList.Find(uiBase);
            if (layer != null && layer.Inactive())
            {
                m_RemovingLayerList.Add(layer);
            }

            if (m_FadeController.ShouldFadeByRemoving(uiBase, m_LayerList, m_RemovingLayerList))
            {
                m_FadeController.FadeIn(Implements, m_AddingLayerList, AddFront);
            }
        }

        public void Replace(IEnumerable<UIBase> uiBases, UIGroup[] removeGroups = null)
        {
            HashSet<UIGroup> removes = (removeGroups == null)
                ? new HashSet<UIGroup>()
                : new HashSet<UIGroup>(removeGroups);

            foreach (var uiBase in uiBases)
            {
                removes.Add(uiBase.Group);
            }

            foreach (UIGroup uiGroup in removes)
            {
                IEnumerable<UIBaseLayer> layers = m_LayerList.FindLayers(uiGroup);
                foreach (var layer in layers)
                {
                    Remove(layer.Base);
                }
            }

            foreach (var uiBase in uiBases)
            {
                AddFront(uiBase);
            }
        }

        public void ListenTouch(UITouchListener listener, TouchType type, PointerEventData pointer)
        {
            if (listener.Layer == null)
            {
                return;
            }

            m_TouchEvents.Enqueue(new TouchEvent(listener, type, pointer));
        }

        public void Dispatch(string eventName, object param)
        {
            m_DispatchedEvents.Enqueue(new DispatchedEvent(eventName, param));
        }

        public void Back()
        {
            UIBaseLayer layer = null;
            foreach (var group in UIBackAble.s_Groups)
            {
                layer = m_LayerList.FindFrontLayerInGroup(group);
                if (layer != null)
                {
                    break;
                }
            }

            if (layer == null)
            {
                return;
            }

            bool ret = layer.Base.OnBack();
            if (ret)
            {
                Remove(layer.Base);
            }
        }

        public IEnumerator YieldAttachParts(UIBase uiBase, List<UIPart> parts)
        {
            UIBaseLayer layer = m_LayerList.Find(uiBase);
            if (layer == null)
            {
                yield break;
            }

            yield return layer.AttachParts(parts);
        }

        public void AttachParts(UIBase uiBase, List<UIPart> parts)
        {
            UIBaseLayer layer = m_LayerList.Find(uiBase);
            if (layer == null)
            {
                return;
            }

            StartCoroutine(layer.AttachParts(parts));
        }

        public void DetachParts(UIBase uiBase, List<UIPart> parts)
        {
            UIBaseLayer layer = m_LayerList.Find(uiBase);

            layer?.DetachParts(parts);
        }

        public void SetScreenTouchable(UIBase uiBase, bool enable)
        {
            UIBaseLayer layer = m_LayerList.Find(uiBase);
            if (layer == null)
            {
                return;
            }

            SetScreenTouchableByLayer(layer, enable);
        }

        public void SetScreenTouchableByLayer(UIBaseLayer layer, bool enable)
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

                foreach (BaseRaycaster rayCaster in m_RayCasterComponents)
                {
                    rayCaster.enabled = true;
                }
            }
            else
            {
                if (m_TouchOffCount == 0)
                {
                    foreach (BaseRaycaster rayCaster in m_RayCasterComponents)
                    {
                        rayCaster.enabled = false;
                    }
                }

                m_TouchOffCount++;
                layer.ScreenTouchOffCount++;
            }
        }

        public bool HasUIBase(string baseName)
        {
            return m_LayerList.Has(baseName);
        }

        public string GetFrontUINameInGroup(UIGroup group)
        {
            UIBaseLayer layer = m_LayerList.FindFrontLayerInGroup(group);
            return layer == null ? "" : layer.Base.Name;
        }

        public int GetUINumInGroup(UIGroup group)
        {
            return m_LayerList.GetNumInGroup(group);
        }

        private void Update()
        {
            m_LayerList.ForEachOnlyActive(layer =>
            {
                if (layer.Base.IsScheduleUpdate)
                {
                    layer.Base.OnUpdate();
                }
            });

            RunTouchEvents();
            RunDispatchedEvents();

            bool isInsert = AddLayer();
            bool isEject = RemoveLayer();

            if (!isEject && !isInsert)
            {
                return;
            }

            RefreshLayer();

            if (isEject && m_FadeController.IsHidden())
            {
                Unload();
            }

            if (m_AddingLayerList.Count != 0 || m_RemovingLayerList.Count != 0)
            {
                return;
            }

            PlayBgm();
            m_FadeController.FadeOut(Remove);
        }

        private void LateUpdate()
        {
            m_LayerList.ForEachOnlyActive(layer =>
            {
                if (layer.Base.IsScheduleUpdate)
                {
                    layer.Base.OnLateUpdate();
                }
            });
        }

        private void OnDestroy()
        {
            s_Instance = null;
        }

        private bool AddLayer()
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

        private bool RemoveLayer()
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

                m_LayerList.Remove(layer);
                layer.Destroy();
                isEject = true;
            }

            return isEject;
        }

        private void RefreshLayer()
        {
            bool isVisible = true;
            bool isTouchable = true;
            UIBaseLayer frontLayer = null;
            int index = m_UiLayers.childCount - 1;

            m_LayerList.ForEachAnything(layer =>
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

        private void RunTouchEvents()
        {
            if (m_TouchEvents.Count == 0)
            {
                return;
            }

            int untouchableIndex = FindUntouchableIndex();
            m_TouchEvents.Clear();

            while (m_TouchEvents.Count > 0)
            {
                bool ret = false;
                TouchEvent touch = m_TouchEvents.Dequeue();

                if (touch.Listener.Layer == null)
                {
                    continue;
                }

                bool touchable = IsScreenTouchable()
                                 && touch.Listener.Layer.IsTouchable()
                                 && untouchableIndex < touch.Listener.Layer.SiblingIndex;

                if (!touchable)
                {
                    continue;
                }

                UIPart part = touch.Listener.Part;
                GameObject listenerObject = touch.Listener.gameObject;
                switch (touch.Type)
                {
                    case TouchType.Click:
                    {
                        UISound se = new UISound();
                        ret = part.OnClick(touch.Listener.gameObject.name, listenerObject, touch.Pointer, se);
                        if (ret && m_Implements.Sounder != null)
                        {
                            if (!string.IsNullOrEmpty(se.m_PlayName))
                            {
                                m_Implements.Sounder.PlayClickSE(se.m_PlayName);
                            }
                            else
                            {
                                m_Implements.Sounder.PlayDefaultClickSE();
                            }
                        }

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

        private void RunDispatchedEvents()
        {
            if (m_DispatchedEvents.Count == 0)
            {
                return;
            }

            while (m_DispatchedEvents.Count > 0)
            {
                DispatchedEvent e = m_DispatchedEvents.Dequeue();
                m_LayerList.ForEachOnlyActive(layer => { layer.Base.OnDispatchedEvent(e.EventName, e.Param); });
            }

            m_DispatchedEvents.Clear();
        }

        private bool IsScreenTouchable()
        {
            return m_RayCasterComponents.Count != 0 && m_RayCasterComponents[0].enabled;
        }

        private int FindUntouchableIndex()
        {
            int index = -1;
            m_LayerList.ForEachOnlyActive(layer =>
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

        private static void Unload()
        {
            GC.Collect();
            Resources.UnloadUnusedAssets();
        }

        private void PlayBgm()
        {
            if (m_Implements.Sounder == null)
            {
                return;
            }

            string bgm = "";
            m_LayerList.ForEachAnything(l =>
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

    [CustomEditor(typeof(UIController))]
    public class UIControllerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            UIController uiController = target as UIController;
            if (uiController != null)
            {
                if (GUILayout.Button("FindRayCaster"))
                {
                    uiController.FindUIControllerItems();
                }
            }

            base.OnInspectorGUI();
        }
    }
}