using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.AddressableAssets.Settings;
using UnityEditor.AddressableAssets.Settings.GroupSchemas;
using UnityEngine;

public class AASUtility : Editor
{
    static public string assetRoot = "Assets" + Path.DirectorySeparatorChar + "Data" + Path.DirectorySeparatorChar;
    const string addressableAssetSettings = "Assets/AddressableAssetsData/AddressableAssetSettings.asset";
    const string menuName = "AASUtility";

    static string editorDir = "Editor";
    const string dataRoot = "Assets/Data/";

    /// <summary>
    /// データディレクトリのルートから全グループを作成する
    /// </summary>
    [MenuItem(menuName + "/CreateAssetsDataGroup")]
    public static void CreateGroup()
    {
        var assetPaths = AssetDatabase.GetAllAssetPaths().ToList();
        var dataPath = assetPaths.FindAll(p => p.Contains(dataRoot));
        foreach (var asset in dataPath)
        {
            AddGroup(asset);
        }

        Sort();
    }

    private static void AddGroup(string asset)
    {
        string dir = Path.GetDirectoryName(asset);

        //グループに含めていいかのチェック
        //ディレクトリとエディターディレクトリのアセットは含めない
        if (Directory.Exists(asset)) return;
        if (dir.Contains(editorDir)) return;

        //アドレス名を求める計算
        var filename = Path.GetFileName(asset);
        dir = dir.Replace(assetRoot, "");
        string group = dir.Replace(Path.DirectorySeparatorChar, '_');
        var address = group + "_" + Path.GetFileNameWithoutExtension(asset);

        AddAssetToGroup(AssetDatabase.AssetPathToGUID(asset), group, address);
    }

    /// <summary>
    /// 任意のアセットをグループに追加
    /// </summary>
    /// <param name="assetGuid"></param>
    /// <param name="groupName"></param>
    /// <param name="address"></param>
    private static void AddAssetToGroup(string assetGuid, string groupName, string address = null)
    {
        AddressableAssetSettings assetSettings = GetSettings();
        AddressableAssetGroup assetGroup = CreateGroup(groupName);
        AddressableAssetEntry assetEntry = assetSettings.CreateOrMoveEntry(assetGuid, assetGroup);
        if (address == null)
        {
            return;
        }

        List<AddressableAssetEntry> entries = new List<AddressableAssetEntry>();
        assetSettings.GetAllAssets(entries);
        if (CheckAddress(entries, address))
        {
            assetEntry.SetAddress(address);
        }
        else
        {
            Debug.LogAssertion("Duplicate Address");
        }
    }

    /// <summary>
    /// 文字列順にソートする
    /// </summary>
    [MenuItem(menuName + "/Sort")]
    public static void Sort()
    {
        var s = GetSettings();
        s.groups.Sort(new GroupCompare());
    }

    /// <summary>
    /// 空グループを削除
    /// </summary>
    [MenuItem(menuName + "/Remove/EmptyGroup")]
    public static void DeleteEmptyGroup()
    {
        var s = GetSettings();
        var groups = s.groups;
        foreach (var g in groups)
        {
            if (g.entries.Count == 0 && !g.IsDefaultGroup())
            {
                s.RemoveGroup(g);
            }
        }
    }

    /// <summary>
    /// 全グループを削除
    /// </summary>
    [MenuItem(menuName + "/Remove/AllGroup")]
    public static void RemoveAllGroup()
    {
        var s = GetSettings();
        var groups = s.groups;
        for (int i = groups.Count - 1; i >= 0; i--)
        {
            if (groups[i].IsDefaultGroup()) continue;
            s.RemoveGroup(groups[i]);
        }
    }

    /// <summary>
    /// 全アドレスの重複チェック
    /// 重複しているとビルドできない
    /// </summary>
    [MenuItem(menuName + "/CheckAllAddress")]
    public static void CheckAllAddress()
    {
        var s = GetSettings();
        List<AddressableAssetEntry> entries = new List<AddressableAssetEntry>();
        s.GetAllAssets(entries);
        List<string> checkedAddress = new List<string>();
        
        foreach (var e in entries)
        {
            //チェック済みアドレスはコンテニュー
            if (checkedAddress.Contains(e.address)) continue;

            //全アセットで重複があるかチェック
            bool ret = CheckAddress(entries, e.address);
            if (!ret)
            {
                checkedAddress.Add(e.address);
            }
        }
    }

