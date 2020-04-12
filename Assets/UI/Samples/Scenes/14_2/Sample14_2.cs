using System.Collections.Generic;
using System.IO;
using DMUIFramework.Samples;
using UnityEngine;
using UnityEngine.UI;
using Firebase;
using Firebase.Functions;
using Firebase.Unity.Editor;
using UniRx.Async;

namespace DM
{
    public class Sample14_2 : MonoBehaviour
    {
        void Start()
        {
            UIController.SetImplement(new PrefabLoader(), new Sounder(), new FadeCreator(), new LoadingCreator(), new ToastCreator());
            UIController.Instance.AddFront(new Sample14_2Scene());
            helloWorldOnCall();
        }
        
        private async UniTask<string> helloWorldOnCall() {
            // Create the arguments to the callable function.
            var data = new Dictionary<string, object>();
            data["name"] = "isdf";
            data["asdf"] = "asdf";
            
            // FirebaseFunctions.DefaultInstance.UseFunctionsEmulator("http://localhost:5000");
            HttpsCallableReference function = FirebaseFunctions.DefaultInstance.GetHttpsCallable("helloWorldOnCall");
            HttpsCallableResult result = await function.CallAsync(data);

            Debug.Log(result.Data);
            var resDict = result.Data as Dictionary<object, object>;
            foreach (var key in resDict.Keys)
            {
                Debug.Log(key.ToString());
                Debug.Log(resDict[key.ToString()].ToString());
            }
            return "yeah";
        }
    }
}