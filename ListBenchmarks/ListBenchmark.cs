using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;
using BenchmarkDotNet.Running;
using System.Collections.Generic;
using System.Linq;
using System;

namespace ListBenchmarks
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("--- LIST BENCHMARKS ---");
            //BenchmarkRunner.Run<PopulateList>();
            //BenchmarkRunner.Run<IterateList>();
            //BenchmarkRunner.Run<ContainsList>();
            //BenchmarkRunner.Run<CopyList>();
            //BenchmarkRunner.Run<MapInt32>();
            //BenchmarkRunner.Run<FilterInt32>();
            //BenchmarkRunner.Run<ReduceInt32>();
            BenchmarkRunner.Run<ReduceInt32>();
        }
    }
}
