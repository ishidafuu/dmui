﻿using UniRx.Async;
using UnityEngine;

namespace DM
{
    public class UIPartContainer
    {
        private const string NONE_ROOT_OBJECT_NAME = "root";
        protected Object m_Prefab;
        private UITouchListener[] m_Listeners;

        public UIPart Part { get; private set; }

        public UIPartContainer(UIPart part)
        {
            Part = part;
        }

        public async UniTask LoadAndSetup(UIBaseLayer targetLayer)
        {
            if (Part.RootTransform == null && !string.IsNullOrEmpty(Part.PrefabPath))
            {
                await LoadPrefab();
            }

            if (Part.RootTransform == null)
            {
                Part.RootTransform = new GameObject(NONE_ROOT_OBJECT_NAME).transform;
            }
    
            GameObject rootGameObject = Part.RootTransform.gameObject;
            rootGameObject.SetActive(false);
            CollectComponents(rootGameObject, targetLayer);
            Part.TargetLayer = targetLayer.Base;
            
            await Part.OnLoadedPart(targetLayer.Base);

            rootGameObject.SetActive(true);
        }

        private async UniTask LoadPrefab()
        {
            PrefabReceiver receiver = new PrefabReceiver();
            
            await UIController.Implements.PrefabLoader.Load(Part.PrefabPath, receiver);

            m_Prefab = receiver.m_Prefab;
            if (m_Prefab == null)
            {
                Debug.LogError($"UIPartContainer Error LoadPrefab {Part.PrefabPath}");
                return;
            }

            GameObject gameObject = Object.Instantiate(m_Prefab) as GameObject;
            if (gameObject != null)
            {
                Part.RootTransform = gameObject.transform;
            }
        }

        public virtual void Destroy()
        {
            UIController.Implements.PrefabLoader.Release(Part.PrefabPath, m_Prefab);
            m_Prefab = null;

            Part.Destroy();
            Part = null;

            foreach (UITouchListener item in m_Listeners)
            {
                item.ClearPart();
            }

            m_Listeners = null;
        }

        protected void CollectComponents(GameObject target, UIBaseLayer layer)
        {
            CollectTouchListener(target, layer);

            CollectAnimator(target);
        }

        private void CollectTouchListener(GameObject target, UIBaseLayer layer)
        {
            m_Listeners = target.GetComponentsInChildren<UITouchListener>();
            foreach (UITouchListener item in m_Listeners)
            {
                item.SetPart(layer, Part);
            }
        }

        private void CollectAnimator(GameObject target)
        {
            Animator[] animators = target.GetComponentsInChildren<Animator>();
            Part.Animators = animators;
        }
    }
}