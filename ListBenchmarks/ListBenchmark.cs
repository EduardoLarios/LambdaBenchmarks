using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using System.Collections.Generic;
using System.Linq;
using System;

namespace ListBenchmarks
{
    public class ReduceList
    {
        private const int N = 1_000_000;
        private readonly List<int> data;

        public ReduceList()
        {
            data = new List<int>(Enumerable.Range(1, N));
        }

        [Benchmark]
        public int LinqAggregate() => data.Aggregate((total, num) => total + num);

        [Benchmark]
        public int ForLoopAggregate()
        {
            int total = 0;
            for (int i = 0; i < data.Count; i++) total += data[i];
            return total;
        }

        [Benchmark]
        public int ForEachAggregate()
        {
            int total = 0;
            foreach (int value in data) total += value;
            return total;
        }
    }

    public class PopulateList
    {
        private const int N = 1_000_000;
        private readonly List<int> fillValues;

        public PopulateList()
        {
            // Initialize pool of data to fill
            fillValues = new List<int>(Enumerable.Range(1, N));
        }

        [Benchmark]
        public List<int> LinqPopulate() => fillValues.Select(n => n).ToList();

        [Benchmark]
        public List<int> ForLoopPopulate()
        {
            var ForLoopData = new List<int>(N);
            for (int i = 0; i < fillValues.Count; i++)
            {
                ForLoopData.Add(fillValues[i]);
            }

            return ForLoopData;
        }

        [Benchmark]
        public List<int> ForEachPopulate()
        {
            var ForEachData = new List<int>(N);
            foreach (int value in fillValues)
            {
                ForEachData.Add(value);
            }

            return ForEachData;
        }
    }

    public class IterateList
    {
        private const int N = 1_000_000;
        private readonly List<int> iterateValues;
        private const int target = 1;

        public IterateList()
        {

            iterateValues = new List<int>(Enumerable.Repeat(1, N));
        }

        [Benchmark]
        public int LinqIterate() => iterateValues.Count(n => n == target);

        [Benchmark]
        public int ForLoopIterate()
        {
            int count = 0;
            for (int i = 0; i < iterateValues.Count; i++)
            {
                if (iterateValues[i] == target) ++count;
            }

            return count;
        }

        [Benchmark]
        public int ForEachIterate()
        {
            int count = 0;
            foreach (int value in iterateValues)
            {
                if(value == target) ++count;
            }

            return count;
        }
    }

    public class ContainsList
    {
        private const int N = 1_000_000;
        private readonly int target;
        private readonly List<int> iterateValues;

        public ContainsList()
        {
            var generator = new Random();
            iterateValues = new List<int>(N);
            target = generator.Next(1, 101);

            for (int i = 0; i < N; i++)
            {
                iterateValues.Add(generator.Next(1, 101));
            }
        }

        [Benchmark]
        public int LinqContains() => iterateValues.Find(n => n == target);

        [Benchmark]
        public int ForLoopContains()
        {
            for (int i = 0; i < iterateValues.Count; i++)
            {
                if (iterateValues[i] == target) return iterateValues[i];
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

    public class FilterList
    {
        private const int N = 1_000_000;
        private readonly List<int> filterValues;

        public FilterList()
        {
            var generator = new Random();
            filterValues = new List<int>(N);

            for (int i = 0; i < N; i++)
            {
                filterValues.Add(generator.Next(-100, 101));
            }
        }

        [Benchmark]
        public IEnumerable<int> LinqFilter() => filterValues.Where(n => n >= 0);

        [Benchmark]
        public IEnumerable<int> ForLoopFilter()
        {
            for (int i = 0; i < filterValues.Count; i++)
            {
                if (filterValues[i] >= 0) yield return filterValues[i];
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

    public class CopyList
    {
        private const int N = 1_000_000;
        private readonly List<int> valuesToCopy;

        public CopyList()
        {
            valuesToCopy = new List<int>(Enumerable.Range(1, N));
        }

        [Benchmark]
        public List<int> LinqCopy() => valuesToCopy.Select(n => n).ToList();

        [Benchmark]
        public List<int> ForLoopCopy()
        {
            var result = new List<int>(valuesToCopy.Count);
            for (int i = 0; i < valuesToCopy.Count; i++)
            {
                result.Add(valuesToCopy[i]);
            }

            return result;
        }

        [Benchmark]
        public List<int> ForEachCopy()
        {
            var result = new List<int>(valuesToCopy.Count);
            foreach (int value in valuesToCopy)
            {
                result.Add(value);
            }

            return result;
        }
    }

    public class MapList
    {
        private const int N = 1_000_000;
        private readonly List<int> originalValues;

        public MapList()
        {
            originalValues = new List<int>(Enumerable.Range(1, N));
        }

        [Benchmark]
        public IEnumerable<int> LinqMapInteger() => originalValues.Select(n => n * n);

        [Benchmark]
        public IEnumerable<int> ForLoopMapInteger()
        {
            for (int i = 0; i < originalValues.Count; i++)
            {
                var value = originalValues[i];
                yield return value * value;
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
            Console.WriteLine("--- LIST BENCHMARKS ---");
            BenchmarkRunner.Run<PopulateList>();
            BenchmarkRunner.Run<IterateList>();
            BenchmarkRunner.Run<ContainsList>();
            BenchmarkRunner.Run<CopyList>();
            BenchmarkRunner.Run<MapList>();
            BenchmarkRunner.Run<FilterList>();
            BenchmarkRunner.Run<ReduceList>();
        }
    }
}
