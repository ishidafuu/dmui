namespace DM
{
    public class DispatchedEvent
    {
        public string EventName { get; }

        public object Param { get; }

        public DispatchedEvent(string eventName, object param)
        {
            EventName = eventName;
            Param = param;
        }
    }
}