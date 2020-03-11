using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

namespace DM
{
    public partial class UIController : MonoBehaviour
    {
        public const string LAYER_TOUCH_AREA_NAME = "LayerTouchArea";
        private Transform m_UiLayers;
        public Transform m_View3D;
        private List<BaseRaycaster> m_RayCasterComponents;
        private List<UIBaseLayer> m_AddingLayerList;
        private List<UIBaseLayer> m_RemovingLayerList;
        private UIBaseLayerController m_LayerController;
        private UIFadeController m_FadeController;
        private UITouchController m_TouchController;
        private UIDispatchController m_DispatchController;

        private UIImplements m_Implements;
        public static UIImplements Implements => Instance.m_Implements;
        
        public UIController(UIImplements implements)
        {
            m_Implements = implements;
        }

        public static void SetImplement(IPrefabLoader prefabLoader, ISounder sounder, IFadeCreator fadeCreator)
        {
            Instance.m_Implements = new UIImplements(prefabLoader, sounder, fadeCreator);
        }

        private static UIController s_Instance;

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
                s_Instance.m_LayerController = new UIBaseLayerController();
                s_Instance.m_FadeController = new UIFadeController();
                s_Instance.m_TouchController = new UITouchController();
                s_Instance.m_DispatchController = new UIDispatchController();
                
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

        private static void Unload()
        {
            GC.Collect();
            Resources.UnloadUnusedAssets();
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

        private void OnDestroy()
        {
            s_Instance = null;
        }
        
        public void AddUIBase(UIBase uiBase)
        {
            if (uiBase == null)
            {
                return;
            }

            UIBaseLayer layer = new UIBaseLayer(uiBase, m_UiLayers);

            if (layer.Base.IsLoadingWithoutFade())
            {
                StartCoroutine(layer.Load());
            }

            if (m_FadeController.ShouldFadeByAdding(uiBase, m_LayerController))
            {
                m_FadeController.FadeIn(Implements, m_AddingLayerList, AddUIBase);
            }

            m_AddingLayerList.Add(layer);
            m_LayerController.AddOrInsert(layer);
        }

        public void RemoveUIBase(UIBase uiBase)
        {
            if (uiBase == null)
            {
                return;
            }

            UIBaseLayer layer = m_LayerController.Find(uiBase);
            if (layer != null && layer.Inactive())
            {
                m_RemovingLayerList.Add(layer);
            }

            if (m_FadeController.ShouldFadeByRemoving(uiBase, m_LayerController, m_RemovingLayerList))
            {
                m_FadeController.FadeIn(Implements, m_AddingLayerList, AddUIBase);
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
                IEnumerable<UIBaseLayer> layers = m_LayerController.FindLayers(uiGroup);
                foreach (var layer in layers)
                {
                    RemoveUIBase(layer.Base);
                }
            }

            foreach (var uiBase in uiBases)
            {
                AddUIBase(uiBase);
            }
        }

        public void ListenTouch(UITouchListener listener, TouchType type, PointerEventData pointer)
        {
            m_TouchController.Enqueue(listener, type, pointer);
        }

        public void Back()
        {
            UIBaseLayer layer = null;
            foreach (var group in UIBackAble.s_Groups)
            {
                layer = m_LayerController.FindFrontLayerInGroup(group);
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
                RemoveUIBase(layer.Base);
            }
        }

        public IEnumerator YieldAttachParts(UIBase uiBase, List<UIPart> parts)
        {
            UIBaseLayer layer = m_LayerController.Find(uiBase);
            if (layer == null)
            {
                yield break;
            }

            yield return layer.AttachParts(parts);
        }

        public void AttachParts(UIBase uiBase, List<UIPart> parts)
        {
            UIBaseLayer layer = m_LayerController.Find(uiBase);
            if (layer == null)
            {
                return;
            }

            StartCoroutine(layer.AttachParts(parts));
        }

        public void DetachParts(UIBase uiBase, List<UIPart> parts)
        {
            UIBaseLayer layer = m_LayerController.Find(uiBase);

            layer?.DetachParts(parts);
        }

        public bool HasUIBase(string baseName)
        {
            return m_LayerController.Has(baseName);
        }

        public string GetFrontUINameInGroup(UIGroup group)
        {
            UIBaseLayer layer = m_LayerController.FindFrontLayerInGroup(group);
            return layer == null ? "" : layer.Base.Name;
        }

        public int GetUINumInGroup(UIGroup group)
        {
            return m_LayerController.GetNumInGroup(group);
        }

        public void SetScreenTouchable(UIBase uiBase, bool enable)
        {
            UIBaseLayer layer = m_LayerController.Find(uiBase);
            m_TouchController.SetScreenTouchableByLayer(layer, enable, m_RayCasterComponents);
        }
        
        public void SetScreenTouchableByLayer(UIBaseLayer layer, bool enable)
        {
            m_TouchController.SetScreenTouchableByLayer(layer, enable, m_RayCasterComponents);
        }
 
        public void Dispatch(string eventName, object param)
        {
            m_DispatchController.Dispatch(eventName, param);
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