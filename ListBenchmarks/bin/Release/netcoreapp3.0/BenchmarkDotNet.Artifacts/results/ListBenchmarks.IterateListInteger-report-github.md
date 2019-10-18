``` ini

BenchmarkDotNet=v0.11.5, OS=Windows 10.0.18362
Intel Core i7-7700HQ CPU 2.80GHz (Kaby Lake), 1 CPU, 8 logical and 4 physical cores
.NET Core SDK=3.0.100
  [Host]     : .NET Core 3.0.0 (CoreCLR 4.700.19.46205, CoreFX 4.700.19.46214), 64bit RyuJIT  [AttachedDebugger]
  DefaultJob : .NET Core 3.0.0 (CoreCLR 4.700.19.46205, CoreFX 4.700.19.46214), 64bit RyuJIT


```
|         Method |       Mean |      Error |    StdDev |
|--------------- |-----------:|-----------:|----------:|
|    LinqIterate | 1,882.7 us | 10.3756 us | 9.1977 us |
| ForLoopIterate |   267.7 us |  0.5792 us | 0.5134 us |
| ForEachIterate | 1,878.8 us |  3.5844 us | 2.7985 us |
