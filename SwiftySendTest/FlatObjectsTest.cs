using Xunit;
using AutoFixture;
using SwiftySend;
using SwiftySendTest.TestData;

namespace SwiftySendTest
{
    public class FlatObjectsTest
    {
        [Fact]
        public void Serialize_Object_With_One_Field()
        {
            var swiftySerializer = new SwiftySendSerializer(typeof(Dummy1));
            var dummy = new Fixture().Create<Dummy1>();
            var result = swiftySerializer.Serialize(dummy);

            Assert.Equal(ExpectedResultCollection.Dummy1ExpectedResult(dummy), result);
        }

        [Fact]
        public void Serialize_Object_With_One_Property()
        {
            var swiftySerializer = new SwiftySendSerializer(typeof(Dummy2));
            var dummy = new Fixture().Create<Dummy2>();
            
            var result = swiftySerializer.Serialize(dummy);

            Assert.Equal(ExpectedResultCollection.Dummy2ExpectedResult(dummy), result);
        }

        [Fact]
        public void Serialize_Object_With_One_Property_And_One_Field()
        {
            var swiftySerializer = new SwiftySendSerializer(typeof(Dummy3));
            var dummy = new Fixture().Create<Dummy3>();
            var result = swiftySerializer.Serialize(dummy);

            Assert.Equal(ExpectedResultCollection.Dummy3ExpectedTest(dummy), result);
        }



    }
}
