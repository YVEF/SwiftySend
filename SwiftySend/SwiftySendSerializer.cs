﻿using SwiftySend.ReaderWriter;
using System;
using System.Linq;
using System.Runtime.CompilerServices;

namespace SwiftySend
{
    public class SwiftySendSerializer
    {
        private readonly Type _targetType;        
        private readonly SerializableStructureBuilder _serializableStructureBuilder;
        private readonly SwiftyReader _reader = new SwiftyReader();

        public SwiftySendSerializer(Type targetType)
        {
            _targetType = targetType ?? throw new ArgumentNullException(nameof(targetType));
            _serializableStructureBuilder = new SerializableStructureBuilder().Build(targetType);
        }

        

        public string Serialize<TObject>(TObject @object)
        {
            if (@object == null)
                throw new ArgumentNullException(nameof(@object));

            _CheckRequirementsForType<TObject>();


            var serializationNodes = _serializableStructureBuilder.GenerateSerializableStructure(@object);
            return new SwiftyWriter(typeof(TObject), serializationNodes).CreateXml().ToString();
        }


        public TObject Deserialize<TObject>(string xmlInput)
        {
            if (string.IsNullOrEmpty(xmlInput))
                throw new ArgumentNullException(nameof(xmlInput));

            _CheckRequirementsForType<TObject>();

            var serializationNodes = _serializableStructureBuilder.ReorderObjectMembersIfNeeded(_reader.ParseXml(xmlInput));
            // [TODO]: need resolve problem with ToArray()
            var result = _serializableStructureBuilder.GenerateObject<TObject>(serializationNodes.ToArray());

            return result;
        }












        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void _CheckRequirementsForType<TObject>()
        {            
            if (typeof(TObject) != _targetType)
                throw new InvalidOperationException($"Target type: '{_targetType.Name}' and current type: '{typeof(TObject).Name}' mismatch");
        }
    }
}
