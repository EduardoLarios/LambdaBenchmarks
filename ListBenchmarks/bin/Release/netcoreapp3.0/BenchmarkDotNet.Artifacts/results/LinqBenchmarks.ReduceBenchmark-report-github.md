``` ini

BenchmarkDotNet=v0.11.5, OS=Windows 10.0.18362
AMD Ryzen 5 1600, 1 CPU, 12 logical and 6 physical cores
.NET Core SDK=3.0.100
  [Host] : .NET Core 3.0.0 (CoreCLR 4.700.19.46205, CoreFX 4.700.19.46214), 64bit RyuJIT


```
|               Method | Mean | Error |
|--------------------- |-----:|------:|
|        LinqAggregate |   NA |    NA |
|       PLinqAggregate |   NA |    NA |
|     ForLoopAggregate |   NA |    NA |
| ForEachLoopAggregate |   NA |    NA |
|      ParallelForLoop |   NA |    NA |

Benchmarks with issues:
  ReduceBenchmark.LinqAggregate: DefaultJob
  ReduceBenchmark.PLinqAggregate: DefaultJob
  ReduceBenchmark.ForLoopAggregate: DefaultJob
  ReduceBenchmark.ForEachLoopAggregate: DefaultJob
  ReduceBenchmark.ParallelForLoop: DefaultJob