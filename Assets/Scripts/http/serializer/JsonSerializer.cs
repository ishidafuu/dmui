using System;
using UnityEngine;

namespace DM
{
    public class JsonSerializer : IJsonSerializer
    {
        public string Serialize<T>(T obj)
        {
            try
            {
                string json = JsonUtility.ToJson(obj);
                //var encodingUTF8 = System.Text.Encoding.UTF8.GetBytes(json);
                //var base64 = Convert.ToBase64String(encodingUTF8);
                //encodingUTF8 = System.Text.Encoding.UTF8.GetBytes( base64);
                return json;
            }
            catch (Exception e)
            {
                Debug.LogError("jsonSerializer error:" + e);
                return null;
            }
        }
    }

    public class JsonDeserializer : IJsonDeserializer
    {
        public T Deserialize<T>(string data)
        {
            try
            {
                //var json = System.Text.Encoding.UTF8.GetString(data);
                //var decodeByte =  Convert.FromBase64String(data)  ;
                //var strBase64 = System.Text.Encoding.UTF8.GetString( decodeByte);
                var value = JsonUtility.FromJson<T>(data);
                return value;
            }
            catch (Exception e)
            {
                Debug.LogError("jsonDeserializer error:" + e);
                return default(T);
            }
        }
    }
}