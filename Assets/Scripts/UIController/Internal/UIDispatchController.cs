using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace DM
{
    public class UIDispatchController
    {
        private readonly Queue<DispatchedEvent> m_DispatchedEvents;
        private int m_TouchOffCount;

        public UIDispatchController()
        {
            m_DispatchedEvents = new Queue<DispatchedEvent>();
        }

        public void CallDispatchedEvents(UIBaseLayerController layerController)
        {
            if (m_DispatchedEvents.Count == 0)
            {
                return;
            }

            while (m_DispatchedEvents.Count > 0)
            {
                DispatchedEvent dispatchedEvent = m_DispatchedEvents.Dequeue();
                layerController.ForEachOnlyActive(layer =>
                {
                    layer.Base.OnDispatchedEvent(dispatchedEvent.EventName, dispatchedEvent.Param);
                });
            }

            m_DispatchedEvents.Clear();
        }
        
        public void Dispatch(string eventName, object param)
        {
            m_DispatchedEvents.Enqueue(new DispatchedEvent(eventName, param));
        }
    }
}