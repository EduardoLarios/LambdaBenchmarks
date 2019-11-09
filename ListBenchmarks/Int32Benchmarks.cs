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
        [Params(100, 1000, 10_000)]
        public int N;
        private List<int> data;

        [GlobalSetup]
        public void ReduceSetup()
        {
            data = new List<int>(N);
            for (int i = 1; i <= N; i++)
            {
                data.Add(i);
            }
        }

        [Benchmark]
        public int LambdaReduce() => data.Aggregate((total, num) => total + num);

        [Benchmark]
        public int LoopReduce()
        {
            int total = 0;
            for (int i = 0; i < data.Count; i++)
            {
                total += data[i];
            }

            return total;
        }

        [Benchmark]
        public int IteratorReduce()
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
        [Params(100, 1000, 10_000)]
        public int N;
        private List<int> data;

        [GlobalSetup]
        public void PopulateSetup()
        {
            // Initialize pool of data to fill
            data = new List<int>(Enumerable.Range(1, N));
        }

        [Benchmark]
        public List<int> LambdaPopulate()
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
        [Params(100, 1000, 10_000)]
        public int N;
        private List<int> data;

        [GlobalSetup]
        public void IterateSetup()
        {
            data = new List<int>(Enumerable.Repeat(1, N));
        }

        [Benchmark]
        public int LambdaIterate() => data.Count(n => n > 0);

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
        [Params(100, 1000, 10_000)]
        public int N;
        private int target;
        private List<int> data;

        [GlobalSetup]
        public void ContainsSetup()
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
        public bool LambdaContains() => data.Any(n => n == target);

        [Benchmark]
        public bool LoopContains()
        {
            for (int i = 0; i < data.Count; i++)
            {
                if (data[i] == target) return true;
            }

            return false;
        }

        [Benchmark]
        public bool IteratorContains()
        {
            foreach (int value in data)
            {
                if (value == target) return true;
            }

            return false;
        }
    }

    [MemoryDiagnoser]
    public class FilterInt32
    {
        [Params(100, 1000, 10_000)]
        public int N;
        private List<int> data;
        private Consumer consumer;

        [GlobalSetup]
        public void FilterSetup()
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
        public void LambdaFilter() => data.Where(n => n >= 0).Consume(consumer);

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
        [Params(100, 1000, 10_000)]
        public int N;
        private List<int> data;

        [GlobalSetup]
        public void CopySetup()
        {
            data = new List<int>(Enumerable.Range(1, N));
        }

        [Benchmark]
        public List<int> LambdaCopy() => data.Select(n => n).ToList();

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
        [Params(100, 1000, 10_000)]
        public int N;
        private List<int> data;
        private Consumer consumer;

        [GlobalSetup]
        public void MapSetup()
        {
            consumer = new Consumer();
            data = new List<int>(Enumerable.Range(1, N));
        }

        [Benchmark]
        public void LambdaMap() => data.Select(n => n * n).Consume(consumer);

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
