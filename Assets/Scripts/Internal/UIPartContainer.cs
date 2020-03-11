using System.Collections;
using UnityEngine;

namespace DM
{
    public class UIPartContainer
    {
        protected Object m_Prefab;
        public UIPart Part { get; private set; }
        private UITouchListener[] m_Listeners;

        public UIPartContainer(UIPart part)
        {
            Part = part;
        }

        public IEnumerator LoadAndSetup(UIBaseLayer targetLayer)
        {
            if (Part.Root == null && !string.IsNullOrEmpty(Part.PrefabPath))
            {
                PrefabReceiver receiver = new PrefabReceiver();
                yield return UIController.Implements.PrefabLoader.Load(Part.PrefabPath, receiver);
                m_Prefab = receiver.m_Prefab;

                if (m_Prefab != null)
                {
                    GameObject g = Object.Instantiate(m_Prefab) as GameObject;
                    Part.Root = g.transform;
                }
            }

            if (Part.Root == null)
            {
                Part.Root = new GameObject("root").transform;
            }

            Part.Root.gameObject.SetActive(false);

            CollectComponents(Part.Root.gameObject, targetLayer);

            yield return Part.OnLoaded(targetLayer.Base);

            Part.Root.gameObject.SetActive(true);
        }

        public virtual void Destroy()
        {
            UIController.Implements.PrefabLoader.Release(Part.PrefabPath, m_Prefab);
            m_Prefab = null;

            Part.Destroy();
            Part = null;


            foreach (var item in m_Listeners)
            {
                item.ResetUI();
            }

            m_Listeners = null;
        }

        protected void CollectComponents(GameObject target, UIBaseLayer layer)
        {
            m_Listeners = target.GetComponentsInChildren<UITouchListener>();
            foreach (var item in m_Listeners)
            {
                item.SetUI(layer, Part);
            }

            Animator[] animators = target.GetComponentsInChildren<Animator>();
            Part.Animators = animators;
        }
    }
}