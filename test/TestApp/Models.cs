using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Hagar;
using Hagar.Invocation;
using TestApp;

namespace MyPocos
{
    [Serializable]
    public class ClassWithoutGenerateSerializers
    {
        public int MyInt { get; set; }
        public int MyInt2 { get; set; }
    }

    public abstract class MyProxyBaseClass
    {
        public List<IInvokable> Invocations { get; } = new List<IInvokable>();

        // The only required method is Invoke and it must have this signature.
        protected ValueTask Invoke<TInvokable>(TInvokable invokable) where TInvokable : IInvokable
        {
            this.Invocations.Add(invokable);
            return default;
        }
    }

    public interface IMyInvokable : IGrain
    {
        ValueTask<int> Multiply(int a, int b, object c);
    }

    public interface IMyInvokable<T> : IGrain
    {
        Task DoStuff<TU>();
    }

    public interface IMyExtension : IGrainExtension
    {
        ValueTask Add(int a);
    }

    [GenerateSerializer]
    public class SomeClassWithSerialzers
    {
        [Id(0)]
        public int IntProperty { get; set; }

        [Id(1)] public int IntField;

        public int UnmarkedField;

        public int UnmarkedProperty { get; set; }

        public override string ToString()
        {
            return $"{nameof(this.IntField)}: {this.IntField}, {nameof(this.IntProperty)}: {this.IntProperty}";
        }
    }

    [GenerateSerializer]
    public class ImmutableClass
    {
        public ImmutableClass(int intProperty, int intField, int unmarkedField, int unmarkedProperty)
        {
            this.IntProperty = intProperty;
            this.IntField = intField;
            this.UnmarkedField = unmarkedField;
            this.UnmarkedProperty = unmarkedProperty;
        }

        [Id(0)]
        public int IntProperty { get; }

        [Id(1)] private readonly int IntField;

        public readonly int UnmarkedField;

        public int UnmarkedProperty { get; }

        public override string ToString()
        {
            return $"{nameof(this.IntField)}: {this.IntField}, {nameof(this.IntProperty)}: {this.IntProperty}";
        }
    }

    [GenerateSerializer]
    public struct ImmutableStruct
    {
        public ImmutableStruct(int intProperty, int intField)
        {
            this.IntProperty = intProperty;
            this.IntField = intField;
        }

        [Id(0)]
        public int IntProperty { get; }

        [Id(1)] public readonly int IntField;

        public override string ToString()
        {
            return $"{nameof(this.IntField)}: {this.IntField}, {nameof(this.IntProperty)}: {this.IntProperty}";
        }
    }

    [GenerateSerializer]
    public class SerializableClassWithCompiledBase : List<int>
    {
        [Id(0)]
        public int IntProperty { get; set; }
    }

    [GenerateSerializer]
    public class GenericPoco<T>
    {
        [Id(0)]
        public T Field { get; set; }

        [Id(1030)]
        public T[] ArrayField { get; set; }

        [Id(2222)]
        public Dictionary<T, T> DictField { get; set; }
    }

    [GenerateSerializer]
    public class GenericPocoWithConstraint<TClass, TStruct>
        : GenericPoco<TStruct> where TClass : List<int>, new() where TStruct : struct
    {
        [Id(0)]
        public new TClass Field { get; set; }

        [Id(999)]
        public TStruct ValueField { get; set; }
    }
}
