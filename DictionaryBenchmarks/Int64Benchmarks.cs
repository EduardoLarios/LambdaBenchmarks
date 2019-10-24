﻿using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;
using BenchmarkDotNet.Running;
using System.Collections.Generic;
using System.Linq;
using System;

namespace DictionaryBenchmarks
{
    public class ReduceInt64
    {
        private const int N = 1_000_000;
        private readonly Dictionary<long, long> data;

        public ReduceInt64()
        {
            data = new Dictionary<long, long>(N);
            for (int i = 0; i < N; i++)
            {
                data.Add(i * 10, i * N);
            }
        }

        [Benchmark]
        public long LinqAggregate()
        {
            long total = 0;
            return data.Aggregate(total, (total, kvp) => total += kvp.Value / kvp.Key);
        }

        [Benchmark]
        public long WhileLoopAggregate()
        {
            var iter = data.GetEnumerator();
            long total = 0;

            while (iter.MoveNext())
            {
                total += (iter.Current.Value / iter.Current.Key);
            }

            return total;
        }

        [Benchmark]
        public long ForEachAggregate()
        {
            long total = 0;
            foreach (var pair in data)
            {
                total += (pair.Value / pair.Key);
            }

            return total;
        }
    }

    public class PopulateInt64
    {
        private const int N = 1_000_000;
        private readonly List<long> data;

        public PopulateInt64()
        {
            data = new List<long>(N);
            for (int i = 1; i <= N; i++)
            {
                data.Add(i * N);
            }
        }

        [Benchmark]
        public Dictionary<long, long> LinqPopulate() => data.ToDictionary(n => n, n => n * 5);

        [Benchmark]
        public Dictionary<long, long> LoopPopulate()
        {
            var map = new Dictionary<long, long>(N);

            for (int i = 0; i < data.Count; i++)
            {
                if (!map.ContainsKey(data[i])) map.Add(data[i], data[i] * 5);
            }

            return map;
        }

        [Benchmark]
        public Dictionary<long, long> IteratorPopulate()
        {
            var map = new Dictionary<long, long>(N);

            foreach (long value in data)
            {
                if (!map.ContainsKey(value)) map.Add(value, value * 5);
            }

            return map;
        }
    }

    public class IterateInt64
    {
        private const int N = 1_000_000;
        private readonly Dictionary<long, long> data;

        public IterateInt64()
        {
            data = new Dictionary<long, long>(N);
            for (int i = 0; i < N; i++)
            {
                data.Add(i, i * N);
            }
        }

        [Benchmark]
        public long LinqIterate() => data.LongCount(kvp => kvp.Value < long.MaxValue);

        [Benchmark]
        public long LoopIterate()
        {
            long count = 0;
            var iter = data.GetEnumerator();

            while (iter.MoveNext())
            {
                var kvp = iter.Current;
                if (kvp.Value < long.MaxValue) ++count;
            }

            return count;
        }

        [Benchmark]
        public long IteratorIterate()
        {
            long count = 0;
            foreach (var kvp in data)
            {
                if (kvp.Value < long.MaxValue) ++count;
            }

            return count;
        }
    }

    public class ContainsInt64
    {
        private const int N = 1_000_000;
        private readonly long target;
        private readonly Dictionary<long, long> data;

        public ContainsInt64()
        {
            var rnd = new Random();

            data = new Dictionary<long, long>(N);
            target = rnd.Next(-N, N);

            for (int i = 0; i < N; i++)
            {
                long value = rnd.Next(-N, N);
                data.Add(i, value);
            }
        }

        [Benchmark]
        public long LinqContains() => data.First(kvp => kvp.Value == target).Key;

        [Benchmark]
        public long LoopContains()
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
        public long IteratorContains()
        {
            foreach (var kvp in data)
            {
                if (kvp.Value == target) return kvp.Key;
            }

            return default;
        }
    }

    public class FilterInt64
    {
        private const int N = 1_000_000;
        private readonly Dictionary<long, long> data;
        private readonly Consumer consumer;

        public FilterInt64()
        {
            consumer = new Consumer();
            data = new Dictionary<long, long>(N);

            for (int i = 0; i < N; i++)
            {
                data.Add(i, i * N);
            }
        }

        [Benchmark]
        public void LinqFilter() => data.Where(kvp => kvp.Key % 2 == 0).Consume(consumer);

        [Benchmark]
        public void LoopFilter()
        {
            var result = new List<KeyValuePair<long, long>>();
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
            var result = new List<KeyValuePair<long, long>>();

            foreach (var kvp in data)
            {
                if (kvp.Key % 2 == 0) result.Add(kvp);
            }

            result.Consume(consumer);
        }
    }

    public class CopyInt64
    {
        private const int N = 1_000_000;
        private readonly Dictionary<long, long> data;

        public CopyInt64()
        {
            data = new Dictionary<long, long>(N);

            for (int i = 0; i < N; i++)
            {
                data.Add(i, i * 10);
            }
        }

        [Benchmark]
        public Dictionary<long, long> LinqCopy() => new Dictionary<long, long>(data.Select(kvp => kvp));

        [Benchmark]
        public Dictionary<long, long> LoopCopy()
        {
            var copy = new Dictionary<long, long>(data.Count);
            var iter = data.GetEnumerator();

            while (iter.MoveNext())
            {
                copy.Add(iter.Current.Key, iter.Current.Value);
            }

            return copy;
        }

        [Benchmark]
        public Dictionary<long, long> IteratorCopy()
        {
            var copy = new Dictionary<long, long>(data.Count);
            foreach (var kvp in data)
            {
                copy.Add(kvp.Key, kvp.Value);
            }

            return copy;
        }
    }

    public class MapInt64
    {
        private const int N = 1_000_000;
        private readonly Dictionary<long, long> data;
        private readonly Consumer consumer;

        public MapInt64()
        {
            consumer = new Consumer();
            data = new Dictionary<long, long>(N);

            for (int i = 0; i < N; i++)
            {
                data.Add(i, i * N);
            }
        }

        [Benchmark]
        public void LinqMap() => data.ToDictionary(kvp => kvp.Key * 2, kvp => kvp.Value * 2).Consume(consumer);

        [Benchmark]
        public void LoopMap()
        {
            var result = new List<KeyValuePair<long, long>>(N);
            var iter = data.GetEnumerator();

            while (iter.MoveNext())
            {
                result.Add(new KeyValuePair<long, long>(iter.Current.Key * 2, iter.Current.Value * 2));
            }

            result.Consume(consumer);
        }

        [Benchmark]
        public void IteratorMap()
        {
            var result = new List<KeyValuePair<long, long>>(N);
            foreach (var kvp in data)
            {
                result.Add(new KeyValuePair<long, long>(kvp.Key * 2, kvp.Value * 2));
            }

            result.Consume(consumer);
        }
    }
}
