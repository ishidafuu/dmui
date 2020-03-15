using System;
using UniRx.Async;
using UnityEngine;
using Object = UnityEngine.Object;

namespace DM
{
    public class UIPart
    {
        private Action m_StopCallback;
        private bool m_IsExit;
        private int m_PlayCount;

        public Animator[] Animators { get; set; }
        public Transform RootTransform { get; set; }
        public string PrefabPath { get; }

        // 生成済みオブジェクトを渡して生成
        protected UIPart(Transform rootTransform)
        {
            RootTransform = rootTransform;
        }

        // 生成前オブジェクト（プレファブPath）を渡して生成
        protected UIPart(string path)
        {
            PrefabPath = path;
        }

        public virtual void Destroy()
        {
            OnDestroy();

            if (RootTransform != null)
            {
                RootTransform.SetParent(null);
                Object.Destroy(RootTransform.gameObject);
                RootTransform = null;
            }

            Animators = null;
            m_StopCallback = null;
        }

        public bool PlayAnimations(string stateName, bool isExit, Action callback)
        {
            if (m_PlayCount > 0)
            {
                return false;
            }

            m_IsExit = isExit;

            int count = Play(stateName);
            if (count <= 0)
            {
                return false;
            }

            if (callback == null)
            {
                return true;
            }

            m_PlayCount = count;
            m_StopCallback = callback;

            return true;
        }

        private int Play(string stateName)
        {
            string playName = UIStateBehaviour.LAYER_NAME + stateName;

            int count = 0;
            foreach (var animator in Animators)
            {
                UIStateBehaviour[] states = animator.GetBehaviours<UIStateBehaviour>();
                foreach (var state in states)
                {
                    state.SetExitCallback(OnExit);
                    state.SetPlayName(playName);
                }

                if (states.Length <= 0)
                {
                    continue;
                }

                animator.Play(playName);
                count++;
            }

            return count;
        }

        private void OnExit(Animator animator)
        {
            if (m_IsExit)
            {
                animator.enabled = false;
            }

            m_PlayCount--;
            if (m_PlayCount > 0)
            {
                return;
            }

            m_StopCallback?.Invoke();
        }

        // -----------------------------------------------------------------------------------------------------------------------------------------
        // Viewから呼ばれるPublicメソッド
        // -----------------------------------------------------------------------------------------------------------------------------------------

        // -----------------------------------------------------------------------------------------------------------------------------------------
        // イベントメソッド
        // -----------------------------------------------------------------------------------------------------------------------------------------

        // ロード完了
        public virtual async UniTask OnLoadedPart(UIBase targetLayer) { }

        // クリック
        public virtual bool OnClick(TouchEvent touch, UISound uiSound)
        {
            return false;
        }

        // タッチ開始
        public virtual bool OnTouchDown(TouchEvent touch)
        {
            return false;
        }

        // タッチ終了
        public virtual bool OnTouchUp(TouchEvent touch)
        {
            return false;
        }

        // ドラッグ
        public virtual bool OnDrag(TouchEvent touch)
        {
            return false;
        }

        // 破棄
        protected virtual void OnDestroy() { }
    }
}