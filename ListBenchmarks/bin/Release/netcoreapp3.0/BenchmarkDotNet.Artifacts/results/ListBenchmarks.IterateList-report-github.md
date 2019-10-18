``` ini

BenchmarkDotNet=v0.11.5, OS=Windows 10.0.18362
AMD Ryzen 5 1600, 1 CPU, 12 logical and 6 physical cores
.NET Core SDK=3.0.100
  [Host]     : .NET Core 3.0.0 (CoreCLR 4.700.19.46205, CoreFX 4.700.19.46214), 64bit RyuJIT
  DefaultJob : .NET Core 3.0.0 (CoreCLR 4.700.19.46205, CoreFX 4.700.19.46214), 64bit RyuJIT


```
|         Method |     Mean |     Error |    StdDev |
|--------------- |---------:|----------:|----------:|
|    LinqIterate | 9.313 ms | 0.1849 ms | 0.2469 ms |
| ForLoopIterate | 1.188 ms | 0.0160 ms | 0.0142 ms |
| ForEachIterate | 2.374 ms | 0.0361 ms | 0.0338 ms |
