using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;
using BenchmarkDotNet.Running;
using System.Collections.Generic;
using System.Linq;
using System;

namespace ListBenchmarks
{
    [MemoryDiagnoser]
    public class ReduceInt32
    {
        private const int N = 1_000_000;
        private readonly List<int> data;

        public ReduceInt32()
        {
            data = new List<int>(Enumerable.Range(1, N));
        }

        [Benchmark]
        public int LinqAggregate() => data.Aggregate((total, num) => total + num);

        [Benchmark]
        public int LoopAggregate()
        {
            int total = 0;
            for (int i = 0; i < data.Count; i++)
            {
                total += data[i];
            }

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

    [MemoryDiagnoser]
    public class PopulateInt32
    {
        private const int N = 1_000_000;
        private readonly List<int> data;

        public PopulateInt32()
        {
            // Initialize pool of data to fill
            data = new List<int>(Enumerable.Range(1, N));
        }

        [Benchmark]
        public List<int> LinqPopulate()
        {
            var rnd = new Random();
            return data.Select(_ => rnd.Next(1, 101)).ToList();
        }

        [Benchmark]
        public List<int> LoopPopulate()
        {
            var result = new List<int>(N);
            var rnd = new Random();

            for (int i = 0; i < data.Count; i++)
            {
                result.Add(rnd.Next(1, 101));
            }

            return result;
        }

        [Benchmark]
        public List<int> IteratorPopulate()
        {
            var rnd = new Random();
            var result = new List<int>(N);

            foreach (int value in data)
            {
                result.Add(rnd.Next(1, 101));
            }

            return result;
        }
    }

    [MemoryDiagnoser]
    public class IterateInt32
    {
        private const int N = 1_000_000;
        private readonly List<int> data;

        public IterateInt32()
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
                if (value > 0) ++count;
            }

            return count;
        }
    }

    [MemoryDiagnoser]
    public class ContainsInt32
    {
        private const int N = 1_000_000;
        private readonly int target;
        private readonly List<int> data;

        public ContainsInt32()
        {
            var rnd = new Random();

            data = new List<int>(N);
            target = rnd.Next(1, 101);

            while (data.Count < N)
            {
                data.Add(rnd.Next(1, 101));
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

    [MemoryDiagnoser]
    public class FilterInt32
    {
        private const int N = 1_000_000;
        private readonly List<int> data;
        private readonly Consumer consumer;

        public FilterInt32()
        {
            var rnd = new Random();

            consumer = new Consumer();
            data = new List<int>(N);

            for (int i = 0; i < N; i++)
            {
                data.Add(rnd.Next(-N, N));
            }
        }

        [Benchmark]
        public void LinqFilter() => data.Where(n => n >= 0).Consume(consumer);

        [Benchmark]
        public void LoopFilter()
        {
            var result = new List<int>();

            for (int i = 0; i < data.Count; i++)
            {
                if (data[i] >= 0) result.Add(data[i]);
            }

            result.Consume(consumer);
        }

        [Benchmark]
        public void IteratorFilter()
        {
            var result = new List<int>();

            foreach (int value in data)
            {
                if (value >= 0) result.Add(value);
            }

            result.Consume(consumer);
        }
    }

    [MemoryDiagnoser]
    public class CopyInt32
    {
        private const int N = 1_000_000;
        private readonly List<int> data;

        public CopyInt32()
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

    [MemoryDiagnoser]
    public class MapInt32
    {
        private const int N = 1_000_000;
        private readonly List<int> data;
        private readonly Consumer consumer;

        public MapInt32()
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
}
