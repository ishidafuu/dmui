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
        // SpriteをロードしてImageに表示する
        Addressables.LoadAsset<Sprite>(target).Completed += op =>
        {
            image.sprite = op.Result;
        };
    }

    public async UniTask OnLoadedBase()
    {
        // await Addressables.LoadAssetAsync<Sprite>("assetbundle/nobi").Completed += op => { image.sprite = op.Result; };
    }
}