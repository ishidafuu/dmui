﻿using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using Object = UnityEngine.Object;

namespace DM
{
    public class UIPart
    {
        public Animator[] Animators { get; set; }

        private int m_PlayCount = 0;
        private Action m_StopCallback = null;
        public Transform Root { get; set; }

        public string PrefabPath { get; }

        private bool m_Exit;

        // 生成済みオブジェクトを渡して生成
        protected UIPart(Transform root)
        {
            Root = root;
        }

        // 生成前オブジェクト（プレファブPath）を渡して生成
        protected UIPart(string path)
        {
            PrefabPath = path;
        }

        public virtual void Destroy()
        {
            OnDestroy();

            if (Root != null)
            {
                Root.SetParent(null);
                Object.Destroy(Root.gameObject);
                Root = null;
            }

            Animators = null;
            m_StopCallback = null;
        }

        public bool PlayAnimations(string name, Action callback = null, bool exit = false)
        {
            if (m_PlayCount > 0)
            {
                return false;
            }

            m_Exit = exit;

            int count = Play(name);
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

        private int Play(string name)
        {
            string playName = UIStateBehaviour.LAYER_NAME + name;

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
                ++count;
            }

            return count;
        }

        private void OnExit(Animator animator)
        {
            if (m_Exit)
            {
                animator.enabled = false;
            }

            if (--m_PlayCount > 0)
            {
                return;
            }

            m_StopCallback?.Invoke();
        }

        // -----------------------------------------------------------------------------------------------------------------------------------------
        // virtual methods

        public virtual IEnumerator OnLoaded(UIBase targetLayer)
        {
            yield break;
        }

        public virtual bool OnClick(string name, GameObject gameObject, PointerEventData pointer, UISound uiSound)
        {
            return false;
        }

        public virtual bool OnTouchDown(string name, GameObject gameObject, PointerEventData pointer)
        {
            return false;
        }

        public virtual bool OnTouchUp(string name, GameObject gameObject, PointerEventData pointer)
        {
            return false;
        }

        public virtual bool OnDrag(string name, GameObject gameObject, PointerEventData pointer)
        {
            return false;
        }

        protected virtual void OnDestroy() { }
    }
}