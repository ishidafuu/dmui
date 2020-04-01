namespace DM
{
    public class AppIdHeaderParam : IRequestHeaderParam
    {
        public AppIdHeaderParam() : base("App-Id") { }

        protected override string HeaderValue() => CryptographicProtection.EncryptAppId();
    }
}