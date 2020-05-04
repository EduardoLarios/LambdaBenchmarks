``` ini

BenchmarkDotNet=v0.12.0, OS=Windows 10.0.18363
Intel Core i7-7700HQ CPU 2.80GHz (Kaby Lake), 1 CPU, 8 logical and 4 physical cores
.NET Core SDK=3.0.100
  [Host]     : .NET Core 3.0.0 (CoreCLR 4.700.19.46205, CoreFX 4.700.19.46214), X64 RyuJIT
  DefaultJob : .NET Core 3.0.0 (CoreCLR 4.700.19.46205, CoreFX 4.700.19.46214), X64 RyuJIT


```
|           Method |   N |     Mean |     Error |    StdDev |  Gen 0 | Gen 1 | Gen 2 | Allocated |
|----------------- |---- |---------:|----------:|----------:|-------:|------:|------:|----------:|
|   LambdaPopulate | 100 | 3.538 us | 0.0381 us | 0.0357 us | 0.2823 |     - |     - |     896 B |
|     LoopPopulate | 100 | 3.309 us | 0.0316 us | 0.0296 us | 0.2327 |     - |     - |     736 B |
| IteratorPopulate | 100 | 3.449 us | 0.0502 us | 0.0469 us | 0.2327 |     - |     - |     736 B |
