using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace DM
{
    public class UIDispatchController
    {
        private Queue<DispatchedEvent> m_DispatchedEvents;
        private int m_TouchOffCount;

        public UIDispatchController()
        {
            m_DispatchedEvents = new Queue<DispatchedEvent>();
            
        }

        public void RunDispatchedEvents(UIBaseLayerController layerController)
        {
            if (m_DispatchedEvents.Count == 0)
            {
                return;
            }

            while (m_DispatchedEvents.Count > 0)
            {
                DispatchedEvent e = m_DispatchedEvents.Dequeue();
                layerController.ForEachOnlyActive(layer => { layer.Base.OnDispatchedEvent(e.EventName, e.Param); });
            }

            m_DispatchedEvents.Clear();
        }
        
        public void Dispatch(string eventName, object param)
        {
            m_DispatchedEvents.Enqueue(new DispatchedEvent(eventName, param));
        }
    }
}