using AutoFixture;
using SwiftySendTest.TestData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SwiftySendTest
{
    public class MoreComplexObjectSerializationTest : ObjectSerializationTestBase
    {

        [Fact]
        public void Test()
        {
            var dummy = new Fixture().Create<ComplexDummy1>();
            CheckResult(() => ExpectedResultCollection.GetResult(dummy), dummy);
        }
    }
}
