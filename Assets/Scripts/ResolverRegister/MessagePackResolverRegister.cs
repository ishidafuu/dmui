using MessagePack;
using MessagePack.Resolvers;
using MessagePack.Unity;
using UnityEngine;

namespace DM
{
    public class MessagePackResolverRegister : MonoBehaviour
    {
        private void Start()
        {
            StaticCompositeResolver.Instance.Register(
                // GeneratedResolver.Instance,
                UnityResolver.Instance,
                BuiltinResolver.Instance,
                AttributeFormatterResolver.Instance,
                PrimitiveObjectResolver.Instance,
                StandardResolver.Instance
            );
            
            MessagePackSerializerOptions option =
                MessagePackSerializerOptions.Standard.WithResolver(StaticCompositeResolver.Instance);
            MessagePackSerializer.DefaultOptions = option;
        }
    }
}