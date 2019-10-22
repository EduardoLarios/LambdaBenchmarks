using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using System.Collections.Generic;
using System.Linq;
using System;
using BenchmarkDotNet.Engines;

namespace ListBenchmarks
{
    public class ReduceList
    {
        private const int N = 1_000_000;
        private readonly List<int> data;

        public ReduceList()
        {
            data = new List<int>(Enumerable.Range(1, N));
        }

        [Benchmark]
        public int LinqAggregate() => data.Aggregate((total, num) => total + num);

        [Benchmark]
        public int LoopAggregate()
        {
            int total = 0;
            for (int i = 0; i < data.Count; i++) total += data[i];
            return total;
        }

        [Benchmark]
        public int IteratorAggregate()
        {
            int total = 0;
            foreach (int value in data)
            {
                total += value;
            }

            return total;
        }
    }

    public class PopulateList
    {
        private const int N = 1_000_000;
        private readonly List<int> data;

        public PopulateList()
        {
            // Initialize pool of data to fill
            data = new List<int>(Enumerable.Range(1, N));
        }

        [Benchmark]
        public List<int> LinqPopulate()
        {
            var generator = new Random();
            return data.Select(_ => generator.Next(1, 101)).ToList();
        }

        [Benchmark]
        public List<int> LoopPopulate()
        {
            var ForLoopData = new List<int>(N);
            var generator = new Random();

            for (int i = 0; i < data.Count; i++)
            {
                ForLoopData.Add(generator.Next(1, 101));
            }

            return ForLoopData;
        }

        [Benchmark]
        public List<int> IteratorPopulate()
        {
            var generator = new Random();
            var ForEachData = new List<int>(N);

            foreach (int value in data)
            {
                ForEachData.Add(generator.Next(1, 101));
            }

            return ForEachData;
        }
    }

    public class IterateList
    {
        private const int N = 1_000_000;
        private readonly List<int> data;

        public IterateList()
        {

            data = new List<int>(Enumerable.Repeat(1, N));
        }

        [Benchmark]
        public int LinqIterate() => data.Count(n => n > 0);

        [Benchmark]
        public int LoopIterate()
        {
            int count = 0;
            for (int i = 0; i < data.Count; i++)
            {
                if (data[i] > 0) ++count;
            }

            return count;
        }

        [Benchmark]
        public int IteratorIterate()
        {
            int count = 0;
            foreach (int value in data)
            {
                if(value > 0) ++count;
            }

            return count;
        }
    }

    public class ContainsList
    {
        private const int N = 1_000_000;
        private readonly int target;
        private readonly List<int> data;

        public ContainsList()
        {
            var generator = new Random();

            data = new List<int>(N);
            target = generator.Next(1, 101);

            while (data.Count < N)
            {
                data.Add(generator.Next(1, 101));
            }
        }

        [Benchmark]
        public int LinqContains() => data.Find(n => n == target);

        [Benchmark]
        public int LoopContains()
        {
            for (int i = 0; i < data.Count; i++)
            {
                if (data[i] == target) return data[i];
            }

            return default;
        }

        [Benchmark]
        public int IteratorContains()
        {
            foreach (int value in data)
            {
                if (value == target) return value;
            }

            return default;
        }
    }

    public class FilterList
    {
        private const int N = 1_000_000;
        private readonly List<int> data;
        private readonly Consumer consumer;

        public FilterList()
        {
            var generator = new Random();

            consumer = new Consumer();
            data = new List<int>(N);

            for (int i = 0; i < N; i++)
            {
                data.Add(generator.Next(-N, N));
            }
        }

        [Benchmark]
        public void LinqFilter() => data.Where(n => n >= 0).Consume(consumer);

        [Benchmark]
        public void LoopFilter()
        {
            var result = new List<int>(N);

            for (int i = 0; i < data.Count; i++)
            {
                if (data[i] >= 0) result.Add(data[i]);
            }

            result.Consume(consumer);
        }

        [Benchmark]
        public void IteratorFilter()
        {
            var result = new List<int>(N);

            foreach (int value in data)
            {
                if (value >= 0) result.Add(value);
            }

            result.Consume(consumer);
        }
    }

    public class CopyList
    {
        private const int N = 1_000_000;
        private readonly List<int> data;

        public CopyList()
        {
            data = new List<int>(Enumerable.Range(1, N));
        }

        [Benchmark]
        public List<int> LinqCopy() => data.Select(n => n).ToList();

        [Benchmark]
        public List<int> LoopCopy()
        {
            var copy = new List<int>(data.Count);
            for (int i = 0; i < data.Count; i++)
            {
                copy.Add(data[i]);
            }

            return copy;
        }

        [Benchmark]
        public List<int> IteratorCopy()
        {
            var copy = new List<int>(data.Count);
            foreach (int value in data)
            {
                copy.Add(value);
            }

            return copy;
        }
    }

    public class MapList
    {
        private const int N = 1_000_000;
        private readonly List<int> data;
        private readonly Consumer consumer;

        public MapList()
        {
            consumer = new Consumer();
            data = new List<int>(Enumerable.Range(1, N));
        }

        [Benchmark]
        public void LinqMap() => data.Select(n => n * n).Consume(consumer);

        [Benchmark]
        public void LoopMap()
        {
            var result = new List<int>(N);
            for (int i = 0; i < data.Count; i++)
            {
                 result.Add(data[i] * data[i]);
            }

            result.Consume(consumer);
        }

        [Benchmark]
        public void IteratorMap()
        {
            var result = new List<int>(N);
            foreach (var value in data)
            {
                result.Add(value * value);
            }

            result.Consume(consumer);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("--- LIST BENCHMARKS ---");
            //BenchmarkRunner.Run<PopulateList>();
            //BenchmarkRunner.Run<IterateList>();
            //BenchmarkRunner.Run<ContainsList>();
            //BenchmarkRunner.Run<CopyList>();
            BenchmarkRunner.Run<MapList>();
            BenchmarkRunner.Run<FilterList>();
            BenchmarkRunner.Run<ReduceList>();
        }
    }
}
