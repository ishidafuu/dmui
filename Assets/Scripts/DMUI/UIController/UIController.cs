using System;
using System.Collections.Generic;
using UniRx.Async;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

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

        private List<BaseRaycaster> m_RayCasterComponents;
        private List<UIBaseLayer> m_AddingLayerList;
        private List<UIBaseLayer> m_RemovingLayerList;
        private UIBaseLayerController m_LayerController;
        private UIDispatchController m_DispatchController;
        private UIFadeController m_FadeController;
        private UIImplements m_Implements;
        private UITouchController m_TouchController;
        private UILoadingController m_LoadingController;
        private UIToastController m_ToastController;

        private Transform m_UILayers;
        public Transform m_UIView3D;
        public CanvasScaler m_CanvasScaler;
        public Camera m_Camera;
        public static UIImplements Implements => Instance.m_Implements;

        public UIController(UIImplements implements)
        {
            m_Implements = implements;
        }

        public static void SetImplement(IPrefabLoader prefabLoader, ISounder sounder, IFadeCreator fadeCreator,
            ILoadingCreator loadingCreator, IToastCreator toastCreator)
        {
            Instance.m_Implements = new UIImplements(prefabLoader, sounder, fadeCreator, loadingCreator, toastCreator);
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
            m_LoadingController = new UILoadingController();
            m_ToastController = new UIToastController();

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
            
            m_CanvasScaler = FindObjectOfType<CanvasScaler>();

            BaseRaycaster[] rayCasters = FindObjectsOfType<BaseRaycaster>();
            m_RayCasterComponents = new List<BaseRaycaster>();
            foreach (BaseRaycaster item in rayCasters)
            {
                m_RayCasterComponents.Add(item);
            }
            
            m_Camera = GameObject.Find("Camera").GetComponent<Camera>();

            Debug.Log("Complete FindUIControllerItems");
        }

        private void OnDestroy()
        {
            s_Instance = null;
        }

        public void ListenTouch(UITouchListener listener, EnumTouchType type, PointerEventData pointer)
        {
            m_TouchController.Enqueue(listener, type, pointer);
        }

        public void SetScreenTouchableByLayer(UIBaseLayer layer, bool enable)
        {
            m_TouchController.SetScreenTouchable(layer, enable, m_RayCasterComponents);
        }

        // -----------------------------------------------------------------------------------------------------------------------------------------
        // Viewから呼ばれるPublicメソッド
        // -----------------------------------------------------------------------------------------------------------------------------------------

        // レイヤ追加
        public void AddFront(UIBase layerUIBase)
        {
            if (layerUIBase == null)
            {
                return;
            }

            UIBaseLayer layer = new UIBaseLayer(layerUIBase, m_UILayers);

            if (layer.Base.IsLoadingWithoutFade())
            {
                layer?.Load();
            }

            if (m_FadeController.IsShouldFadeByAdding(layerUIBase, m_LayerController))
            {
                m_FadeController.FadeIn(Implements, m_AddingLayerList, AddFront);
            }

            m_AddingLayerList.Add(layer);
            m_LayerController.AddOrInsert(layer);
        }

        // レイヤ削除
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

        // レイヤ再配置
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

        // UIパーツ追加（完了待ち（OnLoadedBase内で呼ばれる））
        public async UniTask YieldAttachParts(UIBase targetUIBase, IEnumerable<UIPart> parts)
        {
            UIBaseLayer layer = m_LayerController.Find(targetUIBase);
            if (layer == null)
            {
                return;
            }

            await layer.AttachParts(parts);
        }

        // 即時UIパーツ追加（完了待たず）
        public void AttachParts(UIBase uiBase, IEnumerable<UIPart> parts)
        {
            UIBaseLayer layer = m_LayerController.Find(uiBase);

            layer?.AttachParts(parts);
        }

        // UIパーツ削除
        public void DetachParts(UIBase targetUIBase, IEnumerable<UIPart> parts)
        {
            UIBaseLayer layer = m_LayerController.Find(targetUIBase);

            layer?.DetachParts(parts);
        }

        // イベント発行
        public void Dispatch(string eventName, object param)
        {
            m_DispatchController.Dispatch(eventName, param);
        }

        // Backボタン
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

        // タッチON／OFF切り替え（UIBase指定）
        public void SetScreenTouchable(UIBase uiBase, bool enable)
        {
            UIBaseLayer layer = m_LayerController.Find(uiBase);
            SetScreenTouchableByLayer(layer, enable);
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
        
        // レイヤ取得
        public UIBaseLayer GetBaseLayer(Type type)
        {
            return m_LayerController.Find(type);
        }

        // ローディング開始
        public void LoadingIn()
        {
            m_LoadingController.LoadingIn(Implements, m_AddingLayerList, AddFront);
        }

        // ローディング終了
        public void LoadingOut()
        {
            m_LoadingController.LoadingOut(Remove);
        }

        // ローディング中
        public bool IsLoading()
        {
            return m_LoadingController.IsLoading();
        }

        // トースト開始
        public void ToastIn(string message)
        {
            m_ToastController.ToastIn(Implements, m_AddingLayerList, AddFront, message);
        }

        // トースト終了
        public void ToastOut()
        {
            m_ToastController.ToastOut(Remove);
        }
    }
}