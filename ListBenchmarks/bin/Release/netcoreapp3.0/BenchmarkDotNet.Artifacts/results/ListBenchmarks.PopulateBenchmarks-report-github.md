``` ini

BenchmarkDotNet=v0.11.5, OS=Windows 10.0.18362
AMD Ryzen 5 1600, 1 CPU, 12 logical and 6 physical cores
.NET Core SDK=3.0.100
  [Host]     : .NET Core 3.0.0 (CoreCLR 4.700.19.46205, CoreFX 4.700.19.46214), 64bit RyuJIT  [AttachedDebugger]
  DefaultJob : .NET Core 3.0.0 (CoreCLR 4.700.19.46205, CoreFX 4.700.19.46214), 64bit RyuJIT


```
|               Method |      Mean |     Error |    StdDev |
|--------------------- |----------:|----------:|----------:|
|         LinqPopulate |  3.794 ms | 0.0296 ms | 0.0277 ms |
| ParallelLinqPopulate | 20.601 ms | 0.3996 ms | 0.3925 ms |
