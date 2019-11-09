using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;
using BenchmarkDotNet.Running;
using System.Collections.Generic;
using System.Linq;
using System;

namespace HashSetBenchmarks
{
    [MemoryDiagnoser]
    public class ReduceInt64
    {
        [Params(100, 1000, 10_000)]
        public int N;
        private HashSet<long> data;

        [GlobalSetup]
        public void ReduceSetup()
        {
            data = new HashSet<long>(N);
            for (int i = 1; i <= N; i++)
            {
                data.Add(i * N);
            }
        }

        [Benchmark]
        public long LambdaReduce() => data.Aggregate((total, num) => total + num);

        [Benchmark]
        public long LoopReduce()
        {
            var iter = data.GetEnumerator();
            long total = 0;

            while (iter.MoveNext())
            {
                total += iter.Current;
            }

            return total;
        }

        [Benchmark]
        public long IteratorReduce()
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
        [Params(100, 1000, 10_000)]
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
        public HashSet<long> LambdaPopulate() => new HashSet<long>(data.Select(n => (long)n * N));

        [Benchmark]
        public HashSet<long> LoopPopulate()
        {
            var set = new HashSet<long>(N);

            for (int i = 0; i < data.Count; i++)
            {
                long latest = data[i] * N;
                if (!set.Contains(latest)) set.Add(latest);
            }

            return set;
        }

        [Benchmark]
        public HashSet<long> IteratorPopulate()
        {
            var set = new HashSet<long>(N);

            foreach (long value in data)
            {
                long latest = value * N;
                if (!set.Contains(latest)) set.Add(latest);
            }

            return set;
        }
    }

    [MemoryDiagnoser]
    public class IterateInt64
    {
        [Params(100, 1000, 10_000)]
        public int N;
        private HashSet<long> data;

        [GlobalSetup]
        public void IterateSetup()
        {
            data = new HashSet<long>(N);
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
            var iter = data.GetEnumerator();

            while (iter.MoveNext())
            {
                var value = iter.Current;
                if (value > 0) ++count;
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
        [Params(100, 1000, 10_000)]
        public int N;
        private long target;
        private HashSet<long> data;

        [GlobalSetup]
        public void ContainsSetup()
        {
            var rnd = new Random();
            var temp = rnd.Next(-N, N);

            data = new HashSet<long>(N);
            target = temp * temp;

            while (data.Count < N)
            {
                long value = rnd.Next(-N, N);
                if (!data.Contains(value)) data.Add(value);
            }
        }

        [Benchmark]
        public bool LambdaContains() => data.Any(n => (n * n) == target);

        [Benchmark]
        public bool LoopContains()
        {
            var iter = data.GetEnumerator();
            while (iter.MoveNext())
            {
                if (iter.Current * iter.Current == target) return true;
            }

            return false;
        }

        [Benchmark]
        public bool IteratorContains()
        {
            foreach (long n in data)
            {
                if (n * n == target) return true;
            }

            return false;
        }
    }

    [MemoryDiagnoser]
    public class FilterInt64
    {
        [Params(100, 1000, 10_000)]
        public int N;
        private HashSet<long> data;
        private Consumer consumer;

        [GlobalSetup]
        public void FilterSetup()
        {
            var rnd = new Random();

            consumer = new Consumer();
            data = new HashSet<long>(N);

            while (data.Count < N)
            {
                var value = rnd.Next(-N, N);
                if (!data.Contains(value)) data.Add(value);
            }
        }

        [Benchmark]
        public void LambdaFilter() => data.Where(n => n >= 0).Consume(consumer);

        [Benchmark]
        public void LoopFilter()
        {
            var result = new List<long>();
            var iter = data.GetEnumerator();

            while (iter.MoveNext())
            {
                if (iter.Current >= 0) result.Add(iter.Current);
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
        [Params(100, 1000, 10_000)]
        public int N;
        private HashSet<long> data;

        [GlobalSetup]
        public void CopySetup()
        {
            data = new HashSet<long>(N);
            for (int i = 1; i <= N; i++)
            {
                data.Add(i * N);
            }
        }

        [Benchmark]
        public HashSet<long> LambdaCopy() => new HashSet<long>(data.Select(n => n));

        [Benchmark]
        public HashSet<long> LoopCopy()
        {
            var copy = new HashSet<long>(data.Count);
            var iter = data.GetEnumerator();

            while (iter.MoveNext())
            {
                copy.Add(iter.Current);
            }

            return copy;
        }

        [Benchmark]
        public HashSet<long> IteratorCopy()
        {
            var copy = new HashSet<long>(data.Count);
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
        [Params(100, 1000, 10_000)]
        public int N;
        private HashSet<long> data;
        private Consumer consumer;

        [GlobalSetup]
        public void MapSetup()
        {
            consumer = new Consumer();
            data = new HashSet<long>(N);

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
            var iter = data.GetEnumerator();

            while (iter.MoveNext())
            {
                result.Add(iter.Current * 5);
            }

            result.Consume(consumer);
        }

        [Benchmark]
        public void IteratorMap()
        {
            var result = new List<long>(N);

            foreach (var value in data)
            {
                result.Add(value * 5);
            }

            result.Consume(consumer);
        }
    }
}
