

using System.Collections.Generic;

namespace SwiftySendTest.TestData
{
    internal class ComplexDummy1
    {
        public string StringProperty { get; set; }
        public SimpleDummy2 ComplexDummy2Field;
    }

    internal class SimpleDummy2
    {
        public bool BoolField;
    }



    internal class ComplexDummy3
    {
        public int IntProperty { get; set; }
        public ComplexDummy1 ComplexDummy1Field;
        public SimpleDummy2 ComplexDummy2Property { get; set; }
    }


    internal class CollectionDummy1
    {
        public List<string> ListStringProperty { get; set; }
    }


    internal class AbstractCollectionDummy1
    {
        public ICollection<string> ICollectionStringProperty { get; set; }
    }

    internal class EnumerableDummyWithComplexDummy1
    {
        public IEnumerable<SimpleDummy2> IEnumerableSimpleDummyProperty { get; set; }
    }
}
