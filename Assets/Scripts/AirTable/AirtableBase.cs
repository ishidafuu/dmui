using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.Serialization;
using UniRx.Async;

namespace DM
{
    public class AirTableBase
    {
        private readonly AirTableClient m_Client;
        private readonly string m_BaseId;

        internal AirTableBase(AirTableClient client, string baseId)
        {
            m_Client = client;
            m_BaseId = baseId;
        }

        public UniTask<T[]> LoadTableAsync<T>()
        {
            return LoadTableAsync<T>(typeof(T).Name);
        }

        private async UniTask<T[]> LoadTableAsync<T>(string tableName)
        {
            var result = new List<T>();
            var offset = "0";
            while (!string.IsNullOrWhiteSpace(offset))
            {
                string url = $"https://api.airtable.com/v0/{m_BaseId}/{tableName}?api_key={m_Client.ApiKey}&offset={offset}";
                HttpResponseMessage message = await m_Client.Get(url);

                if (message.StatusCode != HttpStatusCode.OK)
                {
                    switch (message.StatusCode)
                    {
                        case HttpStatusCode.BadRequest:
                            throw new AirTableBadRequestException();
                        case HttpStatusCode.Forbidden:
                            throw new AirTableForbiddenException();
                        case HttpStatusCode.NotFound:
                            throw new AirTableNotFoundException();
                        case HttpStatusCode.PaymentRequired:
                            throw new AirTablePaymentRequiredException();
                        case HttpStatusCode.Unauthorized:
                            throw new AirTableUnauthorizedException();
                        case HttpStatusCode.RequestEntityTooLarge:
                            throw new AirTableRequestEntityTooLargeException();
                        case (HttpStatusCode)422:
                            var error = Utf8Json.JsonSerializer.Deserialize<dynamic>(await message.Content.ReadAsByteArrayAsync());
                            throw new AirTableInvalidRequestException(error?["error"]?["message"]);
                        case (HttpStatusCode)429:
                            throw new AirTableTooManyRequestsException();
                        default:
                            throw new AirTableUnrecognizedException(message.StatusCode);
                    }
                }

                var jsonBody = Utf8Json.JsonSerializer.Deserialize<JsonBody<T>>(await message.Content.ReadAsByteArrayAsync());
                offset = jsonBody.m_Offset;

                result.AddRange(jsonBody.m_Records.Select(x => x.m_Body));
            }
            
            while (!string.IsNullOrWhiteSpace(offset)) { }

            return result.ToArray();
        }
    }

    public class JsonBody<T>
    {
        [DataMember(Name = "records")]
        public Record<T>[] m_Records;

        [DataMember(Name = "offset")]
        public string m_Offset;
    }

    public class Record<T>
    {
        [DataMember(Name = "id")]
        public string m_Id;

        [DataMember(Name = "fields")]
        public T m_Body;

        [DataMember(Name = "createdTime")]
        public string m_CreatedTime;
    }
}