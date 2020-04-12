using System.Collections.Generic;
using Firebase.Functions;
using UniRx.Async;
using UnityEngine;

namespace DM
{
    public class FirebaseTest : MonoBehaviour
    {
        void Start()
        {
            helloWorldOnCall();
        }
        
        private async UniTask<string> helloWorldOnCall() {
            Debug.Log("helloWorldOnCall Start");
            // Create the arguments to the callable function.
            var data = new Dictionary<string, object>();
            data["name"] = "isdf";
            data["asdf"] = "asdf";
            
            FirebaseFunctions.DefaultInstance.UseFunctionsEmulator("http://localhost:5000");
            HttpsCallableReference function = FirebaseFunctions.DefaultInstance.GetHttpsCallable("helloWorldOnCall");
            HttpsCallableResult result = await function.CallAsync(data);

            Debug.Log(result.Data);
            var resDict = result.Data as Dictionary<object, object>;
            foreach (var key in resDict.Keys)
            {
                Debug.Log(key.ToString());
                Debug.Log(resDict[key.ToString()].ToString());
            }
            
            Debug.Log("helloWorldOnCall End");
            return "yeah";
        }
    }
}