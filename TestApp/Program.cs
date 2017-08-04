﻿using System;
using Hagar.Codec;
using Hagar.Session;
using Hagar.Utilities;
using Hagar.Utilities.Orleans.Serialization;
using Hagar.WireProtocol;

namespace TestApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var context = new SerializationContext();
            var writer = new Writer();

            var expected = new SubType
            {
                BaseTypeString = "base",
                String = "sub",
                Int = 2,
            };

            var stringCodec = new StringCodec();
            var intCodec = new IntegerCodec();
            var baseTypeSerializer = new BaseTypeSerializer(stringCodec);
            var serializer = new SubTypeSerializer(baseTypeSerializer, stringCodec, intCodec);

            writer.WriteStartObject(context, 0, typeof(SubType), typeof(SubType));
            serializer.SerializeMyType(writer, context, expected);
            writer.WriteEndObject();

            Console.WriteLine($"Size: {writer.CurrentOffset} bytes");
            var reader = new Reader(writer.ToBytes());
            var deserializationContext = new SerializationContext();
            var initialHeader = reader.ReadFieldHeader(context);
            Console.WriteLine(initialHeader);
            var actual = new SubType();
            serializer.DeserializeMyType(reader, deserializationContext, actual);

            Console.WriteLine($"Expect: {expected}\nActual: {actual}");
        }


        public class BaseTypeSerializer
        {
            private readonly IValueCodec<string> stringCodec;

            public BaseTypeSerializer(IValueCodec<string> stringCodec)
            {
                this.stringCodec = stringCodec;
            }

            public void SerializeBaseType(Writer writer, SerializationContext context, BaseType obj)
            {
                stringCodec.WriteField(writer, context, 0, typeof(string), obj.BaseTypeString);
            }

            public void DeserializeBaseType(Reader reader, SerializationContext context, BaseType obj)
            {
                while (true)
                {
                    var header = reader.ReadFieldHeader(context);
                    Console.WriteLine(header);
                    if (header.IsEndBaseOrEndObject) break;
                    switch (header.FieldId)
                    {
                        case 0:
                        {
                            var type = header.FieldType ?? typeof(string);
                            obj.BaseTypeString = stringCodec.ReadValue(reader, context, header);
                            Console.WriteLine(
                                $"\tReading field {header.FieldId} with type = {type?.ToString() ?? "UNKNOWN"} and wireType = {header.WireType}");
                            break;
                        }
                        default:
                        {
                            var type = header.FieldType;
                            Console.WriteLine(
                                $"\tReading UNKNOWN field {header.FieldId} with type = {type?.ToString() ?? "UNKNOWN"} and wireType = {header.WireType}");
                            break;
                        }
                    }
                }
            }
        }

        public class SubTypeSerializer
        {
            private readonly BaseTypeSerializer baseTypeSerializer;
            private readonly IValueCodec<string> stringCodec;
            private readonly IValueCodec<int> intCodec;

            public SubTypeSerializer(BaseTypeSerializer baseTypeSerializer, IValueCodec<string> stringCodec, IValueCodec<int> intCodec)
            {
                this.baseTypeSerializer = baseTypeSerializer;
                this.stringCodec = stringCodec;
                this.intCodec = intCodec;
            }

            public void SerializeMyType(Writer writer, SerializationContext context, SubType obj)
            {
                this.baseTypeSerializer.SerializeBaseType(writer, context, obj);
                writer.WriteEndBase(); // the base object is complete.
                this.stringCodec.WriteField(writer, context, 0, typeof(string), obj.String);
                this.intCodec.WriteField(writer, context, 1, typeof(int), obj.Int);
                writer.WriteFieldHeader(context, 1025, typeof(Guid), Guid.Empty.GetType(), WireType.Fixed128);
                writer.WriteFieldHeader(context, 1020, typeof(object), typeof(Program), WireType.Reference);
            }
            public void DeserializeMyType(Reader reader, SerializationContext context, SubType obj)
            {
                this.baseTypeSerializer.DeserializeBaseType(reader, context, obj);
                while (true)
                {
                    var header = reader.ReadFieldHeader(context);
                    Console.WriteLine(header);
                    if (header.IsEndBaseOrEndObject) break;
                    Type type;
                    switch (header.FieldId)
                    {
                        case 0:
                            type = header.FieldType ?? typeof(string);
                            obj.String = this.stringCodec.ReadValue(reader, context, header);
                            Console.WriteLine(
                                $"\tReading field {header.FieldId} with type = {type?.ToString() ?? "UNKNOWN"} and wireType = {header.WireType}");
                            break;
                        case 1:
                            obj.Int = this.intCodec.ReadValue(reader, context, header);
                            type = header.FieldType ?? typeof(long);
                            Console.WriteLine(
                                $"\tReading field {header.FieldId} with type = {type?.ToString() ?? "UNKNOWN"} and wireType = {header.WireType}");
                            break;
                        default:
                            type = header.FieldType;
                            Console.WriteLine(
                                $"\tReading UNKNOWN field {header.FieldId} with type = {type?.ToString() ?? "UNKNOWN"} and wireType = {header.WireType}");
                            break;
                    }
                }
            }
        }
    }

    public class BaseType
    {
        public string BaseTypeString { get; set; }

        public override string ToString()
        {
            return $"{nameof(BaseTypeString)}: {BaseTypeString}";
        }
    }

    public class SubType : BaseType
    {
        // 0
        public string String { get; set; }

        // 1
        public int Int { get; set; }

        public override string ToString()
        {
            return $"{base.ToString()}, {nameof(String)}: {String}, {nameof(Int)}: {Int}";
        }
    }
}