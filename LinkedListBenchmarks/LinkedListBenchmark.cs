using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using System.Collections.Generic;
using System.Linq;
using System;

namespace LinkedListBenchmarks
{
    public class ReduceLinkedList
    {
        private const int N = 1_000_000;
        private readonly LinkedList<int> data;

        public ReduceLinkedList()
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

    public class PopulateLinkedList
    {
        private const int N = 1_000_000;
        private readonly List<int> data;

        public PopulateLinkedList()
        {
            // Initialize range to iterate
            data = new List<int>(Enumerable.Range(1, N));
        }

        [Benchmark]
        public LinkedList<int> LinqPopulate()
        {
            var generator = new Random();
            return new LinkedList<int>(data.Select(_ => generator.Next(1, 101)));
        }

        [Benchmark]
        public LinkedList<int> LoopPopulate()
        {
            var generator = new Random();
            var list = new LinkedList<int>();

            for (int i = 0; i < data.Count; i++)
            {
                list.AddLast(generator.Next(1, 101));
            }

            return list;
        }

        [Benchmark]
        public LinkedList<int> IteratorPopulate()
        {
            var generator = new Random();
            var ForEachData = new LinkedList<int>();

            foreach (int value in data)
            {
                ForEachData.AddLast(generator.Next(1, 101));
            }

            return ForEachData;
        }
    }

    public class IterateLinkedList
    {
        private const int N = 1_000_000;
        private readonly LinkedList<int> data;

        public IterateLinkedList()
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
                if(head.Value > 0) ++counter;
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

    public class ContainsLinkedList
    {
        private const int N = 1_000_000;
        private readonly int target;
        private readonly LinkedList<int> data;

        public ContainsLinkedList()
        {
            var generator = new Random();

            data = new LinkedList<int>();
            target = generator.Next(1, 101);
            
            while(data.Count < N)
            {
                data.AddLast(generator.Next(1, 101));
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

    public class FilterLinkedList
    {
        private const int N = 1_000_000;
        private readonly LinkedList<int> data;

        public FilterLinkedList()
        {
            var generator = new Random();
            data = new LinkedList<int>();

            for (int i = 0; i < N; i++)
            {
                data.AddLast(generator.Next(-N, N));
            }
        }

        [Benchmark]
        public IEnumerable<int> LinqFilter() => data.Where(n => n >= 0);

        [Benchmark]
        public IEnumerable<int> LoopFilter()
        {
            var head = data.First;
            while(head != null)
            {
                if (head.Value >= 0) yield return head.Value;
                head = head.Next;
            }
        }

        [Benchmark]
        public IEnumerable<int> IteratorFilter()
        {
            foreach (int value in data)
            {
                if (value >= 0) yield return value;
            }
        }
    }

    public class CopyLinkedList
    {
        private const int N = 1_000_000;
        private readonly LinkedList<int> data;

        public CopyLinkedList()
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

            while(head != null)
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

    public class MapLinkedList
    {
        private const int N = 1_000_000;
        private readonly LinkedList<int> data;

        public MapLinkedList()
        {
            data = new LinkedList<int>(Enumerable.Range(1, N));
        }

        [Benchmark]
        public IEnumerable<int> LinqMap() => data.Select(n => n * n);

        [Benchmark]
        public IEnumerable<int> LoopMap()
        {
            var head = data.First;
            while(head != null)
            {
                yield return head.Value * head.Value;
                head = head.Next;
            }
        }

        [Benchmark]
        public IEnumerable<int> IteratorMap()
        {
            foreach (var value in data)
            {
                yield return value * value;
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("--- LINKED LIST BENCHMARKS ---");
            BenchmarkRunner.Run<PopulateLinkedList>();
            BenchmarkRunner.Run<IterateLinkedList>();
            BenchmarkRunner.Run<ContainsLinkedList>();
            BenchmarkRunner.Run<CopyLinkedList>();
            BenchmarkRunner.Run<MapLinkedList>();
            BenchmarkRunner.Run<FilterLinkedList>();
            BenchmarkRunner.Run<ReduceLinkedList>();
        }
    }
}
