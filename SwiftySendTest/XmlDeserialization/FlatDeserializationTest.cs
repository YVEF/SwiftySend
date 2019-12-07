using AutoFixture;
using SwiftySend;
using SwiftySendTest.TestData;
using System;
using Xunit;

namespace SwiftySendTest.XmlDeserialization
{
    public class FlatDeserializationTest : DeserializationTestBase
    {
        

        [Fact]
        public void Deserialize_Object_With_One_String_Field()
        {
            var dummy = _fixture.Create<Dummy1>();
            Chech(() => XmlRepresentation.GetXml(dummy), dummy);
        }


        [Fact]
        public void Deserialize_Object_With_One_String_Property()
        {
            var dummy = _fixture.Create<Dummy2>();
            Chech(() => XmlRepresentation.GetXml(dummy), dummy);
        }


        [Fact]
        public void Deserialize_Object_With_One_String_Property_And_One_String_Field()
        {
            var dummy = _fixture.Create<Dummy3>();
            Chech(() => XmlRepresentation.GetXml(dummy), dummy);
        }

        [Fact]
        public void Deserialize_Object_With_One_Int_Field_And_One_Enum_Property()
        {
            var dummy = _fixture.Create<Dummy4>();
            Chech(() => XmlRepresentation.GetXml(dummy), dummy);
        }

        [Fact]
        public void Deserialize_Object_With_All_Of_The_Base_Types()
        {
            var dummy = _fixture
                .Build<Dummy5>()
                    .With(x => x.ObjectField, "randome string")
                .Create();
            Chech(() => XmlRepresentation.GetXml(dummy), dummy);
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
    }
}
