using UniRx.Async;
using UnityEngine;

namespace DM
{
    public interface IPrefabLoader
    {
        UniTask Load(string path, PrefabReceiver receiver);
        void Release(string path, Object prefab);
    }
}