using System;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;
using Random = UnityEngine.Random;

namespace DM
{
    //暗号保護
    public static class CryptographicProtection
    {
        //APPID用
        private struct KeyParam
        {
            public KeyParam(string index, string hash)
            {
                m_IndexKey = index;
                m_HashKey = hash;
            }

            public readonly string m_IndexKey;
            public readonly string m_HashKey;
        };

        static readonly KeyParam[] _appIdKeys = new KeyParam[]
        {
            new KeyParam("0962FF67D7CFFE20", "HhaZP5LaQnUHSCJa"),
            new KeyParam("11D3C4CB1C450E81", "upnyiquAHObwimx9"),
            new KeyParam("1CDEE05358712732", "ydYWxOAvVAqm0GcF"),
            new KeyParam("243F2F865B959D63", "TixEvVymp1t8Rfqa"),
            new KeyParam("25FADBCB7173F764", "m61oPyMV04blPm53"),
            new KeyParam("29A273350DD46D95", "lbHL69ZJDylugCIG"),
            new KeyParam("29E78AC497552F06", "ASJPrYnynEUj54co"),
            new KeyParam("2D0793F6DD7CC9A7", "VfzhEBRNryA7cuFK"),
            new KeyParam("2EE25CA6B1EEC6D8", "17dNS0r9eoJEVM6k"),
            new KeyParam("2F6BF25F18E8C1A9", "qzKtgtFEvG40mzw9"),
            new KeyParam("3A364411281632DA", "EtVtMAqdBo1dsLlS"),
            new KeyParam("3A7884E2A6489EFB", "DAZzv08WPcHpc44B"),
            new KeyParam("3AAA56DFE61672FC", "4Dx2hV7vnxFgIYBL"),
            new KeyParam("4732F5292AB8623D", "rwEelNms8zDdpOE7"),
            new KeyParam("4F07470D6305B70E", "cT7SEZAqs2A3gwlX"),
            new KeyParam("513C13741473A18F", "z0RhA8iH0XLRuyKu"),
        };

        private const string HASH_KEY_APP_ID = "t4jUkmvbzSPEH9f8";

        // OSVersion取得
        private static string s_OsVersion = null;

        /// <summary>
        /// 暗号化
        /// </summary>
        /// <param name="inByte"></param>
        /// <param name="aesKey"></param>
        /// <param name="outByte"></param>
        /// <param name="outIv"></param>
        private static void Encrypt(byte[] inByte, byte[] aesKey, out byte[] outByte, out byte[] outIv)
        {
            using AesManaged aes = new AesManaged
            {
                KeySize = 128, 
                BlockSize = 128, 
                Mode = CipherMode.CBC, 
                Padding = PaddingMode.PKCS7
            };
            aes.GenerateIV();

            aes.Key = aesKey;

            using ICryptoTransform transform = aes.CreateEncryptor();
            byte[] encryptedByte = transform.TransformFinalBlock(inByte, 0, inByte.Length);

            outByte = encryptedByte;
            outIv = aes.IV;
        }

        /// <summary>
        /// 暗号化 -> base64
        /// </summary>
        /// <param name="inByte"></param>
        /// <param name="aesKey"></param>
        /// <param name="outEncrypted"></param>
        /// <param name="outIv"></param>
        private static void EncryptBase64(string inByte, string aesKey, out string outEncrypted, out string outIv)
        {
            byte[] byteUuid = Encoding.UTF8.GetBytes(inByte);
            byte[] hash = Encoding.UTF8.GetBytes(aesKey);

            Encrypt(byteUuid, hash, out var outEncryptedByte, out var outIvByte);

            outEncrypted = Convert.ToBase64String(outEncryptedByte);
            outIv = Convert.ToBase64String(outIvByte);
        }

        /// <summary>
        /// uuidの暗号化
        /// </summary>
        /// <param name="uuid"></param>
        /// <returns></returns>
        private static string EncryptAppId(string uuid)
        {
            int index = Random.Range(0, _appIdKeys.Length);

            // Base64で変換
            EncryptBase64(uuid, _appIdKeys[index].m_HashKey, out var encryptedBase64, out var ivBase64);


            SHA512 sha512 = new SHA512Managed();
            string uuidHash = uuid + HASH_KEY_APP_ID;
            string v2 = Convert.ToBase64String(sha512.ComputeHash(Encoding.UTF8.GetBytes(uuidHash)));
            string v3 = Convert.ToBase64String(Encoding.UTF8.GetBytes(_appIdKeys[index].m_IndexKey));


            string v4 = GetOSVersion();
            string v5 = GetApplicationVersion();
            // var v6 = GetAssetBundleVersion();
            string v6 = "1.0.0";

#if UNITY_EDITOR
            string v7 = "OS";
            string v8 = "PC";
#else
		    var v7 = SystemInfo.operatingSystem;
		    v7 = v7.Replace("(","");
		    v7 = v7.Replace(")","");

		    var v8 = SystemInfo.deviceModel;
		    v8 = v8.Replace("(","");
		    v8 = v8.Replace(")","");

#endif
            return $"{encryptedBase64},{ivBase64},{v2},{v3},{v4},{v5},{v6},{v7},{v8}";
        }

