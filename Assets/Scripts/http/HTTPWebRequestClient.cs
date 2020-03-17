﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using UnityEngine.Networking;
using Debug = UnityEngine.Debug;

namespace DM
{
    public class HttpWebRequestClient
    {
        public struct HttpResponsePack
        {
            public bool m_IsNetworkError;
            public bool m_IsHttpError;
            public int m_ResponseCode;
            public int m_ResponseTime;
            public string m_Data;
        }

        public delegate void HttpResponseDataDelegate(HttpWebRequestClient client, ref HttpResponsePack pack);

        private readonly int m_TimeOut;
        private readonly List<IMultipartFormSection> m_FormSection = new List<IMultipartFormSection>();
        private string m_ApiUrl = null;
        private HttpResponseDataDelegate m_ResponseDataDelegate = null;
        private readonly IReadOnlyList<IRequestHeaderParam> m_RequestHeaderParams = null;
        private readonly IJsonSerializer m_Serializer;

        public HttpWebRequestClient(
            IJsonSerializer serializer,
            IReadOnlyList<IRequestHeaderParam> requestHeaders,
            int timeOut = 30)
        {
            m_Serializer = serializer;
            m_RequestHeaderParams = requestHeaders;
            m_TimeOut = timeOut;
        }

        public IEnumerator Request<T>(string url, T param, HttpResponseDataDelegate action)
        {
            Debug.Log($"<color=blue>{url}</color>");

            m_FormSection.Clear();
            m_ApiUrl = url;
            m_ResponseDataDelegate = action;

            InitRequestHeader();

            string requestJson = m_Serializer.Serialize(param);

            foreach (var item in m_RequestHeaderParams)
            {
                Debug.Log($"<color=blue>requestHeaderParam:{item.cacheHeaderValue}</color>");
                Debug.Log($"<color=blue>requestJson:{requestJson}</color>");
            }

            SetInnerApiParam(requestJson);
            yield return InnerRetry();
        }

        private void InitRequestHeader()
        {
            foreach (IRequestHeaderParam item in m_RequestHeaderParams)
            {
                item.Init();
            }
        }

        private void SetInnerApiParam(string json)
        {
            if (string.IsNullOrEmpty(json))
            {
                json = "{}";
            }

            // binary化 -> base64
            byte[] encodingUtf8 = Encoding.UTF8.GetBytes(json);
            string base64 = Convert.ToBase64String(encodingUtf8);

            //"text/plain; encoding=utf-8"
            // "application/x-www-form-urlencoded; encoding=utf-8");
            MultipartFormDataSection dataSection = new MultipartFormDataSection("api_param", base64,
                "application/json; encoding=utf-8");

            m_FormSection.Add(dataSection);
        }

        public IEnumerator Retry()
        {
            foreach (IRequestHeaderParam item in m_RequestHeaderParams)
            {
                item.Reset();
            }

            yield return InnerRetry();
        }

        private IEnumerator InnerRetry()
        {
            using (var www = UnityWebRequest.Post(m_ApiUrl, m_FormSection))
            {
                www.timeout = m_TimeOut;

                // Headerデータをセットする
                foreach (var item in m_RequestHeaderParams)
                    www.SetRequestHeader(item.valueName, item.cacheHeaderValue);

                Stopwatch stop = new Stopwatch();
                stop.Start();

                yield return www.SendWebRequest();

                int responseTime = (int)stop.ElapsedMilliseconds;

                HttpResponsePack res = new HttpResponsePack
                {
                    m_IsHttpError = www.isHttpError,
                    m_IsNetworkError = www.isNetworkError,
                    m_ResponseCode = (int)www.responseCode,
                    m_Data = www.downloadHandler?.text,
                    m_ResponseTime = responseTime,
                };

                Debug.Log(
                    $"<color=blue>SEND_RECEIVE:{responseTime}milli, httpCode:{www.responseCode}, error:{www.error}</color>");
                Debug.Log($"<color=blue>responseJson:{res.m_Data}</color>");

                m_ResponseDataDelegate(this, ref res);
            }
        }
    }
}