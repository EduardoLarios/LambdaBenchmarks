using System;
using System.Collections.Generic;

namespace StrategyPatternBenchmark
{
    // The Strategy interface declares operations common to all supported
    // versions of some algorithm.
    //
    // The Context uses this interface to call the algorithm defined by Concrete
    // Strategies.
    public interface IStrategy
    {
        List<string> SortAlgorithm(List<string> data);
    }
}
