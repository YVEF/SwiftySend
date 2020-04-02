using Xunit;
using AutoFixture;
using SwiftySendTest.TestData;
using System;

namespace SwiftySendTest.XmlSerialization
{
    public class FlatSerializationTest : SerializationTestBase
    {
        
        [Fact]
        public void Serialize_Object_With_One_String_Field()
        {
            var dummy = _fixture.Create<Dummy1>();
            Check(() => XmlRepresentation.GetXml(dummy), dummy);
        }

        [Fact]
        public void Serialize_Object_With_One_String_Property()
        {
            var dummy = _fixture.Create<Dummy2>();
            Check(() => XmlRepresentation.GetXml(dummy), dummy);
        }

        [Fact]
        public void Serialize_Object_With_One_String_Property_And_One_String_Field()
        {
            var dummy = _fixture.Create<Dummy3>();
            Check(() => XmlRepresentation.GetXml(dummy), dummy);
        }

        [Fact]
        public void Serialize_Object_With_One_Enum_Property_And_One_Int_Field()
        {
            var dummy = _fixture.Create<Dummy4>();
            Check(() => XmlRepresentation.GetXml(dummy), dummy);
        }

        [Fact]
        public void Serialize_Object_With_All_Of_The_Base_Types()
        {
            var dummy = _fixture.Create<Dummy5>();
            Check(() => XmlRepresentation.GetXml(dummy), dummy);
        }

        [Fact]
        public void Serialize_Object_With_Datetime()
        {
            var dummy = _fixture.Create<Dummy6>();
            Check(() => XmlRepresentation.GetXml(dummy), dummy);
        }

        [Fact]
        public void Serialize_Object_With_Private_Field_And_Property()
        {
            var dummy = new Dummy7(_fixture.Create<int>(), _fixture.Create<DateTime>());
            Check(() => XmlRepresentation.GetXml(dummy), dummy);
        }



    }
}
