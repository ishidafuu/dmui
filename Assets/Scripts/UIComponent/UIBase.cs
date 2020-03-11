using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DM
{
    public class UIBase : UIPart
    {
        public List<UIVisibleController> VisibleControllers { get; } = new List<UIVisibleController>();
        public bool IsScheduleUpdate { get; protected set; }
        public UIGroup Group { get; }
        private readonly UIPreset m_Preset;
        public string Bgm { get; }

        protected UIBase(string prefabPath, UIGroup group, UIPreset preset = UIPreset.None, string bgm = "")
            : base(prefabPath)
        {
            Group = group;
            m_Preset = preset;
            Bgm = bgm;

            if (IsView3D())
            {
                AddRendererController();
            }
            else
            {
                AddVisibleBehaviourController<Graphic>();
            }
        }

        public string Name => GetType().Name;

        public bool IsBackVisible()
        {
            return (m_Preset & UIPreset.BackVisible) > 0;
        }

        public bool IsBackTouchable()
        {
            return (m_Preset & UIPreset.BackTouchable) > 0;
        }

        public bool IsTouchEventCallable()
        {
            return (m_Preset & UIPreset.TouchEventCallable) > 0;
        }

        public bool IsSystemUntouchable()
        {
            return (m_Preset & UIPreset.SystemUntouchable) > 0;
        }

        public bool IsLoadingWithoutFade()
        {
            return (m_Preset & UIPreset.LoadingWithoutFade) > 0;
        }

        public bool IsActiveWithoutFade()
        {
            return (m_Preset & UIPreset.ActiveWithoutFade) > 0;
        }

        public bool IsView3D()
        {
            return (m_Preset & UIPreset.View3D) > 0;
        }

        public override void Destroy()
        {
            base.Destroy();

            foreach (UIVisibleController item in VisibleControllers)
            {
                item.Destroy();
            }
        }

        private void AddRendererController()
        {
            VisibleControllers.Add(new UIRendererController());
        }

        protected void AddVisibleBehaviourController<T>() where T : Behaviour
        {
            VisibleControllers.Add(new UIBehaviourController<T>());
        }

        // -----------------------------------------------------------------------------------------------------------------------------------------
        // virtual methods

        // プレファブ読み込み完了時
        public virtual IEnumerator OnLoadedBase()
        {
            yield break;
        }

        // 毎フレーム呼び出し
        public virtual void OnUpdate() { }

        // 毎フレーム呼び出し
        public virtual void OnLateUpdate() { }

        // 再表示時
        public virtual void OnReVisible() { }

        // 再タッチ可能時
        public virtual void OnReTouchable() { }

        // 再アクティブ(BaseLayerState.Active)時
        public virtual void OnActive() { }

        //　イベント受信時
        public virtual void OnDispatchedEvent(string eventName, object param) { }

        // 戻るボタン処理
        public virtual bool OnBack()
        {
            return true;
        }

        // 前面のレイヤ切り替わり
        public virtual void OnSwitchFrontUI(string uiName) { }

        // 背面のレイヤ切り替わり
        public virtual void OnSwitchBackUI(string uiName) { }
    }
}