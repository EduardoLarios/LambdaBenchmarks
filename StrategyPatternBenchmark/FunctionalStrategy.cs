using System;
using System.Collections.Generic;
using System.Linq;

public static class FunctionalStrategy
{
	public static void PrintSorted<TKey>(List<string> source, Func<string, TKey> KeySelector, Comparer<TKey> comparer = null)
	{
        var result = source.OrderBy(KeySelector, comparer);
        var print = string.Join(", ", result);

        Console.WriteLine(print);
	}

    public static void PrintReversed<TKey>(List<string> source, Func<string, TKey> KeySelector, Comparer<TKey> comparer = null)
    {
        var result = source.OrderByDescending(KeySelector, comparer);
        var print = string.Join(", ", result);

        Console.WriteLine(print);
    }
}
