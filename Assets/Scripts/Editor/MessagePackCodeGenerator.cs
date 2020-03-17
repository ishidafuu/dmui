using System;
using System.Diagnostics;
using UnityEditor;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace DM
{
    public class MessagePackCodeGenerator : EditorWindow
    {
        //  生成コードの出力先。Assetsフォルダ以降のパスを指定
        private static string outputPath = "Scripts/Generated/";

        [MenuItem("CodeGenerate/MessagePack")]
        private static void Open()
        {
            var window = GetWindow<MessagePackCodeGenerator>();
            window.titleContent = new GUIContent()
            {
                text = "コード生成"
            };
        }

        void OnGUI()
        {
            EditorGUILayout.LabelField("出力先。Assetsフォルダ以下のパス。");
            EditorGUILayout.LabelField("最後に/を忘れずに");
            outputPath = EditorGUILayout.TextField(outputPath);
            EditorGUILayout.Space(10);
            if (GUILayout.Button("生成"))
            {
                ExecuteMessagePackCodeGenerator();
                AssetDatabase.Refresh();
            }
        }

        private static void ExecuteMessagePackCodeGenerator()
        {
            Debug.Log($"{nameof(ExecuteMessagePackCodeGenerator)} : start");

            var exProcess = new Process();

            var rootPath = Application.dataPath + "/..";
            var fileName = rootPath + "/GeneratorTools";
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
                FileName = fileName + exeFileName,
                Arguments =
                    $@"-i ""{Application.dataPath}/../Assembly-CSharp.csproj"" -o ""{Application.dataPath}/{outputPath}",
            };

            var p = Process.Start(psi);

            p.EnableRaisingEvents = true;
            p.Exited += (object sender, EventArgs e) =>
            {
                var data = p.StandardOutput.ReadToEnd();
                Debug.Log($"{data}");
                Debug.Log($"{nameof(ExecuteMessagePackCodeGenerator)} : end");
                p.Dispose();
                p = null;
            };
        }
    }
}