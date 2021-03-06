﻿namespace DM
{
	/// <summary>
	/// Retry時はパラメータが不変
	/// </summary>
    public abstract class IRequestHeaderParam
    {
        // headerValueの名前
        public readonly string m_ValueName = null;
        
        // retry時に同じ値を送る必要があるため
        public string m_CacheHeaderValue = null;        

        /// <summary>
        /// 実際の取得処理を抽象化
        /// </summary>
        /// <returns></returns>
        protected abstract string HeaderValue();

        public IRequestHeaderParam(string valueName)
        {
            m_ValueName = valueName;
        }

        /// <summary>
        /// 初期化
        /// ※Cacheしたいものもあるため、別途retry用のResetも用意する
        /// </summary>
        public void Init() => m_CacheHeaderValue = HeaderValue();

        /// <summary>
        /// Retry時に呼ぶ
        /// ※DefaultでCacheあり
        /// </summary>
        public virtual void Reset() { }
    }
}