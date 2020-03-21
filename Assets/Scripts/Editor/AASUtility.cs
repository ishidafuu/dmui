using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.AddressableAssets.Settings;
using UnityEditor.AddressableAssets.Settings.GroupSchemas;
using UnityEngine;

namespace DM
{
    public class AasUtility : Editor
    {
        private static readonly string s_AssetRoot =
            "Assets" + Path.DirectorySeparatorChar + "Data" + Path.DirectorySeparatorChar;

        private const string ADDRESSABLE_ASSET_SETTINGS = "Assets/AddressableAssetsData/AddressableAssetSettings.asset";
        private const string MENU_NAME = "AASUtility";

        private const string EDITOR_DIR = "Editor";
        private const string DATA_ROOT = "Assets/Data/";

        /// <summary>
        /// データディレクトリのルートから全グループを作成する
        /// </summary>
        [MenuItem(MENU_NAME + "/CreateAssetsDataGroup")]
        public static void CreateGroup()
        {
            List<string> assetPaths = AssetDatabase.GetAllAssetPaths().ToList();
            List<string> dataPath = assetPaths.FindAll(p => p.Contains(DATA_ROOT));
            foreach (string asset in dataPath)
            {
                AddGroup(asset);
            }

            Sort();
        }

        private static void AddGroup(string asset)
        {
            string dir = Path.GetDirectoryName(asset);
            if (dir == null) return;

            //グループに含めていいかのチェック
            //ディレクトリとエディターディレクトリのアセットは含めない
            if (Directory.Exists(asset)) return;
            if (dir.Contains(EDITOR_DIR)) return;

            //アドレス名を求める計算
            dir = dir.Replace(s_AssetRoot, "");
            string group = dir.Replace(Path.DirectorySeparatorChar, '_');
            string address = group + "_" + Path.GetFileNameWithoutExtension(asset);

            AddAssetToGroup(AssetDatabase.AssetPathToGUID(asset), group, address);
        }

        private static void AddAssetToGroup(string assetGuid, string groupName, string address = null)
        {
            AddressableAssetSettings settings = GetSettings();
            AddressableAssetGroup assetGroup = CreateGroup(groupName);
            AddressableAssetEntry assetEntry = settings.CreateOrMoveEntry(assetGuid, assetGroup);
            if (address == null)
            {
                return;
            }

            List<AddressableAssetEntry> entries = new List<AddressableAssetEntry>();
            settings.GetAllAssets(entries);
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
        [MenuItem(MENU_NAME + "/Sort")]
        public static void Sort()
        {
            GetSettings().groups.Sort(new GroupCompare());
        }

        /// <summary>
        /// 空グループを削除
        /// </summary>
        [MenuItem(MENU_NAME + "/Remove/EmptyGroup")]
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
        [MenuItem(MENU_NAME + "/Remove/AllGroup")]
        public static void RemoveAllGroup()
        {
            AddressableAssetSettings settings = GetSettings();
            List<AddressableAssetGroup> groups = settings.groups;
            for (int i = groups.Count - 1; i >= 0; i--)
            {
                if (groups[i].IsDefaultGroup()) continue;
                settings.RemoveGroup(groups[i]);
            }
        }

        /// <summary>
        /// 全アドレスの重複チェック
        /// 重複しているとビルドできない
        /// </summary>
        [MenuItem(MENU_NAME + "/CheckAllAddress")]
        public static void CheckAllAddress()
        {
            AddressableAssetSettings assetSettings = GetSettings();
            List<AddressableAssetEntry> entries = new List<AddressableAssetEntry>();
            assetSettings.GetAllAssets(entries);
            List<string> checkedAddress = new List<string>();

            foreach (AddressableAssetEntry assetEntry in from e in entries
                where !checkedAddress.Contains(e.address)
                let ret = CheckAddress(entries, e.address)
                where !ret
                select e)
            {
                checkedAddress.Add(assetEntry.address);
            }
        }

        /// <summary>
        /// アドレサブルをビルド
        /// </summary>
        [MenuItem(MENU_NAME + "/Build")]
        public static void Build()
        {
            AddressableAssetSettings.BuildPlayerContent();
        }

        /// <summary>
        /// ビルドをクリーン
        /// </summary>
        [MenuItem(MENU_NAME + "/CleanBuild")]
        public static void Clean()
        {
            AddressableAssetSettings.CleanPlayerContent();
        }

        private static bool CheckAddress(List<AddressableAssetEntry> entries, string address)
        {
            List<AddressableAssetEntry> duplicateEntries = entries.FindAll(e => e.address == address);
            if (duplicateEntries.Count <= 1)
            {
                return true;
            }

            {
                string str = "Address=" + address + Environment.NewLine;
                foreach (AddressableAssetEntry e in duplicateEntries)
                {
                    string assetName = Path.GetFileName(e.AssetPath);
                    str += "Group=" + e.parentGroup.Name + "," + "AssetName=" + assetName + Environment.NewLine;
                }

                Debug.LogAssertion("DuplicateAddress" + Environment.NewLine + str);
                return false;
            }

        }

        private static AddressableAssetSettings GetSettings()
        {
            //アドレサブルアセットセッティング取得

            return AssetDatabase.LoadAssetAtPath<AddressableAssetSettings>(ADDRESSABLE_ASSET_SETTINGS);
        }

        private static AddressableAssetGroup CreateGroup(string groupName)
        {
            //アドレサブルアセットセッティング取得
            AddressableAssetSettings assetSettings = GetSettings();
            //スキーマ生成
            List<AddressableAssetGroupSchema> schema = new List<AddressableAssetGroupSchema>()
            {
                CreateInstance<BundledAssetGroupSchema>(),
                CreateInstance<ContentUpdateGroupSchema>()
            };
            //グループの作成
            AddressableAssetGroup assetGroup = assetSettings.groups.Find((g) => g.name == groupName);
            return assetGroup == null
                ? assetSettings.CreateGroup(groupName, false, false, true, schema)
                : assetGroup;
        }

        private static void SetLabelToAsset(List<string> assetGuidList, string label, bool flag)
        {
            AddressableAssetSettings assetSettings = GetSettings();
            //ラベルを追加するように呼んでおく。追加されていないと設定されない。
            assetSettings.AddLabel(label);
            List<AddressableAssetEntry> assetList = new List<AddressableAssetEntry>();
            assetSettings.GetAllAssets(assetList);
            foreach (AddressableAssetEntry asset in assetGuidList.Select(assetGuid =>
                assetList.Find((a) => a.guid == assetGuid)))
            {
                asset?.SetLabel(label, flag);
            }
        }

        private static void RemoveAssetFromGroup(string assetGuid)
        {
            AddressableAssetSettings settings = GetSettings();
            settings.RemoveAssetEntry(assetGuid);
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
}