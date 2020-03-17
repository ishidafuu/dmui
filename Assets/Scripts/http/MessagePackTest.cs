using MessagePack;

[MessagePackObject]
public class MessagePackTest
{
    [Key(0)] public bool IsNetworkError { get; set; }
    [Key(1)] public bool IsHttpError { get; set; }
    [Key(2)] public int ResponseCode { get; set; }
    [Key(3)] public int Sec { get; set; }
}