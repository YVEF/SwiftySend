using AutoFixture;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using System;
using System.Collections.Generic;
using YAXLib;

namespace SwiftySend.PerformanceMeter
{
    [YAXSerializableType(FieldsToSerialize = YAXSerializationFields.AllFields)]
    public class Dummy
    {
        public string StringProperty1 { get; set; }
        public DateTime DateTimeProperty2 { get; set; }

        public string StringProperty3 { get; set; }
        public decimal DecimalField;

        public string StringProperty5 { get; set; }
        public int IntProperty6 { get; set; }        

        public bool BoolProperty7 { get; set; }
        public short ShortProperty8 { get; set; }
        public object ObjectProperty9 { get; set; }
        public long LongProperty10 { get; set; }
        public char CharProperty11 { get; set; }


        //private string StringProperty12 { get => "random data"; }

        //private string StringField13 = "random data";
        //private object ObjectField14 = new object();
    }

    public class Dummy2
    {
        public string Property { get; set; }
    }

    public class Dummy3
    {
        public object ObjectProperty { get; set; }
    }
    class Program
    {
        static void Main(string[] args)
        {
            var summary = BenchmarkRunner.Run<TestingGround>();

            Console.Read();
        }
    }


    [SimpleJob(launchCount: 6, warmupCount: 0, targetCount: 20)]
    public class TestingGround
    {
        private Fixture fixture = new Fixture();
        private IList<Dummy> _testDataObjects = new List<Dummy>();
        private IList<string> _testDataSwiftySendXmls = new List<string>();
        private IList<string> _testDataYaxLibXmls = new List<string>();

        private YAXSerializer YAXSerializer;
        private SwiftySendSerializer SwiftySendSerializer;


        [Params(3, 20)]
        public int limit;

        [IterationSetup]
        public void InitializeTest()
        {
            YAXSerializer = new YAXSerializer(typeof(Dummy));
            SwiftySendSerializer = new SwiftySendSerializer(typeof(Dummy));

            for (int i = 0; i < limit; i++)
                _testDataObjects.Add(fixture.Create<Dummy>());

            var swifty = new SwiftySendSerializer(typeof(Dummy));
            var yax = new YAXSerializer(typeof(Dummy));
            foreach(var item in _testDataObjects)
            {
                _testDataSwiftySendXmls.Add(swifty.Serialize(item));
                _testDataYaxLibXmls.Add(yax.Serialize(item));
            }


        }



        [Benchmark]
        public string Yaxlib_Serialization_Without_Initialization()
        {
            foreach (var item in _testDataObjects)
            {
                string result = YAXSerializer.Serialize(item);
                string m = result;
            }
            return YAXSerializer.Serialize(_testDataObjects[0]);
        }

        [Benchmark]
        public string SwiftySend_Serialization_Without_Initialization()
        {
            foreach (var item in _testDataObjects)
            {
                string result = SwiftySendSerializer.Serialize(item);
                string m = result;
            }

            return SwiftySendSerializer.Serialize(_testDataObjects[0]);
        }



        [Benchmark]
        public string Yaxlib_Serialization_With_Initialization()
        {
            var serializer = new YAXSerializer(typeof(Dummy));
            foreach (var item in _testDataObjects)
            {
                string result = serializer.Serialize(item);
                string m = result;
            }

            return serializer.Serialize(_testDataObjects[0]);
        }




        [Benchmark]
        public string SwiftySend_Serialization_With_Initialization()
        {
            var serializer = new SwiftySendSerializer(typeof(Dummy));
            foreach (var item in _testDataObjects)
            {
                string result = serializer.Serialize(item);
                string m = result;
            }

            return serializer.Serialize(_testDataObjects[0]);
        }

        [Benchmark]
        public string Yaxlib_Deserialization_Without_Initialization()
        {
            foreach (var item in _testDataYaxLibXmls)
            {
                Dummy result = (Dummy)YAXSerializer.Deserialize(item);
                bool a = result.BoolProperty7;
            }

            return YAXSerializer.Serialize(_testDataObjects[0]);
        }




        [Benchmark]
        public string SwiftySend_Deserialization_Without_Initialization()
        {
            foreach (var item in _testDataSwiftySendXmls)
            {
                Dummy result = SwiftySendSerializer.Deserialize<Dummy>(item);
                bool a = result.BoolProperty7;
            }

            return SwiftySendSerializer.Serialize(_testDataObjects[0]);
        }


        [Benchmark]
        public string Yaxlib_Deserialization_With_Initialization()
        {
            var serializer = new YAXSerializer(typeof(Dummy));
            foreach (var item in _testDataYaxLibXmls)
            {
                Dummy result = (Dummy)serializer.Deserialize(item);
                bool a = result.BoolProperty7;
            }

            return serializer.Serialize(_testDataObjects[0]);
        }




        [Benchmark]
        public string SwiftySend_Deserialization_With_Initialization()
        {
            var serializer = new SwiftySendSerializer(typeof(Dummy));
            foreach (var item in _testDataSwiftySendXmls)
            {
                Dummy result = serializer.Deserialize<Dummy>(item);
                bool a = result.BoolProperty7;
            }

            return serializer.Serialize(_testDataObjects[0]);
        }
    }
}
