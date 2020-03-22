// <auto-generated>
// THIS (.cs) FILE IS GENERATED BY MPC(MessagePack-CSharp). DO NOT CHANGE IT.
// </auto-generated>

#pragma warning disable 618
#pragma warning disable 612
#pragma warning disable 414
#pragma warning disable 168

#pragma warning disable SA1129 // Do not use default value type constructor
#pragma warning disable SA1200 // Using directives should be placed correctly
#pragma warning disable SA1309 // Field names should not begin with underscore
#pragma warning disable SA1312 // Variable names should begin with lower-case letter
#pragma warning disable SA1403 // File may only contain a single namespace
#pragma warning disable SA1649 // File name should match first type name

namespace MessagePack.Formatters.DM
{
    using System;
    using System.Buffers;
    using MessagePack;

    public sealed class SkillFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::DM.Skill>
    {


        private readonly global::MessagePack.Internal.AutomataDictionary ____keyMapping;
        private readonly byte[][] ____stringByteKeys;

        public SkillFormatter()
        {
            this.____keyMapping = new global::MessagePack.Internal.AutomataDictionary()
            {
                { "SkillID", 0 },
                { "SkillName", 1 },
            };

            this.____stringByteKeys = new byte[][]
            {
                global::MessagePack.Internal.CodeGenHelpers.GetEncodedStringBytes("SkillID"),
                global::MessagePack.Internal.CodeGenHelpers.GetEncodedStringBytes("SkillName"),
            };
        }

        public void Serialize(ref MessagePackWriter writer, global::DM.Skill value, global::MessagePack.MessagePackSerializerOptions options)
        {
            if (value == null)
            {
                writer.WriteNil();
                return;
            }

            IFormatterResolver formatterResolver = options.Resolver;
            writer.WriteMapHeader(2);
            writer.WriteRaw(this.____stringByteKeys[0]);
            writer.Write(value.SkillID);
            writer.WriteRaw(this.____stringByteKeys[1]);
            formatterResolver.GetFormatterWithVerify<string>().Serialize(ref writer, value.SkillName, options);
        }

        public global::DM.Skill Deserialize(ref MessagePackReader reader, global::MessagePack.MessagePackSerializerOptions options)
        {
            if (reader.TryReadNil())
            {
                return null;
            }

            options.Security.DepthStep(ref reader);
            IFormatterResolver formatterResolver = options.Resolver;
            var length = reader.ReadMapHeader();
            var __SkillID__ = default(int);
            var __SkillName__ = default(string);

            for (int i = 0; i < length; i++)
            {
                ReadOnlySpan<byte> stringKey = global::MessagePack.Internal.CodeGenHelpers.ReadStringSpan(ref reader);
                int key;
                if (!this.____keyMapping.TryGetValue(stringKey, out key))
                {
                    reader.Skip();
                    continue;
                }

                switch (key)
                {
                    case 0:
                        __SkillID__ = reader.ReadInt32();
                        break;
                    case 1:
                        __SkillName__ = formatterResolver.GetFormatterWithVerify<string>().Deserialize(ref reader, options);
                        break;
                    default:
                        reader.Skip();
                        break;
                }
            }

            var ____result = new global::DM.Skill();
            ____result.SkillID = __SkillID__;
            ____result.SkillName = __SkillName__;
            reader.Depth--;
            return ____result;
        }
    }
}

#pragma warning restore 168
#pragma warning restore 414
#pragma warning restore 618
#pragma warning restore 612

#pragma warning restore SA1129 // Do not use default value type constructor
#pragma warning restore SA1200 // Using directives should be placed correctly
#pragma warning restore SA1309 // Field names should not begin with underscore
#pragma warning restore SA1312 // Variable names should begin with lower-case letter
#pragma warning restore SA1403 // File may only contain a single namespace
#pragma warning restore SA1649 // File name should match first type name