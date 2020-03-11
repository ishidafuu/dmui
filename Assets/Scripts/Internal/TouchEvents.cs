using UnityEngine.EventSystems;

namespace DM
{
    public class TouchEvent
    {
        public UITouchListener Listener { get; }
        public TouchType Type { get; }
        public PointerEventData Pointer { get; }

        public TouchEvent(UITouchListener listener, TouchType type, PointerEventData pointer)
        {
            Listener = listener;
            Type = type;
            Pointer = pointer;
        }
    }
}