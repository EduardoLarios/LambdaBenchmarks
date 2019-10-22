using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using System.Collections.Generic;
using System.Linq;
using System;

namespace DictionaryBenchmarks
{
    class DictionaryBenchmarks
    {
        public class ReduceDictionary
        {
            private const int N = 1_000_000;
            private readonly Dictionary<int, int> data;

            public ReduceDictionary()
            {
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

        public class PopulateDictionary
        {
            private const int N = 1_000_000;
            private readonly List<int> data;

            public PopulateDictionary()
            {
                // Initialize pool of data to fill
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
                var current = 0;

                foreach (int value in data)
                {
                    var latest = value + current++;
                    if (!set.Contains(latest)) set.Add(latest);
                }

                return set;
            }
        }

        public class IterateDictionary
        {
            private const int N = 1_000_000;
            private readonly HashSet<int> data;

            public IterateDictionary()
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

        public class ContainsDictionary
        {
            private const int N = 1_000_000;
            private readonly int target;
            private readonly HashSet<int> data;

            public ContainsDictionary()
            {
                var generator = new Random();

                data = new HashSet<int>(N);
                target = (int)Math.Pow(generator.Next(-N, N), 2);

                while (data.Count < N)
                {
                    var value = generator.Next(-N, N);
                    if (!data.Contains(value)) data.Add(value);
                }
            }

            [Benchmark]
            public int LinqContains() => data.First(n => (int)Math.Pow(n, 2) == target);

            [Benchmark]
            public int LoopContains()
            {
                var iter = data.GetEnumerator();
                while (iter.MoveNext())
                {
                    var value = (int)Math.Pow(iter.Current, 2);
                    if (value == target) return iter.Current;
                }

                return default;
            }

            [Benchmark]
            public int IteratorContains()
            {
                foreach (int n in data)
                {
                    var value = (int)Math.Pow(n, 2);
                    if (value == target) return n;
                }

                return default;
            }
        }

        public class FilterDictionary
        {
            private const int N = 1_000_000;
            private readonly HashSet<int> data;

            public FilterDictionary()
            {
                var generator = new Random();
                data = new HashSet<int>(N);

                while (data.Count < N)
                {
                    var value = generator.Next(-N, N);
                    if (!data.Contains(value)) data.Add(value);
                }
            }

            [Benchmark]
            public IEnumerable<int> LinqFilter() => data.Where(n => n >= 0);

            [Benchmark]
            public IEnumerable<int> LoopFilter()
            {
                var iter = data.GetEnumerator();
                while (iter.MoveNext())
                {
                    if (iter.Current >= 0) yield return iter.Current;
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

        public class CopyDictionary
        {
            private const int N = 1_000_000;
            private readonly HashSet<int> data;

            public CopyDictionary()
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

        public class MapDictionary
        {
            private const int N = 1_000_000;
            private readonly HashSet<int> data;

            public MapDictionary()
            {
                data = new HashSet<int>(Enumerable.Range(1, N));
            }

            [Benchmark]
            public IEnumerable<int> LinqMap() => data.Select(n => n * n);

            [Benchmark]
            public IEnumerable<int> LoopMap()
            {
                var iter = data.GetEnumerator();
                while (iter.MoveNext())
                {
                    yield return iter.Current * iter.Current;
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

        static void Main(string[] args)
        {
            Console.WriteLine("--- DICTIONARY BENCHMARKS ---");
            BenchmarkRunner.Run<PopulateDictionary>();
            BenchmarkRunner.Run<IterateDictionary>();
            BenchmarkRunner.Run<ContainsDictionary>();
            BenchmarkRunner.Run<CopyDictionary>();
            BenchmarkRunner.Run<MapDictionary>();
            BenchmarkRunner.Run<FilterDictionary>();
            BenchmarkRunner.Run<ReduceDictionary>();
        }
    }
}
