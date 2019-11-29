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
        public void Serialize_Object_With_One_String_Field()
        {
            var dummy = _fixture.Create<Dummy1>();
            var serializer = new SwiftySendSerializer(typeof(Dummy1));

            var xmlInput = XmlRepresentation.GetXml(dummy);
            var result = serializer.Deserialize<Dummy1>(xmlInput);

            Assert.Equal(dummy.StringField, result.StringField);
        }


        [Fact]
        public void Ser()
        {
            var dummy = _fixture.Create<Dummy2>();
            var serializer = new SwiftySendSerializer(typeof(Dummy2));

            var xmlInput = XmlRepresentation.GetXml(dummy);
            var result = serializer.Deserialize<Dummy2>(xmlInput);

            Assert.Equal(dummy.StringProperty, result.StringProperty);
        }
    }
}
