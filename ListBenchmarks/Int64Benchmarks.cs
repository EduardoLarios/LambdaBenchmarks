using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;
using BenchmarkDotNet.Running;
using System.Collections.Generic;
using System.Linq;
using System;

namespace ListBenchmarks
{
    public class ReduceInt64
    {
        private const int N = 1_000_000;
        private readonly List<long> data;

        public ReduceInt64()
        {
            data = new List<long>(N);
            for (int i = 1; i <= N; i++)
            {
                data.Add(i * N);
            }
        }

        [Benchmark]
        public long LinqAggregate() => data.Aggregate((total, num) => total + num);

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

    public class PopulateInt64
    {
        private const int N = 1_000_000;
        private readonly List<int> data;

        public PopulateInt64()
        {
            data = new List<int>(N);
            for (int i = 1; i <= N; i++)
            {
                data.Add(i);
            }
        }

        [Benchmark]
        public List<long> LinqPopulate()
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

    public class IterateInt64
    {
        private const int N = 1_000_000;
        private readonly List<long> data;

        public IterateInt64()
        {
            data = new List<long>(N);
            for (int i = 1; i <= N; i++)
            {
                data.Add(i);
            }
        }

        [Benchmark]
        public long LinqIterate() => data.LongCount(n => n > 0);

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

    public class ContainsInt64
    {
        private const int N = 1_000_000;
        private readonly long target;
        private readonly List<long> data;

        public ContainsInt64()
        {
            var rnd = new Random();

            data = new List<long>(N);
            target = rnd.Next(1, 101) * N;

            while (data.Count < N)
            {
                data.Add(rnd.Next(1, 101) * N);
            }
        }

        [Benchmark]
        public long LinqContains() => data.Find(n => n == target);

        [Benchmark]
        public long LoopContains()
        {
            for (int i = 0; i < data.Count; i++)
            {
                if (data[i] == target) return data[i];
            }

            return default;
        }

        [Benchmark]
        public long IteratorContains()
        {
            foreach (long value in data)
            {
                if (value == target) return value;
            }

            return default;
        }
    }

    public class FilterInt64
    {
        private const int N = 1_000_000;
        private readonly List<long> data;
        private readonly Consumer consumer;

        public FilterInt64()
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
        public void LinqFilter() => data.Where(n => n >= 0).Consume(consumer);

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

    public class CopyInt64
    {
        private const int N = 1_000_000;
        private readonly List<long> data;

        public CopyInt64()
        {
            data = new List<long>(N);
            for (int i = 1; i <= N; i++)
            {
                data.Add(i * N);
            }
        }

        [Benchmark]
        public List<long> LinqCopy() => data.Select(n => n).ToList();

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

    public class MapInt64
    {
        private const int N = 1_000_000;
        private readonly List<long> data;
        private readonly Consumer consumer;

        public MapInt64()
        {
            consumer = new Consumer();
            data = new List<long>(N);

            for (int i = 1; i <= N; i++)
            {
                data.Add(i * N);
            }
        }

        [Benchmark]
        public void LinqMap() => data.Select(n => n * 5).Consume(consumer);

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
