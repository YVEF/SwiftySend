using AutoFixture;
using SwiftySend;
using SwiftySendTest.TestData;
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
    }
}
