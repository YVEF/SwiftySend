using AutoFixture;
using SwiftySend;
using System;
using Xunit;

namespace SwiftySendTest
{
    public abstract class ObjectSerializationTestBase
    {
        protected Fixture _fixture = new Fixture();
        public void CheckResult<TDummy>(Func<string> expectedResult, TDummy testObject)
        {
            var swiftySerializer = new SwiftySendSerializer(typeof(TDummy));
            var result = swiftySerializer.Serialize(testObject);
            Assert.Equal(expectedResult.Invoke(), result);
        }
    }
}