    /// <summary>
    /// アドレサブルをビルド
    /// </summary>
    [MenuItem(menuName + "/Build")]
    public static void Build()
    {
        AddressableAssetSettings.BuildPlayerContent();
    }

    /// <summary>
    /// ビルドをクリーン
    /// </summary>
    [MenuItem(menuName + "/CleanBuild")]
    public static void Clean()
    {
        AddressableAssetSettings.CleanPlayerContent();
    }

    /// <summary>
    /// 重複アドレスチェック
    /// </summary>
    /// <param name="entries"></param>
    /// <param name="address"></param>
    /// <returns>重複なしtrue、ありfalse</returns>
    private static bool CheckAddress(List<AddressableAssetEntry> entries, string address)
    {
        var s = GetSettings();
        var duplicateEntries = entries.FindAll(e => e.address == address);
        if (duplicateEntries.Count <= 1)
        {
            return true;
        }

        {
            string str = "Address=" + address + Environment.NewLine;
            foreach (var e in duplicateEntries)
            {
                string assetname = Path.GetFileName(e.AssetPath);
                str += "Group=" + e.parentGroup.Name + "," + "AssetName=" + assetname + Environment.NewLine;
            }

            Debug.LogAssertion("DuplicateAddress" + Environment.NewLine + str);
            return false;
        }

    }


    static AddressableAssetSettings GetSettings()
    {
        //アドレサブルアセットセッティング取得
        var d = AssetDatabase.LoadAssetAtPath<AddressableAssetSettings>(
            addressableAssetSettings
        );
        return d;
    }


    /// <summary>
    /// グループを作成
    /// </summary>
    /// <param name="groupName"></param>
    /// <returns></returns>
    static AddressableAssetGroup CreateGroup(string groupName)
    {
        //アドレサブルアセットセッティング取得
        var s = GetSettings();
        //スキーマ生成
        List<AddressableAssetGroupSchema> schema = new List<AddressableAssetGroupSchema>()
        {
            CreateInstance<BundledAssetGroupSchema>(),
            CreateInstance<ContentUpdateGroupSchema>()
        };
        //グループの作成
        var f = s.groups.Find((g) => { return g.name == groupName; });
        if (f == null)
        {
            return s.CreateGroup(groupName, false, false, true, schema);
        }

        return f;
    }

    /// <summary>
    /// アセットにラベルを一括設定
    /// </summary>
    /// <param name="assetGuidList">対象アセットのGUIDリスト</param>
    /// <param name="label">ラベル名</param>
    /// <param name="flag">ラベル有効、無効のフラグ</param>
    static void SetLabelToAsset(List<string> assetGuidList, string label, bool flag)
    {
        var s = GetSettings();
        //ラベルを追加するように呼んでおく。追加されていないと設定されない。
        s.AddLabel(label);
        List<AddressableAssetEntry> assetList = new List<AddressableAssetEntry>();
        s.GetAllAssets(assetList);
        foreach (var assetGuid in assetGuidList)
        {
            var asset = assetList.Find((a) => a.guid == assetGuid);
            asset?.SetLabel(label, flag);
        }
    }

    /// <summary>
    /// グループからアセットを削除
    /// </summary>
    /// <param name="assetGuid"></param>
    static void RemoveAssetFromGroup(string assetGuid)
    {
        var s = GetSettings();
        s.RemoveAssetEntry(assetGuid);
    }
}


/// <summary>
/// グループを文字列順に並べるソート
/// </summary>
internal class GroupCompare : Comparer<AddressableAssetGroup>
{
    public override int Compare(AddressableAssetGroup x, AddressableAssetGroup y)
    {
        int r = string.CompareOrdinal(x.Name, y.Name);
        if (r > 0)
        {
            return 1;
        }

        if (r < 0)
        {
            return -1;
        }
        else
        {
            return 0;
        }
    }
}