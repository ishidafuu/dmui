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
//C:\Program Files\Unity\Editor\Unity.exe -batchmode -quit -logFile .\build.log -projectPath D:\nkpb\dmui -executeMethod DM.AasUtility.Build
    public class AasUtility : Editor
    {
        private static readonly string s_AssetRoot =
            "Assets" + Path.DirectorySeparatorChar + "Data" + Path.DirectorySeparatorChar;

        private const string ADDRESSABLE_ASSET_SETTINGS = "Assets/AddressableAssetsData/AddressableAssetSettings.asset";
        private const string MENU_NAME = "AASUtility";

        private const string EDITOR_DIR = "Editor";
        private const string DATA_ROOT = "Assets/AssetBundles/";
        private const string REMOTE_GROUP = "remote";
        private const string SERVER_DATA_DIR = "ServerData";

        /// <summary>
        /// データディレクトリのルートから全グループを作成する
        /// </summary>
        [MenuItem(MENU_NAME + "/CreateAssetsDataGroup")]
        public static void CreateGroup()
        {
            RemoveAllGroup();

            List<string> dataPath = AssetDatabase.GetAllAssetPaths()
                .Where(p => p.Contains(DATA_ROOT)).ToList();

            foreach (string asset in dataPath)
            {
                AddGroup(asset);
            }

            GetSettings().groups.Sort(new GroupCompare());

            AddressableAssetAddressClassCreator.Create();
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
            string group = asset.Replace(DATA_ROOT, "")
                .Split('/')
                .First();

            string address = Path.GetFileNameWithoutExtension(
                asset.Replace(DATA_ROOT, "")
                    .Replace(group + "/", "")
                    .Replace('/', '_'));

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
            settings.GetAllAssets(entries, true);
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
        /// 空グループを削除
        /// </summary>
        [MenuItem(MENU_NAME + "/Remove/EmptyGroup")]
        public static void DeleteEmptyGroup()
        {
            var s = GetSettings();
            var groups = s.groups;
            foreach (var g in groups
                .Where(g => g.entries.Count == 0 && !g.IsDefaultGroup()))
            {
                s.RemoveGroup(g);
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
        [MenuItem(MENU_NAME + "/CheckDuplicateAddress")]
        public static void CheckAllAddress()
        {
            AddressableAssetSettings assetSettings = GetSettings();

            List<AddressableAssetEntry> entries = new List<AddressableAssetEntry>();
            assetSettings.GetAllAssets(entries, true);
            List<string> checkedAddress = new List<string>();

            foreach (var e in from e in entries
                where !checkedAddress.Contains(e.address)
                let ret = CheckAddress(entries, e.address)
                where !ret
                select e)
            {
                checkedAddress.Add(e.address);
            }
        }

        /// <summary>
        /// アドレサブルをビルド
        /// </summary>
        [MenuItem(MENU_NAME + "/Build")]
        public static void Build()
        {
            Debug.Log("Build Start...");
            AddressableAssetSettings.BuildPlayerContent();
            Debug.Log("Build Complete");
        }

        /// <summary>
        /// ビルドをクリーン
        /// </summary>
        [MenuItem(MENU_NAME + "/Clean")]
        public static void Clean()
        {
            if (Directory.Exists(SERVER_DATA_DIR))
            {
                Directory.Delete(SERVER_DATA_DIR, true);
                Debug.Log("Delete " + SERVER_DATA_DIR);
            }
            else
            {
                Debug.Log("Not Found " + SERVER_DATA_DIR);
            }

            AddressableAssetSettings.CleanPlayerContent();
        }

        private static bool CheckAddress(List<AddressableAssetEntry> entries, string address)
        {
            List<AddressableAssetEntry> duplicateEntries = entries.FindAll(e => e.address == address);
            if (duplicateEntries.Count <= 1)
            {
                return true;
            }

            string str = "Address=" + address + Environment.NewLine;
            foreach (AddressableAssetEntry e in duplicateEntries)
            {
                string assetName = Path.GetFileName(e.AssetPath);
                str += "Group=" + e.parentGroup.Name + "," + "AssetName=" + assetName + Environment.NewLine;
            }

            Debug.LogAssertion("DuplicateAddress" + Environment.NewLine + str);
            return false;
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
            BundledAssetGroupSchema assetGroupSchema = CreateInstance<BundledAssetGroupSchema>();

            if (groupName.IndexOf(REMOTE_GROUP, StringComparison.OrdinalIgnoreCase) >= 0)
            {
                assetGroupSchema.BuildPath.SetVariableByName(assetSettings, AddressableAssetSettings.kRemoteBuildPath);
                assetGroupSchema.LoadPath.SetVariableByName(assetSettings, AddressableAssetSettings.kRemoteLoadPath);
            }

            //スキーマ生成
            List<AddressableAssetGroupSchema> schema = new List<AddressableAssetGroupSchema>()
            {
                CreateInstance<ContentUpdateGroupSchema>(),
                assetGroupSchema,
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
            assetSettings.GetAllAssets(assetList, true);
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