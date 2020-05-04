using System.Collections.Generic;
using System.Linq;
using System;

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;

namespace ListBenchmarks
{
    [MemoryDiagnoser]
    public class ReduceInt64
    {
        [Params(100, 1000, 10_000, 100_000, 1_000_000)]
        public int N;
        private List<long> data;

        [GlobalSetup]
        public void ReduceSetup()
        {
            data = new List<long>(N);
            for (int i = 1; i <= N; i++)
            {
                data.Add(i * N);
            }
        }

        [Benchmark]
        public long LambdaAggresgate() => data.Aggregate((total, num) => total + num);

        [Benchmark]
        public long LoopAggregate()
        {
            long total = 0;
            for (int i = 0; i < data.Count; i++)
            {
                total += data[i];
            }

            return total;
        }

        [Benchmark]
        public long IteratorAggregate()
        {
            long total = 0;
            foreach (long value in data)
            {
                total += value;
            }

            return total;
        }
    }

    [MemoryDiagnoser]
    public class PopulateInt64
    {
        [Params(100, 1000, 10_000, 100_000, 1_000_000)]
        public int N;
        private List<int> data;

        [GlobalSetup]
        public void PopulateSetup()
        {
            data = new List<int>(N);
            for (int i = 1; i <= N; i++)
            {
                data.Add(i);
            }
        }

        [Benchmark]
        public List<long> LambdaPopulate()
        {
            var rnd = new Random();
            return data.Select(_ => (long)rnd.Next(1, 101) * N).ToList();
        }

        [Benchmark]
        public List<long> LoopPopulate()
        {
            var result = new List<long>(N);
            var rnd = new Random();

            for (int i = 0; i < data.Count; i++)
            {
                result.Add((long)rnd.Next(1, 101) * N);
            }

            return result;
        }

        [Benchmark]
        public List<long> IteratorPopulate()
        {
            var rnd = new Random();
            var result = new List<long>(N);

            foreach (long value in data)
            {
                result.Add((long)rnd.Next(1, 101) * N);
            }

            return result;
        }
    }

    [MemoryDiagnoser]
    public class IterateInt64
    {
        [Params(100, 1000, 10_000, 100_000, 1_000_000)]
        public int N;
        private List<long> data;

        [GlobalSetup]
        public void IterateSetup()
        {
            data = new List<long>(N);
            for (int i = 1; i <= N; i++)
            {
                data.Add(i);
            }
        }

        [Benchmark]
        public long LambdaIterate() => data.LongCount(n => n > 0);

        [Benchmark]
        public long LoopIterate()
        {
            long count = 0;
            for (int i = 0; i < data.Count; i++)
            {
                if (data[i] > 0) ++count;
            }

            return count;
        }

        [Benchmark]
        public long IteratorIterate()
        {
            long count = 0;
            foreach (long value in data)
            {
                if (value > 0) ++count;
            }

            return count;
        }
    }

    [MemoryDiagnoser]
    public class ContainsInt64
    {
        [Params(100, 1000, 10_000, 100_000, 1_000_000)]
        public int N;
        private long target;
        private List<long> data;

        [GlobalSetup]
        public void ContainsSetup()
        {
            var rnd = new Random();

            data = new List<long>(N);
            target = rnd.Next(1, 101) * N;

            for (int i = 1; i <= N; i++)
            {
                data.Add(rnd.Next(1, 101) * N);
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
            foreach (long value in data)
            {
                if (value == target) return true;
            }

            return false;
        }
    }

    [MemoryDiagnoser]
    public class FilterInt64
    {
        [Params(100, 1000, 10_000, 100_000, 1_000_000)]
        public int N;
        private List<long> data;
        private Consumer consumer;

        [GlobalSetup]
        public void FilterSetup()
        {
            var rnd = new Random();

            consumer = new Consumer();
            data = new List<long>(N);

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
            var result = new List<long>();

            for (int i = 0; i < data.Count; i++)
            {
                if (data[i] >= 0) result.Add(data[i]);
            }

            result.Consume(consumer);
        }

        [Benchmark]
        public void IteratorFilter()
        {
            var result = new List<long>();

            foreach (long value in data)
            {
                if (value >= 0) result.Add(value);
            }

            result.Consume(consumer);
        }
    }

    [MemoryDiagnoser]
    public class CopyInt64
    {
        [Params(100, 1000, 10_000, 100_000, 1_000_000)]
        public int N;
        private List<long> data;

        [GlobalSetup]
        public void CopySetup()
        {
            data = new List<long>(N);
            for (int i = 1; i <= N; i++)
            {
                data.Add(i * N);
            }
        }

        [Benchmark]
        public List<long> LambdaCopy() => data.Select(n => n).ToList();

        [Benchmark]
        public List<long> LoopCopy()
        {
            var copy = new List<long>(data.Count);
            for (int i = 0; i < data.Count; i++)
            {
                copy.Add(data[i]);
            }

            return copy;
        }

        [Benchmark]
        public List<long> IteratorCopy()
        {
            var copy = new List<long>(data.Count);
            foreach (long value in data)
            {
                copy.Add(value);
            }

            return copy;
        }
    }

    [MemoryDiagnoser]
    public class MapInt64
    {
        [Params(100, 1000, 10_000, 100_000, 1_000_000)]
        public int N;
        private List<long> data;
        private Consumer consumer;

        [GlobalSetup]
        public void MapSetup()
        {
            consumer = new Consumer();
            data = new List<long>(N);

            for (int i = 1; i <= N; i++)
            {
                data.Add(i * N);
            }
        }

        [Benchmark]
        public void LambdaMap() => data.Select(n => n * 5).Consume(consumer);

        [Benchmark]
        public void LoopMap()
        {
            var result = new List<long>(N);
            for (int i = 0; i < data.Count; i++)
            {
                result.Add(data[i] * 5);
            }

            result.Consume(consumer);
        }

        [Benchmark]
        public void IteratorMap()
        {
            var result = new List<long>(N);
            foreach (long value in data)
            {
                result.Add(value * 5);
            }

            result.Consume(consumer);
        }
    }
}
