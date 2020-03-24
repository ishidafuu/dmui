using System;
using System.Diagnostics;
using UnityEditor;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace DM
{
    public class MasterMemoryCodeGenerator : MonoBehaviour
    {
        private static string srcPath = "Scripts/Tables";
        //  生成コードの出力先。Assetsフォルダ以降のパスを指定
        private static string outputPath = "Scripts/Generated/";
        
        [MenuItem("CodeGenerate/MasterMemory")]
        private static void Generate()
        {
            ExecuteMasterMemoryCodeGenerator();
            // ExecuteMessagePackCodeGenerator();
        }

        private static void ExecuteMasterMemoryCodeGenerator()
        {
            Debug.Log($"{nameof(ExecuteMasterMemoryCodeGenerator)} : start");

            var exProcess = new Process();

            var rootPath = Application.dataPath + "/..";
            var filePath = rootPath + "/GeneratorTools";
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
                FileName = filePath + exeFileName,
                Arguments =
                    $@"-i ""{Application.dataPath}/{srcPath}"" -o ""{Application.dataPath}/{outputPath}"" -n ""MasterData""",
            };

            var p = Process.Start(psi);

            p.EnableRaisingEvents = true;
            p.Exited += (object sender, EventArgs e) =>
            {
                var data = p.StandardOutput.ReadToEnd();
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

            var rootPath = Application.dataPath + "/..";
            var filePath = rootPath + "/GeneratorTools";
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
                FileName = filePath + exeFileName,
                Arguments =
                    $@"-i ""{Application.dataPath}/../Assembly-CSharp.csproj"" -o ""{Application.dataPath}/{outputPath}/MessagePack.Generated.cs""",
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