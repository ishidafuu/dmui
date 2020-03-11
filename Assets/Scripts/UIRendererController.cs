using System.Collections.Generic;
using UnityEngine;

namespace DM
{
    public class UIRendererController : UIVisibleController
    {
        protected override IEnumerable<Component> GetComponents(GameObject target)
        {
            return target.GetComponentsInChildren<Renderer>();
        }

        protected override void SetEnable(Component component, bool enable)
        {
            ((Renderer)component).enabled = enable;
        }

        protected override bool IsEnable(Component component)
        {
            return ((Renderer)component).enabled;
        }
    }
}