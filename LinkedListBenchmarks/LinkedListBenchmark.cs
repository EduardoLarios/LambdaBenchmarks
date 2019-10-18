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
        private readonly LinkedList<int> list;

        public ReduceLinkedList()
        {
            list = new LinkedList<int>(Enumerable.Range(1, N));
        }

        [Benchmark]
        public int LinqAggregate() => list.Aggregate((total, num) => total + num);

        [Benchmark]
        public int ForLoopAggregate()
        {
            int total = 0;
            var head = list.First;

            for (int i = 0; i < list.Count; i++)
            {
                total += head.Value;
                head = head.Next;
            }

            return total;
        }

        [Benchmark]
        public int ForEachAggregate()
        {
            int total = 0;
            foreach (int value in list) total += value;
            return total;
        }
    }

    public class PopulateLinkedList
    {
        private const int N = 1_000_000;
        private readonly List<int> Range;

        public PopulateLinkedList()
        {
            // Initialize range to iterate
            Range = new List<int>(Enumerable.Range(1, N));
        }

        [Benchmark]
        public LinkedList<int> LinqPopulate()
        {
            var generator = new Random();
            return new LinkedList<int>(Range.Select(_ => generator.Next(1, 101)));
        }

        [Benchmark]
        public LinkedList<int> ForLoopPopulate()
        {
            var generator = new Random();
            var ForLoopData = new LinkedList<int>();

            for (int i = 0; i < Range.Count; i++)
            {
                ForLoopData.AddLast(generator.Next(1, 101));
            }

            return ForLoopData;
        }

        [Benchmark]
        public LinkedList<int> ForEachPopulate()
        {
            var generator = new Random();
            var ForEachData = new LinkedList<int>();

            foreach (int value in Range)
            {
                ForEachData.AddLast(generator.Next(1, 101));
            }

            return ForEachData;
        }
    }

    public class IterateLinkedList
    {
        private const int N = 1_000_000;
        private const int target = 1;
        private readonly LinkedList<int> iterateValues;

        public IterateLinkedList()
        {
            iterateValues = new LinkedList<int>(Enumerable.Repeat(1, N));
        }

        [Benchmark]
        public int LinqIterate() => iterateValues.Count(n => n == target);

        [Benchmark]
        public int ForLoopIterate()
        {
            int counter = 0;
            var head = iterateValues.First;
            for (int i = 0; i < iterateValues.Count; i++)
            {
                if (head.Value == target) ++counter;
                head = head.Next;
            }

            return counter;
        }

        [Benchmark]
        public int ForEachIterate()
        {
            int counter = 0;
            foreach (int value in iterateValues)
            {
                if(value == target) ++counter;
            }

            return counter;
        }
    }

    public class ContainsLinkedList
    {
        private const int N = 1_000_000;
        private readonly int target;
        private readonly LinkedList<int> iterateValues;

        public ContainsLinkedList()
        {
            var generator = new Random();
            iterateValues = new LinkedList<int>();
            target = generator.Next(1, 101);

            for (int i = 0; i < N; i++)
            {
                iterateValues.AddLast(generator.Next(1, 101));
            }
        }

        [Benchmark]
        public int LinqContains() => iterateValues.First(n => n == target);

        [Benchmark]
        public int ForLoopContains()
        {
            var head = iterateValues.First;
            for (int i = 0; i < iterateValues.Count; i++)
            {
                if (head.Value == target) return head.Value;
                head = head.Next;
            }

            return default;
        }

        [Benchmark]
        public int ForEachContains()
        {
            foreach (int value in iterateValues)
            {
                if (value == target) return value;
            }

            return default;
        }
    }

    public class FilterLinkedList
    {
        private const int N = 1_000_000;
        private readonly LinkedList<int> filterValues;

        public FilterLinkedList()
        {
            var generator = new Random();
            filterValues = new LinkedList<int>();

            for (int i = 0; i < N; i++)
            {
                filterValues.AddLast(generator.Next(-100, 101));
            }
        }

        [Benchmark]
        public IEnumerable<int> LinqFilter() => filterValues.Where(n => n >= 0);

        [Benchmark]
        public IEnumerable<int> ForLoopFilter()
        {
            var head = filterValues.First;
            for (int i = 0; i < filterValues.Count; i++)
            {
                if (head.Value >= 0) yield return head.Value;
                head = head.Next;
            }
        }

        [Benchmark]
        public IEnumerable<int> ForEachFilter()
        {
            foreach (int value in filterValues)
            {
                if (value >= 0) yield return value;
            }
        }
    }

    public class CopyLinkedList
    {
        private const int N = 1_000_000;
        private readonly LinkedList<int> valuesToCopy;

        public CopyLinkedList()
        {
            valuesToCopy = new LinkedList<int>(Enumerable.Range(1, N));
        }

        [Benchmark]
        public LinkedList<int> LinqCopy() => new LinkedList<int>(valuesToCopy.Select(n => n));

        [Benchmark]
        public LinkedList<int> ForLoopCopy()
        {
            var result = new LinkedList<int>();
            var head = valuesToCopy.First;

            for (int i = 0; i < valuesToCopy.Count; i++)
            {
                result.AddLast(head.Value);
                head = head.Next;
            }

            return result;
        }

        [Benchmark]
        public LinkedList<int> ForEachCopy()
        {
            var result = new LinkedList<int>();
            foreach (int value in valuesToCopy)
            {
                result.AddLast(value);
            }

            return result;
        }
    }

    public class MapLinkedList
    {
        private const int N = 1_000_000;
        private readonly LinkedList<int> originalValues;

        public MapLinkedList()
        {
            originalValues = new LinkedList<int>(Enumerable.Range(1, N));
        }

        [Benchmark]
        public IEnumerable<int> LinqMapInteger() => originalValues.Select(n => n * n);

        [Benchmark]
        public IEnumerable<int> ForLoopMapInteger()
        {
            var head = originalValues.First;
            for (int i = 0; i < originalValues.Count; i++)
            {
                yield return head.Value * head.Value;
                head = head.Next;
            }
        }

        [Benchmark]
        public IEnumerable<int> ForEachMapInteger()
        {
            foreach (var value in originalValues)
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
