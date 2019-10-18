``` ini

BenchmarkDotNet=v0.11.5, OS=Windows 10.0.18362
AMD Ryzen 5 1600, 1 CPU, 12 logical and 6 physical cores
.NET Core SDK=3.0.100
  [Host]     : .NET Core 3.0.0 (CoreCLR 4.700.19.46205, CoreFX 4.700.19.46214), 64bit RyuJIT  [AttachedDebugger]
  DefaultJob : .NET Core 3.0.0 (CoreCLR 4.700.19.46205, CoreFX 4.700.19.46214), 64bit RyuJIT


```
| Method |      Mean |     Error |    StdDev |
|------- |----------:|----------:|----------:|
| Sha256 |  6.190 us | 0.0254 us | 0.0225 us |
|    Md5 | 20.326 us | 0.1408 us | 0.1317 us |
