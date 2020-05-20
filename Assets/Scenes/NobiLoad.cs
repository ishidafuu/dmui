using System;
using UniRx.Async;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

namespace DM
{
    public class NobiLoad : MonoBehaviour
    {
        public Image image;
        [SerializeField] AssetReference target;

        private void Start()
        {
            // Addressables.DownloadDependenciesAsync("default").Completed += op =>
            // {
            //     Debug.Log($"DownloadDependenciesAsync Completed {op.Result}");
            //     
            //     Resources.UnloadUnusedAssets();
            // };
            // Addressables.GetDownloadSize(target).Completed += op =>
            // {
            //     Debug.Log($"GetDownloadSize{op.Result}");
            //     // SpriteをロードしてImageに表示する    
            //     Addressables.LoadAsset<Sprite>(target).Completed += op2 => { image.sprite = op2.Result; };
            // };]
            // LoadAssetAsync();

            LoadTableAsync();
        }

        public async UniTask LoadAssetAsync()
        {
            var asdf = await Addressables.LoadAssetAsync<Sprite>(AssetAddress.JPG_TETSUWO).Task;
            image.sprite = asdf;
        }

        private async UniTask LoadTableAsync()
        {
            AirTableClient client = new AirTableClient("key3NNedymjZdyPup");
            AirTableBase clientBase = client.GetBase("applzeJ39lPimqZgv");
            try
            {
                Common[] allRows = await clientBase.LoadTableAsync<Common>();
                foreach (Common row in allRows)
                {
                    // 1レコードずつ取り出す処理 
                    Debug.Log(row.Name + ":" + row.Notes);
                }
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                throw;
            }
        }

        public class Common
        {
            public string Name;
            public string Notes;
        }
    }
}