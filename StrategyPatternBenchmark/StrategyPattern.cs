using System.Collections.Generic;
using System;
using System.Linq;
using System.Diagnostics.CodeAnalysis;

namespace StrategyPatternBenchmark
{
    class StrategyPattern
    {
        class LengthComparison : Comparer<string>
        {
            public override int Compare(string x, string y)
            {
                return Math.Max(x.Length, y.Length);
            }
        }

        static void Main()
        {
            // The client code picks a concrete strategy and passes it to the
            // context. The client should be aware of the differences between
            // strategies in order to make the right choice.
            var source = new List<string>() { "Python", "CSharp", "Java", "Kotlin", "CPP", "Javascript",  "PHP" };
            var linejumps = string.Join("", Enumerable.Repeat('\n', 3));
            var application = new Application() { Source = source };

            Console.WriteLine("OBJECT-ORIENTED STRATEGY PATTERN");
            Console.WriteLine("Client: Strategy is set to normal sorting.");
            application.SetStrategy(new NormalSortStrategy());
            application.ExecuteStrategy();

            Console.WriteLine();

            Console.WriteLine("Client: Strategy is set to reverse sorting.");
            application.SetStrategy(new ReverseSortStrategy());
            application.ExecuteStrategy();

            Console.WriteLine($"{linejumps}FUNCTIONAL STRATEGY PATTERN");
            Console.WriteLine("Client: Strategy is set to normal sorting.");

            FunctionalStrategy.PrintSorted(source, n => n, new LengthComparison());
            Console.WriteLine();

            Console.WriteLine("Client: Strategy is set to reverse sorting.");
            FunctionalStrategy.PrintReversed(source, n => n, new LengthComparison());
            Console.WriteLine();
        }
    }
}
