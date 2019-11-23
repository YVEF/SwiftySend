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
        public void Serialize_Object_With_One_Complex_Property_And_One_String_Property()
        {
            var dummy = _fixture.Create<ComplexDummy1>();
            CheckResult(() => ExpectedResultCollection.GetResult(dummy), dummy);
        }


        [Fact]
        public void Serialize_Object_With_One_ListString_Property()
        {
            var dummy = _fixture.Create<CollectionDummy1>();
            CheckResult(() => ExpectedResultCollection.GetResult(dummy), dummy);
        }
    }
}
