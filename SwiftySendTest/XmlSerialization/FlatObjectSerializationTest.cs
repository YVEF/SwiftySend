using Xunit;
using AutoFixture;
using SwiftySend;
using SwiftySendTest.TestData;
using YAXLib;
using System;

namespace SwiftySendTest
{
    public class FlatObjectSerializationTest : ObjectSerializationTestBase
    {
        
        [Fact]
        public void Serialize_Object_With_One_String_Field()
        {
            var dummy = _fixture.Create<Dummy1>();
            CheckResult(() => XmlRepresentation.GetXml(dummy), dummy);
        }

        [Fact]
        public void Serialize_Object_With_One_String_Property()
        {
            var dummy = _fixture.Create<Dummy2>();
            CheckResult(() => XmlRepresentation.GetXml(dummy), dummy);
        }

        [Fact]
        public void Serialize_Object_With_One_String_Property_And_One_String_Field()
        {
            var dummy = _fixture.Create<Dummy3>();
            CheckResult(() => XmlRepresentation.GetXml(dummy), dummy);
        }

        [Fact]
        public void Serialize_Object_With_One_Enum_Property_And_One_Int_Field()
        {
            var dummy = _fixture.Create<Dummy4>();
            CheckResult(() => XmlRepresentation.GetXml(dummy), dummy);
        }

        [Fact]
        public void Serialize_Object_With_All_Of_The_Base_Types()
        {
            var dummy = _fixture.Create<Dummy5>();
            CheckResult(() => XmlRepresentation.GetXml(dummy), dummy);
        }

        [Fact]
        public void Serialize_Object_With_Datetime()
        {
            var dummy = _fixture.Create<Dummy6>();
            CheckResult(() => XmlRepresentation.GetXml(dummy), dummy);
        }

        [Fact]
        public void Serialize_Object_With_Private_Field_And_Property()
        {
            var dummy = new Dummy7(_fixture.Create<int>(), _fixture.Create<DateTime>());
            CheckResult(() => XmlRepresentation.GetXml(dummy), dummy);
        }



    }
}