# LambdaBenchmarks
Benchmarks for research of functional programming interfaces in OOP at ITESM CQ

# How to run
To run each benchmark:

```bash
cd to directory, i.e:
cd ListBenchmarks

dotnet run --release
```

# Console Arguments
# Filter
The --filter or just -f allows you to filter the benchmarks by their full name (namespace.typeName.methodName) using glob patterns.

Examples:

+ Run all benchmarks from System.Memory namespace: -f System.Memory*
+ Run all benchmarks: -f *
+ Run all benchmarks from ClassA and ClassB -f *ClassA* *ClassB*

Note: If you would like to join all the results into a single summary, you need to us --join.

+ List of benchmarks
The --list allows you to print all of the available benchmark names. Available options are:
```
flat - prints list of the available benchmarks: --list flat
tree - prints tree of the available benchmarks: --list tree
```

# Diagnosers
```
-m, --memory - enables MemoryDiagnoser and prints memory statistics
-d, --disasm- enables DisassemblyDiagnoser and exports diassembly of benchmarked code. When you enable this option, you can use:
--disasmDepth - Sets the recursive depth for the disassembler.
--disasmDiff - Generates diff reports for the disassembler.
```

# Runtimes
The --runtimes or just -r allows you to run the benchmarks for selected Runtimes. Available options are:

+ Clr - BDN will either use Roslyn (if you run it as .NET app) or latest installed .NET SDK to build the benchmarks (if you run it as .NET Core app)

+ Core - if you run it as .NET Core app, BDN will use the same target framework moniker, if you run it as .NET app it's going to use netcoreapp2.1

+ Mono - it's going to use the Mono from $Path, you can override it with --monoPath

+ CoreRT - it's going to use latest CoreRT. Can be customized with additional options: --ilcPath, --coreRtVersion net46, net461, net462, net47, net471, net472 - to build and run benchmarks against specific .NET framework version netcoreapp2.0, netcoreapp2.1, netcoreapp2.2, netcoreapp3.0 - to build and run benchmarks against specific .NET Core version

Example: run the benchmarks for .NET 4.7.2 and .NET Core 2.1:
```
dotnet run -c Release -- --runtimes net472 netcoreapp2.1
Example: run the benchmarks for .NET Core 3.0 and latest .NET SDK installed on your PC:

dotnet run -c Release -f netcoreapp3.0 -- --runtimes clr core
But same command executed with -f netcoreapp2.0 is going to run the benchmarks for .NET Core 2.0:

dotnet run -c Release -f netcoreapp2.0 -- --runtimes clr core
```

# Number of invocations and iterations
+ --launchCount - how many times we should launch process with target benchmark. The default is 1.
+ --warmupCount - how many warmup iterations should be performed. If you set it, the minWarmupCount and maxWarmupCount are ignored. By default calculated by the heuristic.
+ --minWarmupCount - minimum count of warmup iterations that should be performed. The default is 6.
+ --maxWarmupCount - maximum count of warmup iterations that should be performed. The default is 50.
+ --iterationTime - desired time of execution of an iteration. Used by Pilot stage to estimate the number of invocations per iteration. 500ms by default.
+ --iterationCount - how many target iterations should be performed. By default calculated by the heuristic.
+ --minIterationCount - minimum number of iterations to run. The default is 15.
+ --maxIterationCount - maximum number of iterations to run. The default is 100.
+ --invocationCount - invocation count in a single iteration. By default calculated by the heuristic.
+ --unrollFactor - how many times the benchmark method will be invoked per one iteration of a generated loop. 16 by default
+ --runOncePerIteration - run the benchmark exactly once per iteration. False by default.

Example: run single warmup iteration, from 9 to 12 actual workload iterations.
```
dotnet run -c Release -- --warmupCount 1 --minIterationCount 9 --maxIterationCount 12
```
