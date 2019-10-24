using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;
using BenchmarkDotNet.Running;
using System.Collections.Generic;
using System.Linq;
using System;

namespace LinkedListBenchmarks
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("--- LINKED LIST BENCHMARKS ---");
            //BenchmarkRunner.Run<PopulateLinkedList>();
            //BenchmarkRunner.Run<IterateLinkedList>();
            //BenchmarkRunner.Run<ContainsLinkedList>();
            //BenchmarkRunner.Run<CopyLinkedList>();
            //BenchmarkRunner.Run<MapInt32>();
            //BenchmarkRunner.Run<FilterInt32>();
            //BenchmarkRunner.Run<ReduceInt32>();
            BenchmarkRunner.Run<CopyStudent>();
        }
    }
}
