using SwiftySend;
using System;
using Xunit;

namespace SwiftySendTest
{
    public abstract class ObjectSerializationTestBase
    {
        public void CheckResult<TDummy>(Func<string> expectedResult, TDummy testObject)
        {
            var swiftySerializer = new SwiftySendSerializer(typeof(TDummy));
            //var a = swiftySerializer.Serialize(testObject);
            Assert.Equal(expectedResult.Invoke(), swiftySerializer.Serialize(testObject));
        }
    }
}
