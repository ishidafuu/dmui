using MessagePack;

namespace DM
{
    [MessagePackObject(true)]
    public class MessagePackTest
    {
        public bool IsNetworkError { get; set; }
        public bool IsHttpError { get; set; }
        public int ResponseCode { get; set; }
        public int Sec { get; set; }
    }
}