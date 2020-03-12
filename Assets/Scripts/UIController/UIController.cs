using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace DM
{
    public partial class UIController : MonoBehaviour
    {
        // 戻るによる削除対象のグループ
        // この中の最前面のグループのOnBackが呼ばれる
        private static readonly List<EnumUIGroup> s_BackAbleGroups = new List<EnumUIGroup>()
        {
            EnumUIGroup.Dialog,
            EnumUIGroup.Scene,
            EnumUIGroup.MainScene,
            EnumUIGroup.View3D,
        };

        public const string LAYER_TOUCH_AREA_NAME = "LayerTouchArea";

        private List<BaseRaycaster> m_RayCasterComponents;
        private List<UIBaseLayer> m_AddingLayerList;
        private List<UIBaseLayer> m_RemovingLayerList;
        private UIBaseLayerController m_LayerController;
        private UIDispatchController m_DispatchController;
        private UIFadeController m_FadeController;
        private UIImplements m_Implements;
        private UITouchController m_TouchController;

        private Transform m_UILayers;
        public Transform m_UIView3D;

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

                s_BackAbleGroups.Sort((x, y) => y - x);
                s_Instance = FindObjectOfType<UIController>();
                s_Instance.Init();

                return s_Instance;
            }
        }

        private void Init()
        {
            m_AddingLayerList = new List<UIBaseLayer>();
            m_RemovingLayerList = new List<UIBaseLayer>();
            m_LayerController = new UIBaseLayerController();
            m_FadeController = new UIFadeController();
            m_TouchController = new UITouchController();
            m_DispatchController = new UIDispatchController();

            if (m_RayCasterComponents == null
                || m_UILayers == null
                || m_UIView3D == null)
            {
                FindUIControllerItems();
            }
        }

        private static void Unload()
        {
            GC.Collect();
            Resources.UnloadUnusedAssets();
        }

        public void FindUIControllerItems()
        {
            m_UILayers = FindObjectOfType<UILayers>()?.transform;
            if (m_UILayers == null)
            {
                var uiLayers = GameObject.Find("UILayers");
                m_UILayers = uiLayers.AddComponent<UILayers>().transform;
                Debug.Log("add UILayers");
            }
            else
            {
                Debug.Log("found UILayers");
            }
            
            m_UIView3D = FindObjectOfType<UIView3D>()?.transform;
            if (m_UIView3D == null)
            {
                var uiView3D = GameObject.Find("3D");
                m_UIView3D = uiView3D.AddComponent<UIView3D>().transform;
                Debug.Log("add UIView3D");
            }
            else
            {
                Debug.Log("found UIView3D");
            }
            
            BaseRaycaster[] rayCasters = FindObjectsOfType<BaseRaycaster>();
            m_RayCasterComponents = new List<BaseRaycaster>();
            foreach (BaseRaycaster item in rayCasters)
            {
                m_RayCasterComponents.Add(item);
            }
            
            Debug.Log("Complete FindUIControllerItems");
        }

        private void OnDestroy()
        {
            s_Instance = null;
        }

        public void AddFront(UIBase uiBase)
        {
            if (uiBase == null)
            {
                return;
            }

            UIBaseLayer layer = new UIBaseLayer(uiBase, m_UILayers);

            if (layer.Base.IsLoadingWithoutFade())
            {
                StartCoroutine(layer.Load());
            }

            if (m_FadeController.IsShouldFadeByAdding(uiBase, m_LayerController))
            {
                m_FadeController.FadeIn(Implements, m_AddingLayerList, AddFront);
            }

            m_AddingLayerList.Add(layer);
            m_LayerController.AddOrInsert(layer);
        }

        public void Remove(UIBase uiBase)
        {
            if (uiBase == null)
            {
                return;
            }

            UIBaseLayer layer = m_LayerController.Find(uiBase);

            bool isInactive = false;
            
            if (layer != null)
            {
                isInactive = layer.SetInactive();
            }

            if (isInactive)
            {
                m_RemovingLayerList.Add(layer);
            }

            if (m_FadeController.IsShouldFadeByRemoving(uiBase, m_LayerController, m_RemovingLayerList))
            {
                m_FadeController.FadeIn(Implements, m_AddingLayerList, AddFront);
            }
        }

        public void Replace(UIBase[] uiBases, EnumUIGroup[] removeGroups = null)
        {
            HashSet<EnumUIGroup> removes = (removeGroups == null)
                ? new HashSet<EnumUIGroup>()
                : new HashSet<EnumUIGroup>(removeGroups);

            foreach (UIBase uiBase in uiBases)
            {
                removes.Add(uiBase.Group);
            }

            foreach (EnumUIGroup uiGroup in removes)
            {
                IEnumerable<UIBaseLayer> layers = m_LayerController.FindLayers(uiGroup);
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

        public void ListenTouch(UITouchListener listener, EnumTouchType type, PointerEventData pointer)
        {
            m_TouchController.Enqueue(listener, type, pointer);
        }

        public void Back()
        {
            UIBaseLayer layer = null;
            foreach (var group in s_BackAbleGroups)
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
                Remove(layer.Base);
            }
        }

        public void Dispatch(string eventName, object param)
        {
            m_DispatchController.Dispatch(eventName, param);
        }

        public IEnumerator YieldAttachParts(UIBase targetUIBase, IEnumerable<UIPart> parts)
        {
            UIBaseLayer layer = m_LayerController.Find(targetUIBase);
            if (layer == null)
            {
                yield break;
            }

            // 処理を待つ（OnLoadedBase内で呼ばれる）
            yield return layer.AttachParts(parts);
        }

        public void AttachParts(UIBase uiBase, IEnumerable<UIPart> parts)
        {
            UIBaseLayer layer = m_LayerController.Find(uiBase);
            if (layer == null)
            {
                return;
            }

            // 処理を待たない
            StartCoroutine(layer.AttachParts(parts));
        }

        public void DetachParts(UIBase targetUIBase, IEnumerable<UIPart> parts)
        {
            UIBaseLayer layer = m_LayerController.Find(targetUIBase);

            layer?.DetachParts(parts);
        }

        // レイヤの存在チェック
        public bool HasUIBase(string baseName)
        {
            return m_LayerController.Has(baseName);
        }

        // 最前面レイヤ名取得
        public string GetFrontUINameInGroup(EnumUIGroup group)
        {
            UIBaseLayer layer = m_LayerController.FindFrontLayerInGroup(group);
            return layer == null ? string.Empty : layer.Base.Name;
        }

        // レイヤカウント取得
        public int GetLayerCountInGroup(EnumUIGroup group)
        {
            return m_LayerController.GetCountInGroup(group);
        }

        // タッチON／OFF切り替え（UIBase指定）
        public void SetScreenTouchable(UIBase uiBase, bool enable)
        {
            UIBaseLayer layer = m_LayerController.Find(uiBase);
            m_TouchController.SetScreenTouchable(layer, enable, m_RayCasterComponents);
        }

        // タッチON／OFF切り替え（レイヤ指定）
        public void SetScreenTouchableByLayer(UIBaseLayer layer, bool enable)
        {
            m_TouchController.SetScreenTouchable(layer, enable, m_RayCasterComponents);
        }
    }
}