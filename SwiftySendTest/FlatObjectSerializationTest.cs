using Xunit;
using AutoFixture;
using SwiftySend;
using SwiftySendTest.TestData;

namespace SwiftySendTest
{
    public class FlatObjectSerializationTest : ObjectSerializationTestBase
    {
        [Fact]
        public void Serialize_Object_With_One_String_Field()
        {
            var dummy = new Fixture().Create<Dummy1>();
            CheckResult(() => ExpectedResultCollection.GetResult(dummy), dummy);
        }

        [Fact]
        public void Serialize_Object_With_One_String_Property()
        {
            var dummy = new Fixture().Create<Dummy2>();
            CheckResult(() => ExpectedResultCollection.GetResult(dummy), dummy);
        }

        [Fact]
        public void Serialize_Object_With_One_String_Property_And_One_String_Field()
        {
            var dummy = new Fixture().Create<Dummy3>();
            CheckResult(() => ExpectedResultCollection.GetResult(dummy), dummy);
        }

        [Fact]
        public void Serialize_Object_With_One_Enum_Property_And_One_Int_Field()
        {
            var dummy = new Fixture().Create<Dummy4>();
            CheckResult(() => ExpectedResultCollection.GetResult(dummy), dummy);
        }

        [Fact]
        public void Serialize_Object_With_All_Of_The_Base_Types()
        {
            var dummy = new Fixture().Create<Dummy5>();
            CheckResult(() => ExpectedResultCollection.GetResult(dummy), dummy);
        }

        [Fact]
        public void Serialize_Object_With_Datetime()
        {
            var dummy = new Fixture().Create<Dummy6>();
            CheckResult(() => ExpectedResultCollection.GetResult(dummy), dummy);
        }



    }
}
