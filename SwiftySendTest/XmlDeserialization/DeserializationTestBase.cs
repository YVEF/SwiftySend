using AutoFixture;
using FluentAssertions;
using SwiftySend;
using System;

namespace SwiftySendTest.XmlDeserialization
{
    public abstract class DeserializationTestBase
    {
        protected Fixture _fixture = new Fixture();

        public void Chech<TObject>(Func<string> getXmlString, TObject expectedResult)
        {
            var serializer = new SwiftySendSerializer(typeof(TObject));
            var result = serializer.Deserialize<TObject>(getXmlString.Invoke());

            result.Should().BeEquivalentTo(expectedResult);
        }
    }
}
