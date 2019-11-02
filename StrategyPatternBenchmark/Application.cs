using System;
using System.Collections.Generic;
using System.Text;

namespace StrategyPatternBenchmark
{
    // The Application defines the interface of interest to clients.
    class Application
    {
        // The Application maintains a reference to one of the Strategy objects. The
        // Application does not know the concrete class of a strategy. It should
        // work with all strategies via the Strategy interface.
        private IStrategy Strategy { get; set; }
        public List<string> Source { get; set; }

        public Application() { }

        // Usually, the Application accepts a strategy through the constructor, but
        // also provides a setter to change it at runtime.
        public Application(IStrategy strategy, List<string> source)
        {
            Strategy = strategy;
            Source = source;
        }

        // Usually, the Application allows replacing a Strategy object at runtime.
        public void SetStrategy(IStrategy strategy)
        {
            Strategy = strategy;
        }

        // The Application delegates some work to the Strategy object instead of
        // implementing multiple versions of the algorithm on its own.
        public void ExecuteStrategy()
        {
            Console.WriteLine("Application: Sorting data using the provided (unknown) strategy");
            var sorted = Strategy.SortAlgorithm(Source);

            Console.WriteLine(string.Join(", ", sorted));
        }
    }
}
