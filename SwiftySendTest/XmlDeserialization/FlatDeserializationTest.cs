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
            Check(() => XmlRepresentation.GetXml(dummy), dummy);
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
