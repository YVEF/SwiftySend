using SwiftySend.Core;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace SwiftySend.ReaderWriter
{
    internal class SwiftyReader
    {
        //private readonly string _intput;

        public SwiftyReader(/*string input*/)
        {
            //_intput = input;
        }


        public IList<SerializationNode> ParseXml(string intputXml) =>
            ParseXmlInternal(XDocument.Parse(intputXml).Root);

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
