using SwiftySendTest.TestData;
using AutoFixture;
using Xunit;

namespace SwiftySendTest.XmlDeserialization
{
    public class MoreComplexDeserializationTest : DeserializationTestBase
    {
        [Fact]
        public void Serialize_Object_With_One_Complex_Property_And_One_String_Property()
        {
            var dummy = _fixture.Create<ComplexDummy1>();
            Check(() => XmlRepresentation.GetXml(dummy), dummy);
        }
    }
}