        /// <summary>
        /// uuidの暗号化
        /// </summary>
        /// <returns></returns>
        public static string EncryptAppId()
        {
            var uuid = GetOrCreateUUId();
            var res = EncryptAppId(uuid);
            return res;
        }


        // 端末情報 ---------------------------------------------------
        private static string GetOSVersion()
        {
            if (s_OsVersion == null)
            {
                string osVersion = "";
#if UNITY_EDITOR
#if UNITY_IOS
                osVersion = "1." + Application.unityVersion;
#elif UNITY_ANDROID
                osVersion = "2." + Application.unityVersion;
#else
                osVersion = "0." + Application.unityVersion;
#endif

#elif UNITY_IOS
                osVersion = "1." + SystemInfo.operatingSystem;

#elif UNITY_ANDROID
                osVersion = "2." + SystemInfo.operatingSystem;

#endif
                s_OsVersion = osVersion;
            }

            return s_OsVersion;
        }

        private static string GetApplicationVersion() => Application.version;

        // public static string GetAssetBundleVersion() => AssetBundleManager.AssetBundleVersion;

        // ユーザー情報 ------------------------------------------------
        static readonly string _uuid_Key = "uuid";

        private static string GetOrCreateUUId()
        {
            string uuid = UuId;
            // UUID取得 
            if (!string.IsNullOrEmpty(uuid))
            {
                return uuid;
            }

            // まだなければ生成して保存 
            uuid = Guid.NewGuid().ToString();
            UuId = uuid;
            PlayerPrefs.Save();

            return uuid;
        }

        private static string UuId
        {
            get => PlayerPrefs.GetString(_uuid_Key, string.Empty);
            set => PlayerPrefs.SetString(_uuid_Key, value);
        }

        /// <summary>
        /// UUIDが被ったときなどに使用
        /// </summary>
        public static void DeleteUuIdKey()
        {
            Debug.LogWarning("--------Delete PlayerPrefs UUID----------");
            PlayerPrefs.DeleteKey(_uuid_Key);
        }

        private const string MAKE_USER = "make_user";

        /// <summary>
        /// ユーザーの作成可否
        /// ※以前はユーザー作成済みの判定のみの為に、独自暗号化したUserIdを保存していたので、
        /// 安全の為唯のbool値を保存するようにしている
        /// </summary>
        private static bool UserCreated
        {
            get => PlayerPrefs.GetInt(MAKE_USER, 0) != 0;
            set => PlayerPrefs.SetInt(MAKE_USER, value ? 1 : 0);
        }

        public static void DeleteUser()
        {
            Debug.LogWarning("--------Delete PlayerPrefs User----------");
            PlayerPrefs.DeleteKey(MAKE_USER);
        }

        public static void DeleteAll()
        {
            Debug.LogWarning("--------Delete PlayerPrefs All----------");
            PlayerPrefs.DeleteAll();
        }

        /// <summary>
        /// ユーザーデータの作成
        /// </summary>
        /// <param name="cryptId"></param>
        public static void CreateUser(string cryptId)
        {
            UserCreated = true;
            CryptId = cryptId;
            PlayerPrefs.Save();
        }

        static readonly string _crypt = "crypt";

        /// <summary>
        /// 暗号化UserID
        /// </summary>
        /// <returns></returns>
        private static string CryptId
        {
            get => PlayerPrefs.GetString(_crypt, string.Empty);
            set => PlayerPrefs.SetString(_crypt, value);
        }


        public static bool IsFinishConfirmAge() => PlayerPrefs.GetInt("confirmAge", 0) != 0;

        public static void CreateFinishConfirmAge()
        {
            PlayerPrefs.SetInt("confirmAge", 1);
            PlayerPrefs.Save();
        }

        public static void SaveAgreement()
        {
            PlayerPrefs.SetInt("agreement", 1);
            PlayerPrefs.Save();
        }

        public static bool IsAgreementOk() => PlayerPrefs.GetInt("agreement", 0) != 0;

        /// <summary>
        /// 引き継ぎ登録済みか?
        /// </summary>
        /// <returns></returns>
        public static bool IsTransfer() => PlayerPrefs.GetInt("transfer", 0) != 0;

        /// <summary>
        /// 引き継ぎ状態を更新
        /// </summary>
        public static void TransferRegister()
        {
            PlayerPrefs.SetInt("transfer", 1);
            PlayerPrefs.Save();
        }

        /// <summary>
        /// チュートリアルを終えているか？
        /// </summary>
        /// <returns></returns>
        public static bool IsTutorial() => PlayerPrefs.GetInt("tutorial", 1) != 0;

        /// <summary>
        /// チュートリアルの状態の更新
        /// </summary>
        public static void FinishTutorial()
        {
            PlayerPrefs.SetInt("tutorial", 0);
            PlayerPrefs.Save();
        }
    }
}