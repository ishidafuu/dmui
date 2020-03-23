﻿using UniRx.Async;
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
        
        public async UniTask LoadTableAsync()
        {
            var client = new AirTableClient("key3NNedymjZdyPup");
            var clientBase = client.GetBase("appsj9JjmBwaF3Hbz");
            var allRows =  await clientBase.LoadTableAsync<ActionTable>();
            foreach (var row in allRows)
            {
                // 1レコードずつ取り出す処理 
                Debug.Log(row.Name + ":" +row.Notes);
            }
        }
        
        public class ActionTable
        {
            public string Name;
            public string Notes;
        }
    }
}