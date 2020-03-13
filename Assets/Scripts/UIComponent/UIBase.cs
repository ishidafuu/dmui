using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DM
{
    public class UIBase : UIPart
    {
        private readonly EnumUIPreset m_Preset;
        public List<UIVisibleController> VisibleControllers { get; }
        public bool IsScheduleUpdate { get; protected set; }
        public EnumUIGroup Group { get; }
        public string Bgm { get; }

        protected UIBase(string prefabPath, EnumUIGroup group, EnumUIPreset preset = EnumUIPreset.None, string bgm = "")
            : base(prefabPath)
        {
            VisibleControllers = new List<UIVisibleController>();
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
            return (m_Preset & EnumUIPreset.BackVisible) > 0;
        }

        public bool IsBackTouchable()
        {
            return (m_Preset & EnumUIPreset.BackTouchable) > 0;
        }

        public bool IsTouchEventCallable()
        {
            return (m_Preset & EnumUIPreset.TouchEventCallable) > 0;
        }

        public bool IsSystemUntouchable()
        {
            return (m_Preset & EnumUIPreset.SystemUntouchable) > 0;
        }

        public bool IsLoadingWithoutFade()
        {
            return (m_Preset & EnumUIPreset.LoadingWithoutFade) > 0;
        }

        public bool IsActiveWithoutFade()
        {
            return (m_Preset & EnumUIPreset.ActiveWithoutFade) > 0;
        }

        public bool IsView3D()
        {
            return (m_Preset & EnumUIPreset.View3D) > 0;
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
        
        // -----------------------------------------------------------------------------------------------------------------------------------------
        // Viewから呼ばれるPublicメソッド
        // -----------------------------------------------------------------------------------------------------------------------------------------

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
        public virtual void OnDispatchedEvent(DispatchedEvent dispatchedEvent) { }

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