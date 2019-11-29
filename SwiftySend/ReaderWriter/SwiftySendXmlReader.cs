using SwiftySend.Core;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace SwiftySend.ReaderWriter
{
    internal class SwiftySendXmlReader
    {
        private readonly string _xmlInput;

        public SwiftySendXmlReader(string xmlInput)
        {
            _xmlInput = xmlInput;
        }


        public IList<SerializationNode> ParseXml() =>
            ParseXmlInternal(XDocument.Parse(_xmlInput).Root);

        private IList<SerializationNode> ParseXmlInternal(XElement element)
        {
            var serializationNodes = new List<SerializationNode>();

            foreach(var item in element.Elements())
            {
                if (item.Elements().FirstOrDefault() != null)
                    serializationNodes.Add(new SerializationNode() { Name = item.Name.ToString(), NestedNodes = ParseXmlInternal(item) });

                else
                    serializationNodes.Add(new SerializationNode() { Name = item.Name.ToString(), Value = item.Value });
            }

            return serializationNodes;
        }
    }
}
