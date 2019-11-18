using System;
using Xunit;
using YAXLib;
using AutoFixture;
using SwiftySend;

namespace SwiftySendTest
{
    public class FlatObjectsTest
    {
        [Fact]
        public void Serialize_Object_With_One_Field()
        {
            var swiftySerializer = new SwiftySendSerializer(typeof(Dummy1));
            var dummy = new Fixture().Create<Dummy1>();
            var expectedResult = $"<Dummy1>\r\n  <StringValue>{dummy.StringField}</StringValue>\r\n</Dummy1>";
            var result = swiftySerializer.Serialize(dummy);

            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void Serialize_Object_With_One_Property()
        {
            //YAXSerializer _serializer = new YAXSerializer(typeof(Dummy2));
            var swiftySerializer = new SwiftySendSerializer(typeof(Dummy2));
            var dummy = new Fixture().Create<Dummy2>();
            var expectedResult = $"<Dummy2>\r\n  <StringProperty>{dummy.StringProperty}</StringProperty>\r\n</Dummy2>";
            var result = swiftySerializer.Serialize(dummy);
            //var justForTest = _serializer.Serialize(dummy);

            Assert.Equal(expectedResult, result);
        }


        public class Dummy2
        {
            public string StringProperty { get; set; }
        }

        public class Dummy1
        {
            public string StringField;
        }
    }
}
