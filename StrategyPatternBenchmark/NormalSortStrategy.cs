using System;
using System.Collections.Generic;

namespace StrategyPatternBenchmark
{
    // Concrete Strategies implement the algorithm while following the base
    // Strategy interface. The interface makes them interchangeable in the
    // Application.
    class NormalSortStrategy : IStrategy
    {
        public List<string> SortAlgorithm(List<string> data)
        {
            data.Sort();
            return data;
        }
    }
}
