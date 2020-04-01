// namespace DM
// {
//     using System.Collections;
//     using System.Collections.Generic;
//     using UnityEngine;
//
//     /// <summary>
//     /// Moc用の通信しないHTTPClientだよ
//     /// </summary>
//     public class NoCommunicationHTTPClient : HTTPWebRequestClient
//     {
//         public NoCommunicationHTTPClient(
//             IJsonSerializer serializer, 
//             IReadOnlyList<IRequestHeaderParam> _requestHeaders) : base(serializer, _requestHeaders)
//         {
//
//         }
//
//         public override IEnumerator Request<T>(string url, T param, HTTPResponseDataDel action)
//         {
//             LogGer.Log($"<color=blue>{url}</color>");
//
//             responseDataDel = action;
//             yield return Retry();
//         }
//
//         public override IEnumerator Retry()
//         {
//             yield return new WaitForSecondsRealtime(0.2f);
//
//             var res = new HTTPResponsePack
//             {
//                 isHTTPError = false,
//                 isNetworkError = false,
//                 responseCode = 0,
//                 data = null,
//             };
//             responseDataDel(this, ref res);
//         }
//     }
// }
