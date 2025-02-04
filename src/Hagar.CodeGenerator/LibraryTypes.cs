using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
// ReSharper disable InconsistentNaming

namespace Hagar.CodeGenerator
{
    internal class LibraryTypes
    {
        private readonly Compilation compilation;

        public static LibraryTypes FromCompilation(Compilation compilation, CodeGeneratorOptions options)
        {
            return new LibraryTypes(compilation)
            {
                Action_2 = Type("System.Action`2"),
                Byte = compilation.GetSpecialType(SpecialType.System_Byte),
                PartialSerializer = Type("Hagar.Serializers.IPartialSerializer`1"),
                ValueSerializer = Type("Hagar.Serializers.IValueSerializer`1"),
                FieldCodec = Type("Hagar.Codecs.IFieldCodec`1"),
                TypedCodecProvider = Type("Hagar.Serializers.ITypedCodecProvider"),
                Writer = Type("Hagar.Buffers.Writer`1"),
                IBufferWriter = Type("System.Buffers.IBufferWriter`1"),
                Reader = Type("Hagar.Buffers.Reader"),
                SerializerSession = Type("Hagar.Session.SerializerSession"),
                Invokable = Type("Hagar.Invocation.Invokable"),
                Object = compilation.GetSpecialType(SpecialType.System_Object),
                Type = Type("System.Type"),
                SerializerConfiguration = Type("Hagar.Configuration.SerializerConfiguration"),
                ConfigurationProvider = Type("Hagar.Configuration.IConfigurationProvider`1"),
                GenerateMethodSerializersAttribute = Type("Hagar.GenerateMethodSerializersAttribute"),
                GenerateSerializerAttribute = Type("Hagar.GenerateSerializerAttribute"),
                MetadataProviderAttribute = Type("Hagar.Configuration.MetadataProviderAttribute"),
                IdAttributeTypes = options.IdAttributeTypes.Select(Type).ToList(),
                IInvokable = Type("Hagar.Invocation.IInvokable"),
                ITargetHolder = Type("Hagar.Invocation.ITargetHolder"),
                ValueTask = Type("System.Threading.Tasks.ValueTask"),
                Int32 = compilation.GetSpecialType(SpecialType.System_Int32),
                NonSerializedAttribute = Type("System.NonSerializedAttribute"),
                InvalidOperationException = Type("System.InvalidOperationException"),
                Void = compilation.GetSpecialType(SpecialType.System_Void),
                ObsoleteAttribute = Type("System.ObsoleteAttribute"),
                Func_2 = Type("System.Func`2"),
                ValueTypeSetter_2 = Type("Hagar.Utilities.ValueTypeSetter`2"),
                StaticCodecs = new List<StaticCodecDescription>
                {
                    new StaticCodecDescription(compilation.GetSpecialType(SpecialType.System_Boolean), Type("Hagar.Codecs.BoolCodec")),
                    new StaticCodecDescription(compilation.GetSpecialType(SpecialType.System_Char), Type("Hagar.Codecs.CharCodec")),
                    new StaticCodecDescription(compilation.GetSpecialType(SpecialType.System_Byte), Type("Hagar.Codecs.ByteCodec")),
                    new StaticCodecDescription(compilation.GetSpecialType(SpecialType.System_SByte), Type("Hagar.Codecs.SByteCodec")),
                    new StaticCodecDescription(compilation.GetSpecialType(SpecialType.System_Int16), Type("Hagar.Codecs.Int16Codec")),
                    new StaticCodecDescription(compilation.GetSpecialType(SpecialType.System_Int32), Type("Hagar.Codecs.Int32Codec")),
                    new StaticCodecDescription(compilation.GetSpecialType(SpecialType.System_Int64), Type("Hagar.Codecs.Int64Codec")),
                    new StaticCodecDescription(compilation.GetSpecialType(SpecialType.System_UInt16), Type("Hagar.Codecs.UInt16Codec")),
                    new StaticCodecDescription(compilation.GetSpecialType(SpecialType.System_UInt32), Type("Hagar.Codecs.UInt32Codec")),
                    new StaticCodecDescription(compilation.GetSpecialType(SpecialType.System_UInt64), Type("Hagar.Codecs.UInt64Codec")),
                    new StaticCodecDescription(compilation.GetSpecialType(SpecialType.System_String), Type("Hagar.Codecs.StringCodec")),
                    new StaticCodecDescription(compilation.GetSpecialType(SpecialType.System_Object), Type("Hagar.Codecs.ObjectCodec")),
                    new StaticCodecDescription(compilation.CreateArrayTypeSymbol(compilation.GetSpecialType(SpecialType.System_Byte), 1), Type("Hagar.Codecs.ByteArrayCodec")),
                    new StaticCodecDescription(compilation.GetSpecialType(SpecialType.System_Single), Type("Hagar.Codecs.FloatCodec")),
                    new StaticCodecDescription(compilation.GetSpecialType(SpecialType.System_Double), Type("Hagar.Codecs.DoubleCodec")),
                    new StaticCodecDescription(compilation.GetSpecialType(SpecialType.System_Decimal), Type("Hagar.Codecs.DecimalCodec")),
                    new StaticCodecDescription(compilation.GetSpecialType(SpecialType.System_DateTime), Type("Hagar.Codecs.DateTimeCodec")),
                    new StaticCodecDescription(Type("System.TimeSpan"), Type("Hagar.Codecs.TimeSpanCodec")),
                    new StaticCodecDescription(Type("System.DateTimeOffset"), Type("Hagar.Codecs.DateTimeOffsetCodec")),
                    new StaticCodecDescription(Type("System.Guid"), Type("Hagar.Codecs.GuidCodec")),
                    new StaticCodecDescription(Type("System.Type"), Type("Hagar.Codecs.TypeSerializerCodec")),
                }
            };

            INamedTypeSymbol Type(string metadataName)
            {
                var result = compilation.GetTypeByMetadataName(metadataName);
                if (result == null) throw new InvalidOperationException("Cannot find type with metadata name " + metadataName);
                return result;
            }
        }

