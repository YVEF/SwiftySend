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
        private IList<Dummy> _testDatas = new List<Dummy>();

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
                _testDatas.Add(fixture.Create<Dummy>());


        }



        [Benchmark]
        public string Yaxlib_WithoutInitialization()
        {
            foreach (var item in _testDatas)
            {
                string result = YAXSerializer.Serialize(item);
            }
            return YAXSerializer.Serialize(_testDatas[0]);
        }

        [Benchmark]
        public string SwiftySend_WithoutInitialization()
        {
            foreach (var item in _testDatas)
            {
                string result = SwiftySendSerializer.Serialize(item);
            }

            return SwiftySendSerializer.Serialize(_testDatas[0]);
        }



        [Benchmark]
        public string Yaxlib_WithInitialization()
        {
            var serializer = new YAXSerializer(typeof(Dummy));
            foreach (var item in _testDatas)
            {
                string result = serializer.Serialize(item);
            }

            return serializer.Serialize(_testDatas[0]);
        }




        [Benchmark]
        public string SwiftySend_WithInitialization()
        {
            var serializer = new SwiftySendSerializer(typeof(Dummy));
            foreach (var item in _testDatas)
            {
                string result = serializer.Serialize(item);
            }

            return serializer.Serialize(_testDatas[0]);
        }
    }
}
