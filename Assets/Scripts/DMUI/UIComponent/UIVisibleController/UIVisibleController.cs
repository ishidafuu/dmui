using System.Collections.Generic;
using UnityEngine;

namespace DM
{
    public class UIVisibleController
    {
        private Dictionary<Component, bool> m_Components = new Dictionary<Component, bool>();

        public void SetVisible(GameObject target, bool enable)
        {
            if (m_Components == null)
            {
                return;
            }

            if (enable)
            {
                SetVisibleEnable();
            }
            else
            {
                SetVisibleDisable(target);
            }
        }

        private void SetVisibleEnable()
        {
            if (IsVisible())
            {
                return;
            }

            foreach (KeyValuePair<Component, bool> pair in m_Components)
            {
                SetEnable(pair.Key, pair.Value);
            }

            m_Components.Clear();
        }

        private void SetVisibleDisable(GameObject target)
        {
            if (!IsVisible())
            {
                return;
            }

            IEnumerable<Component> components = GetComponents(target);
            foreach (var component in components)
            {
                m_Components.Add(component, IsEnable(component));
                SetEnable(component, false);
            }
        }

        public void Destroy()
        {
            m_Components = null;
        }

        public bool IsVisible()
        {
            return (m_Components.Count == 0);
        }

        protected virtual IEnumerable<Component> GetComponents(GameObject target)
        {
            return null;
        }

        protected virtual void SetEnable(Component component, bool enable) { }

        protected virtual bool IsEnable(Component component)
        {
            return true;
        }
    }
}