using System;

namespace DM
{
    public class AppTokenHeaderParam : IRequestNoCacheHeaderParam
    {
        private static readonly DateTime s_UnixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);    
        public AppTokenHeaderParam() : base("App-Token") { }

        protected override string HeaderValue() => ((DateTime.Now - s_UnixEpoch).Ticks / 10L).ToString();
    }
}