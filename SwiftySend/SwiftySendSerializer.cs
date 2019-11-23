using SwiftySend.ReaderWriter;
using System;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace SwiftySend
{
    public class SwiftySendSerializer
    {
        private readonly Type _targetType;
        
        private readonly SerializableStructureBuilder _serializableStructureBuilder;
        

        public SwiftySendSerializer(Type targetType)
        {
            _targetType = targetType ?? throw new ArgumentNullException(nameof(targetType));

            _serializableStructureBuilder = new SerializableStructureBuilder().Build(targetType);


        }

        

        public string Serialize<TObject>(TObject @object)
        {
            _CheckRequirements(@object);

            var serializationNodes = _serializableStructureBuilder.GenerateSerializableStructure(@object);
            return new SwiftySendXmlWriter(typeof(TObject), serializationNodes).CreateXml().ToString();
        }






        


        

        



        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void _CheckRequirements<TObject>(TObject @object)
        {
            if (@object == null)
                throw new ArgumentNullException(nameof(@object));
            if (typeof(TObject) != _targetType)
                throw new InvalidOperationException($"Target type: '{_targetType.Name}' and current type: '{typeof(TObject).Name}' mismatch");
        }
    }
}
