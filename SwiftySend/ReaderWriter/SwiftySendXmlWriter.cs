using SwiftySend.Core;
using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace SwiftySend.ReaderWriter
{
    internal class SwiftySendXmlWriter
    {
        private readonly IList<SerializationNode> _serializationNodes;

        private readonly XDocument _xmlDocument;
        private readonly XElement _rootElement;

        public SwiftySendXmlWriter(Type rootType, IList<SerializationNode> serializationNodes)
        {
            _serializationNodes = serializationNodes ?? throw new ArgumentNullException(nameof(serializationNodes));
            _rootElement = new XElement(rootType.Name);
            _xmlDocument = new XDocument(_rootElement);
        }

        public XDocument CreateXml()
        {
            foreach(var node in _serializationNodes)
                _rootElement.Add(_CreateElement(node));

            return _xmlDocument;
        }

        

        private XElement _CreateElement(SerializationNode serializationNode)
        {
            var element = new XElement(serializationNode.Name);
            if(serializationNode.NestedNodes == null || serializationNode.NestedNodes.Count == 0)
            {
                element.Add(serializationNode.Value);
                return element;
            }
            foreach (var node in serializationNode.NestedNodes)
                element.Add(_CreateElement(node));

            return element;

        }

    }
}
