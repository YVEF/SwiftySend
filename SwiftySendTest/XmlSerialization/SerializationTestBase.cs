using AutoFixture;
using SwiftySend;
using System;
using Xunit;

namespace SwiftySendTest.XmlSerialization
{
    public abstract class SerializationTestBase
    {
        protected Fixture _fixture = new Fixture();
        public void Check<TDummy>(Func<string> expectedResult, TDummy testObject)
        {
            var swiftySerializer = new SwiftySendSerializer(typeof(TDummy));
            var result = swiftySerializer.Serialize(testObject);
            Assert.Equal(expectedResult.Invoke(), result);
        }
    }
}
