using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace DM
{
    public class UIBaseLayer : UIPartContainer
    {
        private const string NONE_PREFAB_OBJECT_NAME = "root";
        public BaseLayerState State { get; private set; } = BaseLayerState.None;
        public int ScreenTouchOffCount { get; set; }
        private GameObject m_Origin;
        private GameObject m_TouchOff;
        private readonly List<UIPartContainer> m_PartContainers = new List<UIPartContainer>();
        public UIBaseLayer BackLayer { get; set; }
        public UIBaseLayer FrontLayer { get; set; }

        private string m_LinkedFrontName = "";
        private string m_LinkedBackName = "";

        private Transform m_Parent;

        public UIBase Base => (UIBase)Part;

        public UIBaseLayer(UIPart uiPart, Transform parent) : base(uiPart)
        {
            m_Parent = parent;
            ProgressState(BaseLayerState.InFading);
        }

        public int SiblingIndex
        {
            set => m_Origin.transform.SetSiblingIndex(value);
            get => m_Origin.transform.GetSiblingIndex();
        }

        public bool IsNotYetLoaded()
        {
            return (State <= BaseLayerState.Loading || State == BaseLayerState.UselessLoading);
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
            if (!ProgressState(BaseLayerState.Loading))
            {
                ProgressState(BaseLayerState.Removing);
                yield break;
            }

            if (!string.IsNullOrEmpty(Base.PrefabPath))
            {
                PrefabReceiver receiver = new PrefabReceiver();
                yield return UIController.Implements.PrefabLoader.Load(Base.PrefabPath, receiver);
                m_Prefab = receiver.m_Prefab;
            }

            m_Origin = new GameObject(Base.Name);
            SetupStretchAll(m_Origin.AddComponent<RectTransform>());
            m_Origin.transform.SetParent(m_Parent, false);

            GameObject rootObject = null;
            if (m_Prefab != null)
            {
                rootObject = GameObject.Instantiate(m_Prefab) as GameObject;
                rootObject.name = m_Prefab.name;
            }
            else
            {
                rootObject = new GameObject(NONE_PREFAB_OBJECT_NAME);
                SetupStretchAll(rootObject.AddComponent<RectTransform>());
            }

            Base.Root = rootObject.transform;

            Transform parent = Base.IsView3D() ? UIController.Instance.m_UIView3D : m_Origin.transform;
            Base.Root.SetParent(parent, false);
            Base.Root.gameObject.SetActive(false);

            yield return Base.OnLoadedBase();
            Setup();

            if (State != BaseLayerState.Loading)
            {
                ProgressState(BaseLayerState.Removing);
                yield break;
            }

            Base.Root.gameObject.SetActive(true);
            ProgressState(BaseLayerState.Adding);
        }

        public IEnumerator AttachParts(IEnumerable<UIPart> parts)
        {
            if (State > BaseLayerState.Active)
            {
                yield break;
            }

            foreach (var container in parts.Select(item => new UIPartContainer(item)))
            {
                m_PartContainers.Add(container);
                yield return container.LoadAndSetup(this);
            }
        }

        public void DetachParts(IEnumerable<UIPart> parts)
        {
            if (State != BaseLayerState.Active)
            {
                return;
            }

            foreach (var item in parts)
            {
                m_PartContainers.RemoveAll(container => container.Part == item);
                item.Destroy();
            }
        }

        public bool Activate()
        {
            if (State != BaseLayerState.Adding)
            {
                ExceptState();
                return false;
            }

            ProgressState(BaseLayerState.InAnimation);
            bool isPlay = Base.PlayAnimations("In", () => { ProgressState(BaseLayerState.Active); });
            if (!isPlay)
            {
                ProgressState(BaseLayerState.Active);
            }

            return true;
        }

        public bool Inactive()
        {
            if (State < BaseLayerState.Active)
            {
                ExceptState();
                return true;
            }

            if (State > BaseLayerState.Active)
            {
                return false;
            }

            bool ret = ProgressState(BaseLayerState.OutAnimation);
            if (!ret)
            {
                return false;
            }

            bool isPlay = IsVisible();
            if (isPlay)
            {
                isPlay = Base.PlayAnimations("Out", () => { ProgressState(BaseLayerState.OutFading); }, true);
            }

            if (!isPlay)
            {
                ProgressState(BaseLayerState.OutFading);
            }

            return true;
        }

        public void Remove()
        {
            if (State == BaseLayerState.Removing || State == BaseLayerState.UselessLoading)
            {
                return;
            }

            ProgressState(State == BaseLayerState.Loading ? BaseLayerState.UselessLoading : BaseLayerState.Removing);
        }

        public void CallSwitchFront()
        {
            string pre = m_LinkedFrontName;
            m_LinkedFrontName = (FrontLayer != null) ? FrontLayer.Base.Name : "";
            if (pre != m_LinkedFrontName)
            {
                Base.OnSwitchFrontUI(m_LinkedFrontName);
            }
        }

        public void CallSwitchBack()
        {
            string pre = m_LinkedBackName;
            m_LinkedBackName = (BackLayer != null) ? BackLayer.Base.Name : "";
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

            return Base.VisibleControllers.Count <= 0 ? m_Origin.activeSelf : Base.VisibleControllers[0].IsVisible();
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
            if (enable && !StateFlags.s_Map[State].m_IsVisible)
            {
                return;
            }

            if (m_Origin == null)
            {
                return;
            }

            if (Base.VisibleControllers.Count <= 0)
            {
                Base.Root.gameObject.SetActive(enable);
            }
            else
            {
                foreach (var item in Base.VisibleControllers)
                {
                    item.SetVisible(Base.Root.gameObject, enable);
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

        private bool CanVisible()
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

        private bool CanTouchable()
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

        private void ExceptState()
        {
            Remove();
        }

        private bool ProgressState(BaseLayerState nextBaseLayerState)
        {
            if (State >= nextBaseLayerState)
            {
                return false;
            }

            State = nextBaseLayerState;
            StateFlags flags = StateFlags.s_Map[nextBaseLayerState];

            if ((ScreenTouchOffCount == 0) != flags.m_IsTouchable)
            {
                UIController.Instance.SetScreenTouchableByLayer(this, flags.m_IsTouchable);
            }

            if (flags.m_IsVisible != IsVisible())
            {
                if (!flags.m_IsVisible || CanVisible())
                {
                    SetVisible(flags.m_IsVisible);
                }
            }

            if (nextBaseLayerState == BaseLayerState.Active)
            {
                Base.OnActive();
            }

            return true;
        }

        private void Setup()
        {
            m_TouchOff = CreateTouchPanel("LayerTouchOff");
            m_TouchOff.SetActive(false);
            m_TouchOff.transform.SetParent(m_Origin.transform, false);

            GameObject touchArea = null;
            if (Base.IsTouchEventCallable())
            {
                touchArea = CreateTouchPanel(UIController.LAYER_TOUCH_AREA_NAME);
                UILayerTouchListener listener = touchArea.AddComponent<UILayerTouchListener>();
                listener.SetUI(this, Base);
                touchArea.transform.SetParent(m_Origin.transform, false);
            }

            GameObject systemTouchOff = null;
            if (Base.IsSystemUntouchable())
            {
                systemTouchOff = CreateTouchPanel("SystemTouchOff");
                systemTouchOff.transform.SetParent(m_Origin.transform, false);
            }

            List<GameObject> innerIndex = new List<GameObject>()
            {
                systemTouchOff,
                touchArea,
                Base.Root.gameObject,
                m_TouchOff,
            };
            int index = 0;
            foreach (var item in innerIndex.Where(item => item != null))
            {
                item.transform.SetSiblingIndex(index++);
            }

            CollectComponents(Base.Root.gameObject, this);
        }

        private GameObject CreateTouchPanel(string name)
        {
            GameObject gameObject = new GameObject(name);
            Image image = gameObject.AddComponent<Image>();
            image.color = new Color(0f, 0f, 0f, 0f);
            SetupStretchAll(gameObject.GetComponent<RectTransform>());
            return gameObject;
        }

        private void SetupStretchAll(RectTransform rectTransform)
        {
            rectTransform.anchorMin = Vector2.zero;
            rectTransform.anchorMax = Vector2.one;
            rectTransform.pivot = Vector2.zero;
            rectTransform.sizeDelta = Vector2.zero;
        }
    }
}