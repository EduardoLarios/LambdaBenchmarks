﻿using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;
using BenchmarkDotNet.Running;
using System.Collections.Generic;
using System.Linq;
using System;

namespace HashSetBenchmarks
{
    public class ReduceInt32
    {
        private const int N = 1_000_000;
        private readonly HashSet<int> data;

        public ReduceInt32()
        {
            data = new HashSet<int>(Enumerable.Range(1, N));
        }

        [Benchmark]
        public int LinqAggregate() => data.Aggregate((total, num) => total + num);

        [Benchmark]
        public int LoopAggregate()
        {
            var iter = data.GetEnumerator();
            int total = 0;

            while (iter.MoveNext())
            {
                total += iter.Current;
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

    public class PopulateInt32
    {
        private const int N = 1_000_000;
        private readonly List<int> data;

        public PopulateInt32()
        {
            data = new List<int>(Enumerable.Range(1, N));
        }

        [Benchmark]
        public HashSet<int> LinqPopulate() => new HashSet<int>(data.Select((n, index) => n + index));

        [Benchmark]
        public HashSet<int> LoopPopulate()
        {
            var set = new HashSet<int>(N);

            for (int i = 0; i < data.Count; i++)
            {
                var latest = data[i] + i;
                if (!set.Contains(latest)) set.Add(latest);
            }

            return set;
        }

        [Benchmark]
        public HashSet<int> IteratorPopulate()
        {
            var set = new HashSet<int>(N);
            int current = 0;

            foreach (int value in data)
            {
                var latest = value + current++;
                if (!set.Contains(latest)) set.Add(latest);
            }

            return set;
        }
    }

    public class IterateInt32
    {
        private const int N = 1_000_000;
        private readonly HashSet<int> data;

        public IterateInt32()
        {
            data = new HashSet<int>(Enumerable.Range(1, N));
        }

        [Benchmark]
        public int LinqIterate() => data.Count(n => n > 0);

        [Benchmark]
        public int LoopIterate()
        {
            int count = 0;
            var iter = data.GetEnumerator();

            while (iter.MoveNext())
            {
                var value = iter.Current;
                if (value > 0) ++count;
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

    public class ContainsInt32
    {
        private const int N = 1_000_000;
        private readonly int target;
        private readonly HashSet<int> data;

        public ContainsInt32()
        {
            var rnd = new Random();
            var temp = rnd.Next(-N, N);

            data = new HashSet<int>(N);
            target = temp * temp;

            while (data.Count < N)
            {
                var value = rnd.Next(-N, N);
                if (!data.Contains(value)) data.Add(value);
            }
        }

        [Benchmark]
        public int LinqContains() => data.First(n => (n * n) == target);

        [Benchmark]
        public int LoopContains()
        {
            var iter = data.GetEnumerator();
            while (iter.MoveNext())
            {
                var value = iter.Current * iter.Current;
                if (value == target) return iter.Current;
            }

            return default;
        }

        [Benchmark]
        public int IteratorContains()
        {
            foreach (int n in data)
            {
                var value = n * n;
                if (value == target) return n;
            }

            return default;
        }
    }

    public class FilterInt32
    {
        private const int N = 1_000_000;
        private readonly HashSet<int> data;
        private readonly Consumer consumer;

        public FilterInt32()
        {
            var rnd = new Random();

            consumer = new Consumer();
            data = new HashSet<int>(N);

            while (data.Count < N)
            {
                var value = rnd.Next(-N, N);
                if (!data.Contains(value)) data.Add(value);
            }
        }

        [Benchmark]
        public void LinqFilter() => data.Where(n => n >= 0).Consume(consumer);

        [Benchmark]
        public void LoopFilter()
        {
            var result = new List<int>();
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
            var result = new List<int>();

            foreach (int value in data)
            {
                if (value >= 0) result.Add(value);
            }

            result.Consume(consumer);
        }
    }

    public class CopyInt32
    {
        private const int N = 1_000_000;
        private readonly HashSet<int> data;

        public CopyInt32()
        {
            data = new HashSet<int>(Enumerable.Range(1, N));
        }

        [Benchmark]
        public HashSet<int> LinqCopy() => new HashSet<int>(data.Select(n => n));

        [Benchmark]
        public HashSet<int> LoopCopy()
        {
            var copy = new HashSet<int>(data.Count);
            var iter = data.GetEnumerator();

            while (iter.MoveNext())
            {
                copy.Add(iter.Current);
            }

            return copy;
        }

        [Benchmark]
        public HashSet<int> IteratorCopy()
        {
            var copy = new HashSet<int>(data.Count);
            foreach (int value in data)
            {
                copy.Add(value);
            }

            return copy;
        }
    }

    public class MapInt32
    {
        private const int N = 1_000_000;
        private readonly HashSet<int> data;
        private readonly Consumer consumer;

        public MapInt32()
        {
            consumer = new Consumer();
            data = new HashSet<int>(Enumerable.Range(1, N));
        }

        [Benchmark]
        public void LinqMap() => data.Select(n => n * n).Consume(consumer);

        [Benchmark]
        public void LoopMap()
        {
            var result = new List<int>(N);
            var iter = data.GetEnumerator();

            while (iter.MoveNext())
            {
                result.Add(iter.Current * iter.Current);
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