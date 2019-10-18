using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using System.Collections.Generic;
using System.Linq;
using System;

namespace DictionaryBenchmarks
{
    class DictionaryBenchmarks
    {
        public class ReduceDictionary
        {
            private const int N = 1_000_000;
            private readonly List<int> data;

            public ReduceDictionary()
            {
                data = new List<int>(Enumerable.Range(1, N));
            }

            [Benchmark]
            public int LinqAggregate() => data.Aggregate((total, num) => total + num);

            [Benchmark]
            public int ForLoopAggregate()
            {
                int total = 0;
                for (int i = 0; i < data.Count; i++) total += data[i];
                return total;
            }

            [Benchmark]
            public int ForEachAggregate()
            {
                int total = 0;
                foreach (int value in data) total += value;
                return total;
            }
        }

        static void Main(string[] args)
        {
            Console.WriteLine("--- DICTIONARY BENCHMARKS ---");
            //BenchmarkRunner.Run<PopulateList>();
            //BenchmarkRunner.Run<IterateList>();
            //BenchmarkRunner.Run<ContainsList>();
            //BenchmarkRunner.Run<CopyList>();
            //BenchmarkRunner.Run<MapList>();
            //BenchmarkRunner.Run<FilterList>();
            //BenchmarkRunner.Run<ReduceHashSet>();
        }
    }
}
