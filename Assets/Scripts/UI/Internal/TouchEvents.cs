using UnityEngine.EventSystems;

namespace DM
{
    public class TouchEvent
    {
        public UITouchListener Listener { get; }
        public EnumTouchType Type { get; }
        public PointerEventData Pointer { get; }

        public TouchEvent(UITouchListener listener, EnumTouchType type, PointerEventData pointer)
        {
            Listener = listener;
            Type = type;
            Pointer = pointer;
        }
    }
}