using BenchmarkDotNet.Running;
using System;

namespace ListBenchmarks
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("--- LIST BENCHMARKS ---");
            BenchmarkRunner.Run<PopulateInt32>();
            BenchmarkRunner.Run<IterateInt32>();
            BenchmarkRunner.Run<ContainsInt32>();
            BenchmarkRunner.Run<CopyInt32>();
            BenchmarkRunner.Run<MapInt32>();
            BenchmarkRunner.Run<FilterInt32>();
            BenchmarkRunner.Run<ReduceInt32>();

            BenchmarkRunner.Run<PopulateInt64>();
            BenchmarkRunner.Run<IterateInt64>();
            BenchmarkRunner.Run<ContainsInt64>();
            BenchmarkRunner.Run<CopyInt64>();
            BenchmarkRunner.Run<MapInt64>();
            BenchmarkRunner.Run<FilterInt64>();
            BenchmarkRunner.Run<ReduceInt64>();

            BenchmarkRunner.Run<PopulateStudent>();
            BenchmarkRunner.Run<IterateStudent>();
            BenchmarkRunner.Run<ContainsStudent>();
            BenchmarkRunner.Run<CopyStudent>();
            BenchmarkRunner.Run<MapStudent>();
            BenchmarkRunner.Run<FilterStudent>();
            BenchmarkRunner.Run<ReduceStudent>();
        }
    }
}
