namespace DM
{
    public interface IJsonSerializer
    {
        string Serialize<T>(T obj);
    }

    public interface IJsonDeserializer
    {
        T Deserialize<T>(string data);
    }
}