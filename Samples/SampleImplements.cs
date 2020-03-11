using System.Collections;
using DM;
using UnityEngine;

namespace DMUIFramework.Samples {
    public class PrefabLoader : IPrefabLoader
    {
        public IEnumerator Load(string path, PrefabReceiver receiver)
        {
            ResourceRequest req = Resources.LoadAsync(path);
            yield return req;

            receiver.m_Prefab = req.asset;
        }

        public void Release(string path, Object prefab) { }
    }


    public class Sounder : ISounder
    {
        public void PlayDefaultClickSE()
        {
            Debug.Log("Sounder: DefaultClickSE");
        }

        public void PlayClickSE(string name)
        {
            Debug.Log("Sounder: ClickSE[" + name + "]");
        }

        public void PlayBGM(string name)
        {
            Debug.Log("Sounder: PlayBGM[" + name + "]");
        }

        public void StopBGM()
        {
            Debug.Log("Sounder: StopBGM");
        }
    }


    public class FadeCreator : IFadeCreator
    {
        public UIFade Create()
        {
            return new UIFade("BlackCurtainFade");
        }
    }
}