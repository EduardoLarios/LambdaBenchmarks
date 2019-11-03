using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;
using BenchmarkDotNet.Running;
using System.Collections.Generic;
using System.Linq;
using System;

namespace LinkedListBenchmarks
{
    [MemoryDiagnoser]
    public class ReduceInt32
    {
        [Params(100, 1000, 10_000)]
        public int N;
        private LinkedList<int> data;
        
        [GlobalSetup]
        public void ReduceSetup()
        {
            data = new LinkedList<int>(Enumerable.Range(1, N));
        }

        [Benchmark]
        public int LinqAggregate() => data.Aggregate((total, num) => total + num);

        [Benchmark]
        public int LoopAggregate()
        {
            int total = 0;
            var head = data.First;

            while (head != null)
            {
                total += head.Value;
                head = head.Next;
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
        [Params(100, 1000, 10_000)]
        public int N;
        private List<int> data;

        [GlobalSetup]
        public void PopulateSetup()
        {
            // Initialize range to iterate
            data = new List<int>(Enumerable.Range(1, N));
        }

        [Benchmark]
        public LinkedList<int> LinqPopulate()
        {
            var rnd = new Random();
            return new LinkedList<int>(data.Select(_ => rnd.Next(1, 101)));
        }

        [Benchmark]
        public LinkedList<int> LoopPopulate()
        {
            var rnd = new Random();
            var list = new LinkedList<int>();

            for (int i = 0; i < data.Count; i++)
            {
                list.AddLast(rnd.Next(1, 101));
            }

            return list;
        }

        [Benchmark]
        public LinkedList<int> IteratorPopulate()
        {
            var rnd = new Random();
            var ForEachData = new LinkedList<int>();

            foreach (int value in data)
            {
                ForEachData.AddLast(rnd.Next(1, 101));
            }

            return ForEachData;
        }
    }

    [MemoryDiagnoser]
    public class IterateInt32
    {
        [Params(100, 1000, 10_000)]
        public int N;
        private LinkedList<int> data;

        [GlobalSetup]
        public void IterateSetup()
        {
            data = new LinkedList<int>(Enumerable.Repeat(1, N));
        }

        [Benchmark]
        public int LinqIterate() => data.Count(n => n > 0);

        [Benchmark]
        public int LoopIterate()
        {
            int counter = 0;
            var head = data.First;

            while (head != null)
            {
                if (head.Value > 0) ++counter;
                head = head.Next;
            }

            return counter;
        }

        [Benchmark]
        public int IteratorIterate()
        {
            int counter = 0;
            foreach (int value in data)
            {
                if (value > 0) ++counter;
            }

            return counter;
        }
    }

    [MemoryDiagnoser]
    public class ContainsInt32
    {
        [Params(100, 1000, 10_000)]
        public int N;
        private int target;
        private LinkedList<int> data;

        [GlobalSetup]
        public void ContainsSetup()
        {
            var rnd = new Random();

            data = new LinkedList<int>();
            target = rnd.Next(1, 101);

            while (data.Count < N)
            {
                data.AddLast(rnd.Next(1, 101));
            }
        }

        [Benchmark]
        public int LinqContains() => data.First(n => n == target);

        [Benchmark]
        public int LoopContains()
        {
            var head = data.First;

            while (head != null)
            {
                if (head.Value == target) return head.Value;
                head = head.Next;
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
        [Params(100, 1000, 10_000)]
        public int N;
        private LinkedList<int> data;
        private Consumer consumer;

        [GlobalSetup]
        public void FilterSetup()
        {
            var rnd = new Random();

            consumer = new Consumer();
            data = new LinkedList<int>();

            for (int i = 0; i < N; i++)
            {
                data.AddLast(rnd.Next(-N, N));
            }
        }

        [Benchmark]
        public void LinqFilter() => data.Where(n => n >= 0).Consume(consumer);

        [Benchmark]
        public void LoopFilter()
        {
            var result = new List<int>();
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
        private LinkedList<int> data;

        [GlobalSetup]
        public void CopySetup()
        {
            data = new LinkedList<int>(Enumerable.Range(1, N));
        }

        [Benchmark]
        public LinkedList<int> LinqCopy() => new LinkedList<int>(data.Select(n => n));

        [Benchmark]
        public LinkedList<int> LoopCopy()
        {
            var copy = new LinkedList<int>();
            var head = data.First;

            while (head != null)
            {
                copy.AddLast(head.Value);
                head = head.Next;
            }

            return copy;
        }

        [Benchmark]
        public LinkedList<int> IteratorCopy()
        {
            var copy = new LinkedList<int>();
            foreach (int value in data)
            {
                copy.AddLast(value);
            }

            return copy;
        }
    }

    [MemoryDiagnoser]
    public class MapInt32
    {
        [Params(100, 1000, 10_000)]
        public int N;
        private LinkedList<int> data;
        private Consumer consumer;

        [GlobalSetup]
        public void MapSetup()
        {
            consumer = new Consumer();
            data = new LinkedList<int>(Enumerable.Range(1, N));
        }

        [Benchmark]
        public void LinqMap() => data.Select(n => n * n).Consume(consumer);

        [Benchmark]
        public void LoopMap()
        {
            var result = new List<int>(N);
            var head = data.First;

            while (head != null)
            {
                result.Add(head.Value * head.Value);
                head = head.Next;
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
