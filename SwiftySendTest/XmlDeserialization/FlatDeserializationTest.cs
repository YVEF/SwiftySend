using AutoFixture;
using SwiftySend;
using SwiftySendTest.TestData;
using System;
using System.Reflection;
using Xunit;

namespace SwiftySendTest.XmlDeserialization
{
    public class FlatDeserializationTest : DeserializationTestBase
    {
        

        [Fact]
        public void Deserialize_Object_With_One_String_Field()
        {
            var dummy = _fixture.Create<Dummy1>();
            Check(() => XmlRepresentation.GetXml(dummy), dummy);
        }


        [Fact]
        public void Deserialize_Object_With_One_String_Property()
        {
            var dummy = _fixture.Create<Dummy2>();
            Check(() => XmlRepresentation.GetXml(dummy), dummy);
        }


        [Fact]
        public void Deserialize_Object_With_One_String_Property_And_One_String_Field()
        {
            var dummy = _fixture.Create<Dummy3>();
            Check(() => XmlRepresentation.GetXml(dummy), dummy);
        }

        [Fact]
        public void Deserialize_Object_With_One_Int_Field_And_One_Enum_Property()
        {
            var dummy = _fixture.Create<Dummy4>();
            Check(() => XmlRepresentation.GetXml(dummy), dummy);
        }

        [Fact]
        public void Deserialize_Object_With_All_Of_The_Base_Types()
        {
            var dummy = _fixture
                .Build<Dummy5>()
                    .With(x => x.ObjectField, "randome string")
                .Create();
            var ser = new SwiftySendSerializer(typeof(Dummy5));
            var xml = ser.Serialize(dummy);
            var rxml = XmlRepresentation.GetXml(dummy);
            var newdummy = ser.Deserialize<Dummy5>(xml);
            var newnewdummy = ser.Deserialize<Dummy5>(rxml);

            Check(() => XmlRepresentation.GetXml(dummy), dummy);
        }

        [Fact]
        public void Test1()
        {
            var dd = _fixture.Build<DD>()
                //.With(x => x.ObjectFiled, "random string1")
                .With(x => x.ObjectProperty, "random string2")
                .Create();
            var ser = new SwiftySendSerializer(typeof(DD));
            string xml = ser.Serialize(dd);
            var newdd = ser.Deserialize<DD>(xml);
        }

        internal class DD
        {
            //public string StringProperty { get; set; }
            //public string StringField;
            public object ObjectProperty { get; set; }
            //public object ObjectFiled;
            //public int IntField;
            public int IntProperty { get; set; }
            //public decimal DecimalField;
            //public decimal DecimalProperty { get; set; }
            //public char CharField;
            //public char CharProperty { get; set; }
            //public long LongProperty { get; set; }
            //public byte ByteProperty { get; set; }
            //public byte ByteField;
            //public long LongField;
            //public DummyEnum DummyEnumProperty { get; set; }
            //public DummyEnum DummyEnumField;
            //public float FloatProperty { get; set; }
            //public bool BoolProperty { get; set; }
            //public short ShortProperty { get; set; }
        }


        [Fact]
        public void Deserialize_Object_With_Datetime()
        {
            var dummy = _fixture.Create<Dummy6>();
            var serializer = new SwiftySendSerializer(typeof(Dummy6));
            var result = serializer.Deserialize<Dummy6>(XmlRepresentation.GetXml(dummy));

            Assert.Equal(result.DateTimeProperty.ToString(), dummy.DateTimeProperty.ToString());
            Assert.Equal(result.DateTimeField.ToString(), dummy.DateTimeField.ToString());
        }

        [Fact]
        public void Deserialize_Object_With_Private_Field_And_Property()
        {
            var dummy = new Dummy7(_fixture.Create<int>(), _fixture.Create<DateTime>());

            var serializer = new SwiftySendSerializer(typeof(Dummy7));
            var result = serializer.Deserialize<Dummy7>(XmlRepresentation.GetXml(dummy));

            var propertyValue = typeof(Dummy7).GetProperty("IntPrivateProperty", BindingFlags.Instance | BindingFlags.NonPublic);
            var fieldValue = typeof(Dummy7).GetField("DateTimePrivateField", BindingFlags.Instance | BindingFlags.NonPublic);

            Assert.Equal(propertyValue.GetValue(result).ToString(), propertyValue.GetValue(dummy).ToString());
            Assert.Equal(fieldValue.GetValue(result).ToString(), fieldValue.GetValue(dummy).ToString());
        }
    }
}
