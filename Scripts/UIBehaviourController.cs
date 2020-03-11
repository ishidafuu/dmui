using System.Collections.Generic;
using UnityEngine;

namespace DM
{
    public class UIBehaviourController<T> : UIVisibleController where T : Behaviour
    {
        protected override IEnumerable<Component> GetComponents(GameObject target)
        {
            return target.GetComponentsInChildren<T>();
        }

        protected override void SetEnable(Component component, bool enable)
        {
            ((T)component).enabled = enable;
        }

        protected override bool IsEnable(Component component)
        {
            return ((T)component).enabled;
        }
    }
}