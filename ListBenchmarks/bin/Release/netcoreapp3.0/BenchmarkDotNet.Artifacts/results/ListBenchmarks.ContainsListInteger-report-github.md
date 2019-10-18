``` ini

BenchmarkDotNet=v0.11.5, OS=Windows 10.0.18362
AMD Ryzen 5 1600, 1 CPU, 12 logical and 6 physical cores
.NET Core SDK=3.0.100
  [Host]     : .NET Core 3.0.0 (CoreCLR 4.700.19.46205, CoreFX 4.700.19.46214), 64bit RyuJIT  [AttachedDebugger]
  DefaultJob : .NET Core 3.0.0 (CoreCLR 4.700.19.46205, CoreFX 4.700.19.46214), 64bit RyuJIT


```
|          Method |      Mean |     Error |    StdDev |
|---------------- |----------:|----------:|----------:|
|    LinqContains |  31.71 ns | 0.2403 ns | 0.2247 ns |
| ForLoopContains |  36.02 ns | 0.3090 ns | 0.2891 ns |
| ForEachContains | 217.75 ns | 1.3845 ns | 1.2951 ns |
