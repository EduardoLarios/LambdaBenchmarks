using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;
using BenchmarkDotNet.Running;
using System.Collections.Generic;
using System.Linq;
using System;

namespace DictionaryBenchmarks
{
    class DictionaryBenchmarks
    {
        static void Main(string[] args)
        {
            Console.WriteLine("--- DICTIONARY BENCHMARKS ---");
            //BenchmarkRunner.Run<PopulateDictionary>();
            //BenchmarkRunner.Run<IterateDictionary>();
            //BenchmarkRunner.Run<ContainsDictionary>();
            //BenchmarkRunner.Run<CopyDictionary>();
            BenchmarkRunner.Run<MapInt32>();
            BenchmarkRunner.Run<FilterInt32>();
            BenchmarkRunner.Run<ReduceInt32>();
        }
    }
}
