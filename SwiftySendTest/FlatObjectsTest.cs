using Xunit;
using AutoFixture;
using SwiftySend;
using SwiftySendTest.TestData;

namespace SwiftySendTest
{
    public class FlatObjectsTest
    {
        [Fact]
        public void Serialize_Object_With_One_String_Field()
        {
            var swiftySerializer = new SwiftySendSerializer(typeof(Dummy1));
            var dummy = new Fixture().Create<Dummy1>();
            var result = swiftySerializer.Serialize(dummy);

            Assert.Equal(ExpectedResultCollection.GetResult(dummy), result);
        }

        [Fact]
        public void Serialize_Object_With_One_String_Property()
        {
            var swiftySerializer = new SwiftySendSerializer(typeof(Dummy2));
            var dummy = new Fixture().Create<Dummy2>();
            
            var result = swiftySerializer.Serialize(dummy);

            Assert.Equal(ExpectedResultCollection.GetResult(dummy), result);
        }

        [Fact]
        public void Serialize_Object_With_One_String_Property_And_One_String_Field()
        {
            var swiftySerializer = new SwiftySendSerializer(typeof(Dummy3));
            var dummy = new Fixture().Create<Dummy3>();
            var result = swiftySerializer.Serialize(dummy);

            Assert.Equal(ExpectedResultCollection.GetResult(dummy), result);
        }

        [Fact]
        public void Serialize_Object_With_One_Enum_Property_And_One_Int_Field()
        {
            var swiftySerializer = new SwiftySendSerializer(typeof(Dummy4));
            var dummy = new Fixture().Create<Dummy4>();
            var result = swiftySerializer.Serialize(dummy);

            Assert.Equal(ExpectedResultCollection.GetResult(dummy), result);
        }

        [Fact]
        public void Serialize_Flat_Object_With_All_Of_The_Base_Types()
        {
            var swiftySerializer = new SwiftySendSerializer(typeof(Dummy5));
            var dummy = new Fixture().Create<Dummy5>();
            var result = swiftySerializer.Serialize(dummy);

            Assert.Equal(ExpectedResultCollection.GetResult(dummy), result);
        }



    }
}
