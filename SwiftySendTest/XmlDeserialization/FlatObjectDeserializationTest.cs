using AutoFixture;
using SwiftySend;
using SwiftySendTest.TestData;
using Xunit;

namespace SwiftySendTest.XmlDeserialization
{
    public class FlatObjectDeserializationTest
    {
        protected Fixture _fixture = new Fixture();

        [Fact]
        public void Deserialize_Object_With_One_String_Field()
        {
            var dummy = _fixture.Create<Dummy1>();
            var serializer = new SwiftySendSerializer(typeof(Dummy1));

            var xmlInput = XmlRepresentation.GetXml(dummy);
            var result = serializer.Deserialize<Dummy1>(xmlInput);

            Assert.Equal(dummy.StringField, result.StringField);
        }


        [Fact]
        public void Deserialize_Object_With_One_String_Property()
        {
            var dummy = _fixture.Create<Dummy2>();
            var serializer = new SwiftySendSerializer(typeof(Dummy2));

            var xmlInput = XmlRepresentation.GetXml(dummy);
            var result = serializer.Deserialize<Dummy2>(xmlInput);

            Assert.Equal(dummy.StringProperty, result.StringProperty);
        }


        [Fact]
        public void Deserialize_Object_With_One_String_Property_And_One_String_Field()
        {
            
            var dummy = _fixture.Create<Dummy3>();
            var serializer = new SwiftySendSerializer(typeof(Dummy3));

            var xmlInput = XmlRepresentation.GetXml(dummy);
            var result = serializer.Deserialize<Dummy3>(xmlInput);

            Assert.Equal(dummy.StringProperty, result.StringProperty);
            Assert.Equal(dummy.StringField, result.StringField);
        }

        [Fact]
        public void Deserialize_Object_With_One_Int_Field_And_One_Enum_Property()
        {

            var dummy = _fixture.Create<Dummy4>();
            var serializer = new SwiftySendSerializer(typeof(Dummy4));

            var xmlInput = XmlRepresentation.GetXml(dummy);
            var result = serializer.Deserialize<Dummy4>(xmlInput);

            Assert.Equal(dummy.IntField, result.IntField);
            Assert.Equal(dummy.EnumProperty, result.EnumProperty);
        }
    }
}
