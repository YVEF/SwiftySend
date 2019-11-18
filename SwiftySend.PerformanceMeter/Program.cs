using AutoFixture;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;
using BenchmarkDotNet.Running;
using System;
using System.Collections.Generic;
using YAXLib;

namespace SwiftySend.PerformanceMeter
{
    public class Dummy
    {
        public string StringProperty1 { get; set; }
        public string StringProperty2 { get; set; }

        public string StringProperty3 { get; set; }
        public string StringProperty4 { get; set; }

        public string StringProperty5 { get; set; }
        public string StringProperty6 { get; set; }

        public string StringProperty7 { get; set; }
        public string StringProperty8 { get; set; }

        public string StringProperty9 { get; set; }
        public string StringProperty10 { get; set; }
        public string StringProperty11 { get; set; }
        public string StringProperty12 { get; set; }
        public string StringProperty13 { get; set; }
        public string StringProperty14 { get; set; }
    }
    class Program
    {
        static void Main(string[] args)
        {
            var summary = BenchmarkRunner.Run<TestingGround>();

            Console.Read();
        }
    }

    [SimpleJob(launchCount: 3, warmupCount: 0, targetCount: 10)]
    public class TestingGround
    {
        private Fixture fixture = new Fixture();
        private IList<Dummy> _testDatas = new List<Dummy>();
        private YAXSerializer YAXSerializer;
        private SwiftySendSerializer SwiftySendSerializer;

        [Params(5, 20)]
        public int limit;

        [IterationSetup]
        public void InitializeTest()
        {
            for (int i = 0; i < limit; i++)
                _testDatas.Add(fixture.Create<Dummy>());
            
            
        }

        [Benchmark]
        public string YaxlibTest()
        {
            YAXSerializer = new YAXSerializer(typeof(Dummy));
            foreach (var item in _testDatas)
                YAXSerializer.Serialize(item);
            return YAXSerializer.Serialize(_testDatas[0]);
        }

        [Benchmark]
        public string SwiftySendTest()
        {
            SwiftySendSerializer = new SwiftySendSerializer(typeof(Dummy));
            foreach (var item in _testDatas)
                SwiftySendSerializer.Serialize(item);
            return SwiftySendSerializer.Serialize(_testDatas[0]);
        }
    }
}
