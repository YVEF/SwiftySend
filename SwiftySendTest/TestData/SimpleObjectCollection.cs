using System;
using YAXLib;
namespace SwiftySendTest.TestData
{
    internal class Dummy2
    {
        public string StringProperty { get; set; }
    }

    internal class Dummy1
    {
        public string StringField;
    }

    internal class Dummy3
    {
        public string StringProperty { get; set; }
        public string StringField;
    }

    internal enum DummyEnum
    {
        Yes = 1 << 0,
        No = 1 << 1
    }

    internal class Dummy4
    {
        public DummyEnum EnumProperty { get; set; }
        public int IntField;
    }

    [YAXSerializableType(FieldsToSerialize = YAXSerializationFields.AllFields)]
    internal class Dummy5
    {
        public DummyEnum EnumProperty { get; set; }
        public int IntField;
        public string StringField;
        public decimal DecimalProperty { get; set; }
        public float FloatField;
        public short ShortProperty { get; set; }
        public object ObjectField;
        public byte ByteProperty { get; set; }
        public long LongField { get; set; }
        public char CharProperty { get; set; }
        public bool BoolField;
    }

    internal class Dummy6
    {
        public DateTime DateTimeProperty { get; set; }
        public DateTime DateTimeField;
    }

    internal class Dummy7
    {
        public Dummy7() { }

        public Dummy7(int @int, DateTime dateTime)
        {
            IntPrivateProperty = @int;
            DateTimePrivateField = dateTime;
        }
        private int IntPrivateProperty { get; set; }
        private DateTime DateTimePrivateField;
    }



}
