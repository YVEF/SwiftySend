

using System.Collections.Generic;

namespace SwiftySendTest.TestData
{
    internal class ComplexDummy1
    {
        public string StringProperty { get; set; }
        public ComplexDummy2 ComplexDummy2Field;
    }

    internal class ComplexDummy2
    {
        public bool BoolField;
    }



    internal class ComplexDummy3
    {
        public int IntProperty { get; set; }
        public ComplexDummy1 ComplexDummy1Field;
        public ComplexDummy2 ComplexDummy2Property { get; set; }
    }


    internal class CollectionDummy1
    {
        public List<string> ListStringProperty { get; set; }
    }
}
