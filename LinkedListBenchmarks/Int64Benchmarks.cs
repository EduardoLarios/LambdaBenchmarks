using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;
using BenchmarkDotNet.Running;
using System.Collections.Generic;
using System.Linq;
using System;

namespace LinkedListBenchmarks
{
    [MemoryDiagnoser]
    public class ReduceInt64
    {
        [Params(100, 1000, 10_000)]
        public int N;
        private LinkedList<long> data;

        [GlobalSetup]
        public void ReduceSetup()
        {
            data = new LinkedList<long>();
            for (int i = 1; i <= N; i++)
            {
                data.AddLast(i * N);
            }
        }

        [Benchmark]
        public long LambdaReduce() => data.Aggregate((total, num) => total + num);

        [Benchmark]
        public long LoopReduce()
        {
            long total = 0;
            var head = data.First;

            while (head != null)
            {
                total += head.Value;
                head = head.Next;
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
        private List<long> data;

        [GlobalSetup]
        public void PopulateSetup()
        {
            data = new List<long>(N);
            for (int i = 1; i <= N; i++)
            {
                data.Add(i);
            }
        }

        [Benchmark]
        public LinkedList<long> LambdaPopulate()
        {
            var rnd = new Random();
            return new LinkedList<long>(data.Select(_ => (long)rnd.Next(1, 101) * N));
        }

        [Benchmark]
        public LinkedList<long> LoopPopulate()
        {
            var rnd = new Random();
            var result = new LinkedList<long>();

            for (int i = 0; i < data.Count; i++)
            {
                result.AddLast(rnd.Next(1, 101) * N);
            }

            return result;
        }

        [Benchmark]
        public LinkedList<long> IteratorPopulate()
        {
            var rnd = new Random();
            var result = new LinkedList<long>();

            foreach (long value in data)
            {
                result.AddLast(rnd.Next(1, 101) * N);
            }

            return result;
        }
    }

    [MemoryDiagnoser]
    public class IterateInt64
    {
        [Params(100, 1000, 10_000)]
        public int N;
        private LinkedList<long> data;

        [GlobalSetup]
        public void IterateSetup()
        {
            data = new LinkedList<long>();
            for (int i = 1; i <= N; i++)
            {
                data.AddLast(i);
            }
        }

        [Benchmark]
        public long LambdaIterate() => data.LongCount(n => n > 0);

        [Benchmark]
        public long LoopIterate()
        {
            long counter = 0;
            var head = data.First;

            while (head != null)
            {
                if (head.Value > 0) ++counter;
                head = head.Next;
            }

            return counter;
        }

        [Benchmark]
        public long IteratorIterate()
        {
            long counter = 0;
            foreach (long value in data)
            {
                if (value > 0) ++counter;
            }

            return counter;
        }
    }

    [MemoryDiagnoser]
    public class ContainsInt64
    {
        [Params(100, 1000, 10_000)]
        public int N;
        private long target;
        private LinkedList<long> data;

        [GlobalSetup]
        public void ContainsSetup()
        {
            var rnd = new Random();

            data = new LinkedList<long>();
            target = rnd.Next(1, 101) * N;

            for (int i = 1; i <= N; i++)
            {
                data.AddLast(rnd.Next(1, 101) * N);
            }
        }

        [Benchmark]
        public bool LambdaContains() => data.Any(n => n == target);

        [Benchmark]
        public bool LoopContains()
        {
            var head = data.First;

            while (head != null)
            {
                if (head.Value == target) return true;
                head = head.Next;
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
        [Params(100, 1000, 10_000)]
        public int N;
        private LinkedList<long> data;
        private Consumer consumer;

        [GlobalSetup]
        public void FilterSetup()
        {
            var rnd = new Random();

            consumer = new Consumer();
            data = new LinkedList<long>();

            for (int i = 0; i < N; i++)
            {
                data.AddLast(rnd.Next(-N, N));
            }
        }

        [Benchmark]
        public void LambdaFilter() => data.Where(n => n >= 0).Consume(consumer);

        [Benchmark]
        public void LoopFilter()
        {
            var result = new List<long>();
            var head = data.First;

            while (head != null)
            {
                if (head.Value >= 0) result.Add(head.Value);
                head = head.Next;
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
        private LinkedList<long> data;

        [GlobalSetup]
        public void CopySetup()
        {
            data = new LinkedList<long>();
            for (int i = 1; i <= N; i++)
            {
                data.AddLast(i * N);
            }
        }

        [Benchmark]
        public LinkedList<long> LambdaCopy() => new LinkedList<long>(data.Select(n => n));

        [Benchmark]
        public LinkedList<long> LoopCopy()
        {
            var copy = new LinkedList<long>();
            var head = data.First;

            while (head != null)
            {
                copy.AddLast(head.Value);
                head = head.Next;
            }

            return copy;
        }

        [Benchmark]
        public LinkedList<long> IteratorCopy()
        {
            var copy = new LinkedList<long>();
            foreach (long value in data)
            {
                copy.AddLast(value);
            }

            return copy;
        }
    }

    [MemoryDiagnoser]
    public class MapInt64
    {
        [Params(100, 1000, 10_000)]
        public int N;
        private LinkedList<long> data;
        private Consumer consumer;

        [GlobalSetup]
        public void MapSetup()
        {
            consumer = new Consumer();
            data = new LinkedList<long>();

            for (int i = 1; i <= N; i++)
            {
                data.AddLast(i * N);
            }
        }

        [Benchmark]
        public void LambdaMap() => data.Select(n => n * 5).Consume(consumer);

        [Benchmark]
        public void LoopMap()
        {
            var result = new List<long>(N);
            var head = data.First;

            while (head != null)
            {
                result.Add(head.Value * 5);
                head = head.Next;
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
