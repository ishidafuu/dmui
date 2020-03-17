// using MasterData;

using MasterData;
using MessagePack;
using MessagePack.Resolvers;
using MessagePack.Unity;
using UnityEngine;

namespace DM
{
    public static class Initializer
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void SetupMessagePackResolver()
        {
            StaticCompositeResolver.Instance.Register(new[]{
                MasterMemoryResolver.Instance, // set MasterMemory generated resolver
                GeneratedResolver.Instance,    // set MessagePack generated resolver
                StandardResolver.Instance      // set default MessagePack resolver
            });

            var option = MessagePackSerializerOptions.Standard.WithResolver (StaticCompositeResolver.Instance);
            MessagePackSerializer.DefaultOptions = option;
        }
    }
}