using System;
using System.Diagnostics;
using UnityEditor;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace DM
{
    public class MasterMemoryCodeGenerator : EditorWindow
    {
        private const string SRC_PATH = "Scripts/Master/Tables";

        //  生成コードの出力先。Assetsフォルダ以降のパスを指定
        private static string s_OutputPath = "Scripts/Generated/";

        private static string ArgumentMasterMemory =>
            $@"-i ""{Application.dataPath}/{SRC_PATH}"" -o ""{Application.dataPath}/{s_OutputPath}"" -n ""MasterData""";

        private static string ArgumentMessagePack =>
            $@"-i ""{Application.dataPath}/../Assembly-CSharp.csproj"" -o ""{Application.dataPath}/{s_OutputPath}/MessagePack.Generated.cs""";

        private static readonly string s_GeneratorTools = $"{Application.dataPath}/../GeneratorTools";

        [MenuItem("Tools/MessagePack")]
        private static void Open()
        {
            var window = GetWindow<MasterMemoryCodeGenerator>();
            window.titleContent = new GUIContent()
            {
                text = "MessagePackCodeGenerator"
            };
        }

        private void OnGUI()
        {
            EditorGUILayout.LabelField("出力先。Assetsフォルダ以下のパス。");
            EditorGUILayout.LabelField("最後に/を忘れずに");
            s_OutputPath = EditorGUILayout.TextField(s_OutputPath);
            EditorGUILayout.Space(10);
            if (GUILayout.Button("MasterMemory CodeGenerator"))
            {
                ExecuteMasterMemoryCodeGenerator();
                AssetDatabase.Refresh();
            }

            if (GUILayout.Button("MessagePack CodeGenerator"))
            {
                ExecuteMessagePackCodeGenerator();
                AssetDatabase.Refresh();
            }

            if (GUILayout.Button("Build MasterData"))
            {
                MasterDataBuilder.Build();
                AssetDatabase.Refresh();
            }
        }

        private static void ExecuteMasterMemoryCodeGenerator()
        {
            Debug.Log($"{nameof(ExecuteMasterMemoryCodeGenerator)} : start");

            var exProcess = new Process();

            var exeFileName = "";
#if UNITY_EDITOR_WIN
            exeFileName = "/MasterMemory.Generator.exe";
#elif UNITY_EDITOR_OSX
        exeFileName = "/MasterMemory.Generator";
#elif UNITY_EDITOR_LINUX
        exeFileName = "/MasterMemory.Generator";
#else
        return;
#endif

            var psi = new ProcessStartInfo()
            {
                CreateNoWindow = true,
                WindowStyle = ProcessWindowStyle.Hidden,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                FileName = s_GeneratorTools + exeFileName,
                Arguments = ArgumentMasterMemory,
            };

            Process p = Process.Start(psi);

            p.EnableRaisingEvents = true;
            p.Exited += (object sender, EventArgs e) =>
            {
                string data = p.StandardOutput.ReadToEnd();
                Debug.Log($"{data}");
                Debug.Log($"{nameof(ExecuteMasterMemoryCodeGenerator)} : end");
                p.Dispose();
                p = null;
            };
        }

        private static void ExecuteMessagePackCodeGenerator()
        {
            Debug.Log($"{nameof(ExecuteMessagePackCodeGenerator)} : start");

            var exProcess = new Process();

            var exeFileName = "";
#if UNITY_EDITOR_WIN
            exeFileName = "/mpc.exe";
#elif UNITY_EDITOR_OSX
        exeFileName = "/mpc";
#elif UNITY_EDITOR_LINUX
        exeFileName = "/mpc";
#else
        return;
#endif

            var psi = new ProcessStartInfo()
            {
                CreateNoWindow = true,
                WindowStyle = ProcessWindowStyle.Hidden,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                FileName = s_GeneratorTools + exeFileName,
                Arguments = ArgumentMessagePack,
            };

            Process p = Process.Start(psi);

            p.EnableRaisingEvents = true;
            p.Exited += (object sender, EventArgs e) =>
            {
                string data = p.StandardOutput.ReadToEnd();
                Debug.Log($"{data}");
                Debug.Log($"{nameof(ExecuteMessagePackCodeGenerator)} : end");
                p.Dispose();
                p = null;
            };
        }
    }
}