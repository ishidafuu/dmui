namespace DM
{
    public class DispatchedEvent
    {
        public string Name { get; }

        public object Param { get; }

        public DispatchedEvent(string name, object param)
        {
            Name = name;
            Param = param;
        }
    }
}