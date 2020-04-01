// namespace DM
// {
//     using System.Collections;
//
//     public abstract class IHTTPClient
//     {
//         public struct HTTPResponsePack
//         {
//             public bool isNetworkError;
//             public bool isHTTPError;
//             public string data;
//             public int responseCode;
//             public int milisec;
//         }
//
//         public delegate void HTTPResponseDataDel(IHTTPClient client, ref HTTPResponsePack pack);
//
//         protected IJsonSerializer serializer;
//
//         public IHTTPClient(IJsonSerializer _serializer)//, MonoBehaviour _behaviour)
//         {
//             serializer = _serializer;
//             //behaviour = _behaviour;
//         }
//
//         public abstract IEnumerator Request<T>(string apiURL, T param, HTTPResponseDataDel action);
//
//         /// <summary>
//         /// リスエストのリトライ
//         /// </summary>
//         /// <returns></returns>
//         public abstract IEnumerator Retry();
//
//     }
// }