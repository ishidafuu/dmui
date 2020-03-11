using System.Collections;
using UnityEngine;

namespace DM
{
    public interface IPrefabLoader
    {
        IEnumerator Load(string path, PrefabReceiver receiver);
        void Release(string path, Object prefab);
    }
}