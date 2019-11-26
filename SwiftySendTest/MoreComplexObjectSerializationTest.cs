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

        [Fact]
        public void Serialize_Object_With_One_ICollectionString_Property()
        {
            var dummy = _fixture.Build<AbstractCollectionDummy1>()
                .With(x => x.ICollectionStringProperty, _fixture.CreateMany<string>().ToList())
                .Create();
            CheckResult(() => ExpectedResultCollection.GetResult(dummy), dummy);
            
        }


        [Fact]
        public void Serialize_Object_With_One_IEnumerable_Of_SimpleDummy_Property()
        {
            var dummy = _fixture.Build<EnumerableDummyWithComplexDummy1>()
                .With(x => x.IEnumerableSimpleDummyProperty, _fixture.CreateMany<SimpleDummy2>().ToList())
                .Create();
            CheckResult(() => ExpectedResultCollection.GetResult(dummy), dummy);
        }
    }
}
