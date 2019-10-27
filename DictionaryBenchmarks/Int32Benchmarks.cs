using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;
using BenchmarkDotNet.Running;
using System.Collections.Generic;
using System.Linq;
using System;

namespace DictionaryBenchmarks
{
    [MemoryDiagnoser]
    public class ReduceInt32
    {
        private const int N = 1_000_000;
        private readonly Dictionary<int, int> data;

        public ReduceInt32()
        {
            data = new Dictionary<int, int>(N);
            for (int i = 0; i < N; i++)
            {
                data.Add(i, i * 10);
            }
        }

        [Benchmark]
        public int LinqAggregate()
        {
            int total = 0;
            return data.Aggregate(total, (total, kvp) => total += kvp.Value / kvp.Key);
        }

        [Benchmark]
        public int WhileLoopAggregate()
        {
            var iter = data.GetEnumerator();
            int total = 0;

            while (iter.MoveNext())
            {
                total += (iter.Current.Value / iter.Current.Key);
            }

            return total;
        }

        [Benchmark]
        public int ForEachAggregate()
        {
            int total = 0;
            foreach (var pair in data)
            {
                total += (pair.Value / pair.Key);
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
            data = new List<int>(Enumerable.Range(1, N));
        }

        [Benchmark]
        public Dictionary<int, int> LinqPopulate() => data.ToDictionary(k => k, v => v * 5);

        [Benchmark]
        public Dictionary<int, int> LoopPopulate()
        {
            var map = new Dictionary<int, int>(N);

            for (int i = 0; i < data.Count; i++)
            {
                if (!map.ContainsKey(data[i])) map.Add(data[i], data[i] * 5);
            }

            return map;
        }

        [Benchmark]
        public Dictionary<int, int> IteratorPopulate()
        {
            var map = new Dictionary<int, int>(N);

            foreach (int value in data)
            {
                if (!map.ContainsKey(value)) map.Add(value, value * 5);
            }

            return map;
        }
    }

    [MemoryDiagnoser]
    public class IterateInt32
    {
        private const int N = 1_000_000;
        private readonly Dictionary<int, int> data;

        public IterateInt32()
        {
            data = new Dictionary<int, int>(N);
            for (int i = 0; i < N; i++)
            {
                data.Add(i, i * 10);
            }
        }

        [Benchmark]
        public int LinqIterate() => data.Count(kvp => kvp.Value < int.MaxValue);

        [Benchmark]
        public int LoopIterate()
        {
            int count = 0;
            var iter = data.GetEnumerator();

            while (iter.MoveNext())
            {
                var kvp = iter.Current;
                if (kvp.Value < int.MaxValue) ++count;
            }

            return count;
        }

        [Benchmark]
        public int IteratorIterate()
        {
            int count = 0;
            foreach (var kvp in data)
            {
                if (kvp.Value < int.MaxValue) ++count;
            }

            return count;
        }
    }

    [MemoryDiagnoser]
    public class ContainsInt32
    {
        private const int N = 1_000_000;
        private readonly int target;
        private readonly Dictionary<int, int> data;

        public ContainsInt32()
        {
            var rnd = new Random();

            data = new Dictionary<int, int>(N);
            target = rnd.Next(-N, N);

            for (int i = 0; i < N; i++)
            {
                var value = rnd.Next(-N, N);
                data.Add(i, value);
            }
        }

        [Benchmark]
        public int LinqContains() => data.First(kvp => kvp.Value == target).Key;

        [Benchmark]
        public int LoopContains()
        {
            var iter = data.GetEnumerator();
            while (iter.MoveNext())
            {
                var kvp = iter.Current;
                if (kvp.Value == target) return iter.Current.Key;
            }

            return default;
        }

        [Benchmark]
        public int IteratorContains()
        {
            foreach (var kvp in data)
            {
                if (kvp.Value == target) return kvp.Key;
            }

            return default;
        }
    }

    [MemoryDiagnoser]
    public class FilterInt32
    {
        private const int N = 1_000_000;
        private readonly Dictionary<int, int> data;
        private readonly Consumer consumer;

        public FilterInt32()
        {
            consumer = new Consumer();
            data = new Dictionary<int, int>(N);

            for (int i = 0; i < N; i++)
            {
                data.Add(i, i * 10);
            }
        }

        [Benchmark]
        public void LinqFilter() => data.Where(kvp => kvp.Key % 2 == 0).Consume(consumer);

        [Benchmark]
        public void LoopFilter()
        {
            var result = new List<KeyValuePair<int, int>>();
            var iter = data.GetEnumerator();

            while (iter.MoveNext())
            {
                if (iter.Current.Key % 2 == 0)
                {
                    result.Add(iter.Current);
                }
            }

            result.Consume(consumer);
        }

        [Benchmark]
        public void IteratorFilter()
        {
            var result = new List<KeyValuePair<int, int>>();

            foreach (var kvp in data)
            {
                if (kvp.Key % 2 == 0) result.Add(kvp);
            }

            result.Consume(consumer);
        }
    }

    [MemoryDiagnoser]
    public class CopyInt32
    {
        private const int N = 1_000_000;
        private readonly Dictionary<int, int> data;

        public CopyInt32()
        {
            data = new Dictionary<int, int>(N);

            for (int i = 0; i < N; i++)
            {
                data.Add(i, i * 10);
            }
        }

        [Benchmark]
        public Dictionary<int, int> LinqCopy() => new Dictionary<int, int>(data.Select(kvp => kvp));

        [Benchmark]
        public Dictionary<int, int> LoopCopy()
        {
            var copy = new Dictionary<int, int>(data.Count);
            var iter = data.GetEnumerator();

            while (iter.MoveNext())
            {
                copy.Add(iter.Current.Key, iter.Current.Value);
            }

            return copy;
        }

        [Benchmark]
        public Dictionary<int, int> IteratorCopy()
        {
            var copy = new Dictionary<int, int>(data.Count);
            foreach (var kvp in data)
            {
                copy.Add(kvp.Key, kvp.Value);
            }

            return copy;
        }
    }

    [MemoryDiagnoser]
    public class MapInt32
    {
        private const int N = 1_000_000;
        private readonly Dictionary<int, int> data;
        private readonly Consumer consumer;

        public MapInt32()
        {
            consumer = new Consumer();
            data = new Dictionary<int, int>(N);

            for (int i = 0; i < N; i++)
            {
                data.Add(i, i * 10);
            }
        }

        [Benchmark]
        public void LinqMap() => data.ToDictionary(kvp => kvp.Key * 2, kvp => kvp.Value * 2).Consume(consumer);

        [Benchmark]
        public void LoopMap()
        {
            var result = new List<KeyValuePair<int, int>>(N);
            var iter = data.GetEnumerator();

            while (iter.MoveNext())
            {
                result.Add(new KeyValuePair<int, int>(iter.Current.Key * 2, iter.Current.Value * 2));
            }

            result.Consume(consumer);
        }

        [Benchmark]
        public void IteratorMap()
        {
            var result = new List<KeyValuePair<int, int>>(N);
            foreach (var kvp in data)
            {
                result.Add(new KeyValuePair<int, int>(kvp.Key * 2, kvp.Value * 2));
            }

            result.Consume(consumer);
        }
    }
}
