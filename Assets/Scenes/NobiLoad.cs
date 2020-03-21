using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;
using UniRx.Async;

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
        LoadAssetAsync();
    }

    public async UniTask LoadAssetAsync()
    { 
        var asdf = await Addressables.LoadAssetAsync<Sprite>(AssetAddress.JPG_TETSUWO2).Task;
        image.sprite = asdf;
    }
}