using System.Collections;
using UnityEngine;

namespace DM
{
    public class UIPartContainer
    {
        private const string NONE_ROOT_OBJECT_NAME = "root";
        private UITouchListener[] m_Listeners;
        
        protected Object m_Prefab;
        
        public UIPart Part { get; private set; }

        public UIPartContainer(UIPart part)
        {
            Part = part;
        }

        public IEnumerator LoadAndSetup(UIBaseLayer targetLayer)
        {
            if (Part.RootTransform == null && !string.IsNullOrEmpty(Part.PrefabPath))
            {
                yield return LoadPrefab();
            }

            if (Part.RootTransform == null)
            {
                Part.RootTransform = new GameObject(NONE_ROOT_OBJECT_NAME).transform;
            }

            var rootGameObject = Part.RootTransform.gameObject;
            rootGameObject.SetActive(false);

            CollectComponents(rootGameObject, targetLayer);

            yield return Part.OnLoadedPart(targetLayer.Base);

            rootGameObject.SetActive(true);
        }

        private IEnumerator LoadPrefab()
        {
            PrefabReceiver receiver = new PrefabReceiver();
            yield return UIController.Implements.PrefabLoader.Load(Part.PrefabPath, receiver);

            m_Prefab = receiver.m_Prefab;
            
            if (m_Prefab == null)
            {
                yield break;
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
                item.ClearUI();
            }

            m_Listeners = null;
        }

        protected void CollectComponents(GameObject target, UIBaseLayer layer)
        {
            m_Listeners = target.GetComponentsInChildren<UITouchListener>();
            foreach (UITouchListener item in m_Listeners)
            {
                item.SetUI(layer, Part);
            }

            Animator[] animators = target.GetComponentsInChildren<Animator>();
            Part.Animators = animators;
        }
    }
}