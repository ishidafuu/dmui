﻿using System;
using System.Diagnostics;
using UnityEditor;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace DM
{
    public class MasterMemoryCodeGenerator : MonoBehaviour
    {
        [MenuItem("CodeGenerate/MasterMemory")]
        private static void Generate()
        {
            ExecuteMasterMemoryCodeGenerator();
            ExecuteMessagePackCodeGenerator();
        }

        private static void ExecuteMasterMemoryCodeGenerator()
        {
            Debug.Log($"{nameof(ExecuteMasterMemoryCodeGenerator)} : start");

            var exProcess = new Process();

            var rootPath = Application.dataPath + "/..";
            var filePath = rootPath + "/GeneratorTools/MasterMemory.Generator";
            var exeFileName = "";
#if UNITY_EDITOR_WIN
            exeFileName = "/win-x64/MasterMemory.Generator.exe";
#elif UNITY_EDITOR_OSX
        exeFileName = "/osx-x64/MasterMemory.Generator";
#elif UNITY_EDITOR_LINUX
        exeFileName = "/linux-x64/MasterMemory.Generator";
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
                    $@"-i ""{Application.dataPath}/Scripts/Tables"" -o ""{Application.dataPath}/Scripts/Generated"" -n ""MasterData""",
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
            var filePath = rootPath + "/GeneratorTools/MessagePackUniversalCodeGenerator";
            var exeFileName = "";
#if UNITY_EDITOR_WIN
            exeFileName = "/win-x64/mpc.exe";
#elif UNITY_EDITOR_OSX
        exeFileName = "/osx-x64/mpc";
#elif UNITY_EDITOR_LINUX
        exeFileName = "/linux-x64/mpc";
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
                    $@"-i ""{Application.dataPath}/../Assembly-CSharp.csproj"" -o ""{Application.dataPath}/Scripts/Generated/MessagePack.Generated.cs""",
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