using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace DM
{
    public abstract class AirTableClient : IDisposable
    {
        internal string ApiKey { get; }

        private readonly HttpClient m_HttpClient = new HttpClient();

        protected AirTableClient(string apiKey)
        {
            ApiKey = apiKey;
        }

        public void Dispose()
        {
            m_HttpClient.Dispose();
        }

        public AirTableBase GetBase(string baseId)
        {
            return new AirTableBase(this, baseId);
        }

        internal Task<HttpResponseMessage> Get(string url)
        {
            return m_HttpClient.GetAsync(url);
        }
    }
}
