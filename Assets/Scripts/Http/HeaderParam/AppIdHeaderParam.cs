namespace DM
{
    /// <summary>
    /// AppId用のデータ
    /// UUID,端末情報など
    /// </summary>
    public class AppIdHeaderParam : IRequestHeaderParam
    {
        public AppIdHeaderParam() : base("App-Id") { }

        protected override string HeaderValue() => CryptographicProtection.EncryptAppId();
    }
}