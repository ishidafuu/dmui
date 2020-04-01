namespace DM
{
    public abstract class IRequestNoCacheHeaderParam : IRequestHeaderParam
    {
        public IRequestNoCacheHeaderParam(string valueName) : base(valueName) { }

        /// <summary>
        /// Cacheなし
        /// </summary>
        public override void Reset() => m_CacheHeaderValue = HeaderValue();
    }
}