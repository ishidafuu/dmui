//  AddressableAssetAddressClassCreator.cs
//  http://kan-kikuchi.hatenablog.com/entry/AddressableAssetAddressClassCreator
//
//  Created by kan.kikuchi on 2019.09.16.

using System.IO;
using System.Linq;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.AddressableAssets.Settings;
using UnityEngine;

namespace DM
{
    /// <summary>
    /// AddressableAssetのAddressを管理する定数クラスを自動生成するクラス
    /// </summary>
    public class AddressableAssetAddressClassCreator : AssetPostprocessor
    {
        //変更をチェックするディレクトリのパス
        private const string TARGET_DIRECTORY_PATH = "Assets/AddressableAssetsData/AssetGroups";

        //定数クラスを生成するディレクトリのパス
        private const string EXPORT_DIRECTORY_PATH = "Assets/Scripts";
        private const string DATA_ROOT = "Assets/AssetBundles/";

        //=================================================================================
        //変更の監視
        //=================================================================================
        
        
        //対象のディレクトリ以下の変更をチェック
        private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets,
            string[] movedAssets,
            string[] movedFromAssetPaths)
        {
            List<string> assetsList = new List<string>();
            assetsList.AddRange(importedAssets);
            assetsList.AddRange(deletedAssets);
            assetsList.AddRange(movedAssets);
            assetsList.AddRange(movedFromAssetPaths);

            List<string> targetDirectoryNameList = new List<string>()
            {
                TARGET_DIRECTORY_PATH,
            };

            //変更があったら定数クラス作成
            if (ExistsDirectoryInAssets(assetsList, targetDirectoryNameList))
            {
                Create();
                Debug.Log("OnPostprocessAllAssets");
            }
        }

        //入力されたassetsのパスの中に、親ディレクトリの名前ががtargetDirectoryNameListのものが一つでもあるか
        private static bool ExistsDirectoryInAssets(List<string> assetPathsList, List<string> targetDirectoryNameList)
        {
            return assetPathsList.Select(Path.GetDirectoryName).Intersect(targetDirectoryNameList).Any();
        }
        

        //=================================================================================
        //作成
        //=================================================================================

        //定数クラス作成
        [MenuItem("Tools/AddressableAsset Constants Class")]
        public static void Create()
        {
            //アドレスとラベルをまとめるやつ
            var addressDict = new Dictionary<string, string>();
            var labelDict = new Dictionary<string, string>();

            //対象のディレクトリ以下のアセットを全て取得し、アドレスとラベルを記録
            foreach (AddressableAssetEntry entry in LoadAll<AddressableAssetGroup>(TARGET_DIRECTORY_PATH)
                .SelectMany(assetGroup => assetGroup.entries))
            {
                if (addressDict.ContainsKey(entry.address))
                {
                    Debug.LogError($"{entry.address}というアドレスが重複しています！");
                }

                addressDict[entry.address] = entry.address;

                foreach (var label in entry.labels)
                {
                    labelDict[label] = label;
                }
            }

            //記録したDictionaryから定数クラスを生成
            ConstantsClassCreator.Create("AssetAddress", "AddressableAssetのAddressを管理する定数クラス", EXPORT_DIRECTORY_PATH,
                addressDict);
            ConstantsClassCreator.Create("AssetLabel", "AddressableAssetのLabelを管理する定数クラス", EXPORT_DIRECTORY_PATH,
                labelDict);
        }

        //ディレクトリのパス(Assetsから)と型を設定し、Objectを読み込む。存在しない場合は空のListを返す
        private static List<T> LoadAll<T>(string directoryPath) where T : Object
        {
            List<T> assetList = new List<T>();

            //指定したディレクトリに入っている全ファイルを取得(子ディレクトリも含む)
            string[] filePathArray = Directory.GetFiles(directoryPath, "*", SearchOption.AllDirectories);

            //取得したファイルの中からアセットだけリストに追加する
            foreach (string filePath in filePathArray)
            {
                T asset = AssetDatabase.LoadAssetAtPath<T>(filePath);
                if (asset != null)
                {
                    assetList.Add(asset);
                }
            }

            return assetList;
        }
    }
}