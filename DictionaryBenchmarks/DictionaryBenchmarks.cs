using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;
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
            public Dictionary<int, int> LinqPopulate() => new Dictionary<int, int>(data.Select(n => new KeyValuePair<int, int>(n, n * 5)));

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

        public class IterateDictionary
        {
            private const int N = 1_000_000;
            private readonly Dictionary<int, int> data;

            public IterateDictionary()
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

        public class ContainsDictionary
        {
            private const int N = 1_000_000;
            private readonly int target;
            private readonly Dictionary<int, int> data;

            public ContainsDictionary()
            {
                var generator = new Random();

                data = new Dictionary<int, int>(N);
                target = generator.Next(-N, N);

                for (int i = 0; i < N; i++)
                {
                    var value = generator.Next(-N, N);
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

        public class FilterDictionary
        {
            private const int N = 1_000_000;
            private readonly Dictionary<int, int> data;
            private readonly Consumer consumer;

            public FilterDictionary()
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
                var result = new List<KeyValuePair<int, int>>(N);
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
                var result = new List<KeyValuePair<int, int>>(N);

                foreach (var kvp in data)
                {
                    if (kvp.Key % 2 == 0) result.Add(kvp);
                }

                result.Consume(consumer);
            }
        }

        public class CopyDictionary
        {
            private const int N = 1_000_000;
            private readonly Dictionary<int, int> data;

            public CopyDictionary()
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

        public class MapDictionary
        {
            private const int N = 1_000_000;
            private readonly Dictionary<int, int> data;
            private readonly Consumer consumer;

            public MapDictionary()
            {
                consumer = new Consumer();
                data = new Dictionary<int, int>(N);

                for (int i = 0; i < N; i++)
                {
                    data.Add(i, i * 10);
                }
            }

            [Benchmark]
            public void LinqMap() => data.Select(kvp => new KeyValuePair<int, int>(kvp.Key * 2, kvp.Value * 2)).Consume(consumer);

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

        static void Main(string[] args)
        {
            Console.WriteLine("--- DICTIONARY BENCHMARKS ---");
            //BenchmarkRunner.Run<PopulateDictionary>();
            //BenchmarkRunner.Run<IterateDictionary>();
            //BenchmarkRunner.Run<ContainsDictionary>();
            //BenchmarkRunner.Run<CopyDictionary>();
            BenchmarkRunner.Run<MapDictionary>();
            BenchmarkRunner.Run<FilterDictionary>();
            BenchmarkRunner.Run<ReduceDictionary>();
        }
    }
}
