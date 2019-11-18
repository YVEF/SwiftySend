using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace SwiftySend.Core
{
    [StructLayout(LayoutKind.Sequential)]
    public struct SerializationNode
    {
        public string Name;
        public object Value;
        public int Nesting;

        public IList<SerializationNode> NestedNodes;
    }
}
