using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DM
{
    public class UIBaseLayer : UIPartContainer
    {
        private const string TOUCH_OFF_LAYER_NAME = "LayerTouchOff";
        private const string LAYER_TOUCH_AREA_NAME = "LayerTouchArea";
        private const string SYSTEM_TOUCH_OFF_LAYER_NAME = "SystemTouchOff";
        private const string NONE_PREFAB_OBJECT_NAME = "root";
        private const string IN_ANIMATION_NAME = "In";
        private const string OUT_ANIMATION_NAME = "Out";

        private GameObject m_Origin;
        private GameObject m_TouchOff;
        private Transform m_Parent;
        private readonly List<UIPartContainer> m_PartContainers = new List<UIPartContainer>();
        private string m_LinkedBackName = "";
        private string m_LinkedFrontName = "";

        public UIBase Base => (UIBase)Part;
        public EnumLayerState State { get; private set; } 
        public UIBaseLayer FrontLayer { get; set; }
        public UIBaseLayer BackLayer { get; set; }
        public int ScreenTouchOffCount { get; set; }

        public UIBaseLayer(UIPart uiPart, Transform parent) : base(uiPart)
        {
            State　= EnumLayerState.None;
            m_Parent = parent;
            ProgressState(EnumLayerState.InFading);
        }

        public int SiblingIndex
        {
            set => m_Origin.transform.SetSiblingIndex(value);
            get => m_Origin.transform.GetSiblingIndex();
        }

        public bool IsNotYetLoaded()
        {
            return (State <= EnumLayerState.Loading || State == EnumLayerState.UselessLoading);
        }

        public override void Destroy()
        {
            for (int i = 0; i < ScreenTouchOffCount; i++)
            {
                UIController.Instance.SetScreenTouchableByLayer(this, true);
            }

            if (m_Origin != null)
            {
                m_Origin.transform.SetParent(null);
                Object.Destroy(m_Origin);
                m_Origin = null;
            }

            foreach (UIPartContainer partContainer in m_PartContainers)
            {
                partContainer.Destroy();
            }

            m_Parent = null;

            base.Destroy();
        }

        public IEnumerator Load()
        {
            if (!ProgressState(EnumLayerState.Loading))
            {
                ProgressState(EnumLayerState.Removing);
                yield break;
            }

            yield return LoadPrefab();

            m_Origin = new GameObject(Base.Name);
            SetupStretchAll(m_Origin.AddComponent<RectTransform>());
            m_Origin.transform.SetParent(m_Parent, false);

            GameObject rootObject = m_Prefab != null
                ? CreatePrefabObject()
                : CreateNonePrefabObject();

            if (rootObject != null)
            {
                Base.RootTransform = rootObject.transform;
            }

            Transform parent = Base.IsView3D() ? UIController.Instance.m_UIView3D : m_Origin.transform;
            Base.RootTransform.SetParent(parent, false);
            Base.RootTransform.gameObject.SetActive(false);

            yield return Base.OnLoadedBase();
            Setup();

            if (State != EnumLayerState.Loading)
            {
                ProgressState(EnumLayerState.Removing);
                yield break;
            }

            Base.RootTransform.gameObject.SetActive(true);
            ProgressState(EnumLayerState.Adding);
        }

        private static GameObject CreateNonePrefabObject()
        {
            var rootObject = new GameObject(NONE_PREFAB_OBJECT_NAME);
            SetupStretchAll(rootObject.AddComponent<RectTransform>());
            return rootObject;
        }

        private GameObject CreatePrefabObject()
        {
            var rootObject = Object.Instantiate(m_Prefab) as GameObject;
            if (rootObject != null)
            {
                rootObject.name = m_Prefab.name;
            }

            return rootObject;
        }

        private IEnumerator LoadPrefab()
        {
            if (string.IsNullOrEmpty(Base.PrefabPath))
            {
                yield break;
            }

            PrefabReceiver receiver = new PrefabReceiver();
            yield return UIController.Implements.PrefabLoader.Load(Base.PrefabPath, receiver);
            m_Prefab = receiver.m_Prefab;
        }

        public IEnumerator AttachParts(IEnumerable<UIPart> parts)
        {
            if (State > EnumLayerState.Active)
            {
                yield break;
            }
            
            foreach (var part in parts)
            {
                var container = new UIPartContainer(part);
                m_PartContainers.Add(container);
                yield return container.LoadAndSetup(this);
            }
        }

        public void DetachParts(IEnumerable<UIPart> parts)
        {
            if (State != EnumLayerState.Active)
            {
                return;
            }

            foreach (UIPart part in parts)
            {
                m_PartContainers.RemoveAll(container => container.Part == part);
                part.Destroy();
            }
        }

        public bool SetActivate()
        {
            if (State != EnumLayerState.Adding)
            {
                Remove();
                return false;
            }

            ProgressState(EnumLayerState.InAnimation);
            bool isPlay = Base.PlayAnimations(IN_ANIMATION_NAME, false, () => { ProgressState(EnumLayerState.Active); });

            if (!isPlay)
            {
                ProgressState(EnumLayerState.Active);
            }

            return true;
        }

        public bool SetInactive()
        {
            if (State < EnumLayerState.Active)
            {
                Remove();
                return true;
            }

            if (State > EnumLayerState.Active)
            {
                return false;
            }

            bool ret = ProgressState(EnumLayerState.OutAnimation);
            if (!ret)
            {
                return false;
            }

            bool isPlay = IsVisible();
            if (isPlay)
            {
                isPlay = Base.PlayAnimations(OUT_ANIMATION_NAME, true, () =>
                    {
                        ProgressState(EnumLayerState.OutFading);
                    });
            }

            if (!isPlay)
            {
                ProgressState(EnumLayerState.OutFading);
            }

            return true;
        }

        public void Remove()
        {
            if (State == EnumLayerState.Removing || State == EnumLayerState.UselessLoading)
            {
                return;
            }

            var nextBaseLayerState = (State == EnumLayerState.Loading)
                ? EnumLayerState.UselessLoading
                : EnumLayerState.Removing;

            ProgressState(nextBaseLayerState);
        }

        public void CallSwitchFront()
        {
            string pre = m_LinkedFrontName;
            m_LinkedFrontName = (FrontLayer != null) ? FrontLayer.Base.Name : string.Empty;
            if (pre != m_LinkedFrontName)
            {
                Base.OnSwitchFrontUI(m_LinkedFrontName);
            }
        }

        public void CallSwitchBack()
        {
            string pre = m_LinkedBackName;
            m_LinkedBackName = (BackLayer != null) ? BackLayer.Base.Name : string.Empty;
            if (pre != m_LinkedBackName)
            {
                Base.OnSwitchBackUI(m_LinkedBackName);
            }
        }

        public bool IsVisible()
        {
            if (m_Origin == null)
            {
                return false;
            }

            return Base.VisibleControllers.Count <= 0
                ? m_Origin.activeSelf
                : Base.VisibleControllers[0].IsVisible();
        }

        public bool IsTouchable()
        {
            if (m_TouchOff == null)
            {
                return false;
            }

            return !m_TouchOff.activeSelf;
        }

        public void SetVisible(bool enable)
        {
            if (enable && !StateFlags.IsVisible(State))
            {
                return;
            }

            if (m_Origin == null)
            {
                return;
            }

            if (Base.VisibleControllers.Count <= 0)
            {
                Base.RootTransform.gameObject.SetActive(enable);
            }
            else
            {
                foreach (var item in Base.VisibleControllers)
                {
                    item.SetVisible(Base.RootTransform.gameObject, enable);
                }
            }
        }

        public void SetTouchable(bool enable)
        {
            if (m_TouchOff == null)
            {
                return;
            }

            m_TouchOff.SetActive(!enable);
        }

        private bool IsAllFrontLayerBackVisible()
        {
            UIBaseLayer layer = FrontLayer;
            while (layer != null)
            {
                if (!layer.Base.IsBackVisible())
                {
                    return false;
                }

                layer = layer.FrontLayer;
            }

            return true;
        }

        private bool IsAllFrontLayerBackTouchable()
        {
            UIBaseLayer layer = FrontLayer;
            while (layer != null)
            {
                if (!layer.Base.IsBackTouchable())
                {
                    return false;
                }

                layer = layer.FrontLayer;
            }

            return true;
        }

        private bool ProgressState(EnumLayerState nextEnumLayerState)
        {
            if (State >= nextEnumLayerState)
            {
                return false;
            }

            State = nextEnumLayerState;
            
            bool isTouchable = StateFlags.IsTouchable(State);
            if ((ScreenTouchOffCount == 0) != isTouchable)
            {
                UIController.Instance.SetScreenTouchableByLayer(this, isTouchable);
            }

            bool isVisible = StateFlags.IsVisible(State);
            if (isVisible != IsVisible())
            {
                if (!isVisible || IsAllFrontLayerBackVisible())
                {
                    SetVisible(isVisible);
                }
            }

            if (nextEnumLayerState == EnumLayerState.Active)
            {
                Base.OnActive();
            }

            return true;
        }

        private void Setup()
        {
            m_TouchOff = CreateTouchPanel(TOUCH_OFF_LAYER_NAME);
            m_TouchOff.SetActive(false);

            GameObject touchArea = CreateTouchArea();

            GameObject systemTouchOffArea = CreateSystemTouchOffArea();

            List<GameObject> innerIndex = new List<GameObject>()
            {
                systemTouchOffArea,
                touchArea,
                Base.RootTransform.gameObject,
                m_TouchOff,
            };

            int index = 0;
            foreach (GameObject item in innerIndex)
            {
                if (item == null)
                {
                    continue;    
                }

                item.transform.SetSiblingIndex(index++);
            }

            CollectComponents(Base.RootTransform.gameObject, this);
        }

        private GameObject CreateSystemTouchOffArea()
        {
            if (!Base.IsSystemUntouchable())
            {
                return null;
            }

            GameObject systemTouchOff = CreateTouchPanel(SYSTEM_TOUCH_OFF_LAYER_NAME);

            return systemTouchOff;
        }

        private GameObject CreateTouchArea()
        {
            if (!Base.IsTouchEventCallable())
            {
                return null;
            }

            GameObject touchArea = CreateTouchPanel(LAYER_TOUCH_AREA_NAME);
            UILayerTouchListener listener = touchArea.AddComponent<UILayerTouchListener>();
            listener.SetLayerAndPart(this, Base);

            return touchArea;
        }

        private GameObject CreateTouchPanel(string name)
        {
            GameObject touchPanel = new GameObject(name);
            touchPanel.transform.SetParent(m_Origin.transform, false);
            Image image = touchPanel.AddComponent<Image>();
            image.color = new Color(0f, 0f, 0f, 0f);
            SetupStretchAll(touchPanel.GetComponent<RectTransform>());
            
            return touchPanel;
        }

        private static void SetupStretchAll(RectTransform rectTransform)
        {
            rectTransform.anchorMin = Vector2.zero;
            rectTransform.anchorMax = Vector2.one;
            rectTransform.pivot = Vector2.zero;
            rectTransform.sizeDelta = Vector2.zero;
        }
    }
}