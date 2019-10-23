using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;
using BenchmarkDotNet.Running;
using System.Collections.Generic;
using System.Linq;
using System;

namespace HashSetBenchmarks
{
    class HashSetBenchmarks
    {
        static void Main(string[] args)
        {
            Console.WriteLine("--- HASHSET BENCHMARKS ---");
            //BenchmarkRunner.Run<PopulateHashSet>();
            //BenchmarkRunner.Run<IterateHashSet>();
            //BenchmarkRunner.Run<ContainsHashSet>();
            //BenchmarkRunner.Run<CopyHashSet>();
            BenchmarkRunner.Run<MapInt64>();
            BenchmarkRunner.Run<FilterInt64>();
            BenchmarkRunner.Run<ReduceInt64>();
        }
    }
}