        public INamedTypeSymbol InvalidOperationException { get; private set; }

        public INamedTypeSymbol ITargetHolder { get; private set; }
        public INamedTypeSymbol ObsoleteAttribute { get; private set; }
        public INamedTypeSymbol Action_2 { get; private set; }
        public INamedTypeSymbol Func_2 { get; private set; }
        public INamedTypeSymbol ValueTypeSetter_2 { get; private set; }

        public INamedTypeSymbol NonSerializedAttribute { get; private set; }

        public INamedTypeSymbol Int32 { get; private set; }

        private LibraryTypes(Compilation compilation) => this.compilation = compilation;

        public INamedTypeSymbol MetadataProviderAttribute { get; private set; }

        public List<INamedTypeSymbol> IdAttributeTypes { get; private set; }

        public INamedTypeSymbol GenerateSerializerAttribute { get; private set; }

        public List<StaticCodecDescription> StaticCodecs { get; private set; }

        public INamedTypeSymbol TypedCodecProvider { get; private set; }

        public INamedTypeSymbol ConfigurationProvider { get; private set; }

        public INamedTypeSymbol SerializerConfiguration { get; private set; }

        public INamedTypeSymbol Type { get; private set; }

        public INamedTypeSymbol Object { get; private set; }

        public INamedTypeSymbol SerializerSession { get; private set; }

        public INamedTypeSymbol Reader { get; private set; }

        public INamedTypeSymbol Writer { get; private set; }

        public INamedTypeSymbol IBufferWriter { get; private set; }

        public INamedTypeSymbol FieldCodec { get; private set; }

        public INamedTypeSymbol PartialSerializer { get; private set; }

        public INamedTypeSymbol Invokable { get; private set; }

        public INamedTypeSymbol ValueSerializer { get; private set; }

        public INamedTypeSymbol Byte { get; private set; }

        public INamedTypeSymbol GenerateMethodSerializersAttribute { get; private set; }
        public INamedTypeSymbol IInvokable { get; private set; }
        public INamedTypeSymbol ValueTask { get; private set; }
        public INamedTypeSymbol Void { get; private set; }
    }
}