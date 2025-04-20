using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;

public class Program
{
    private static string _chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789\\|!\"`£$%€/()=?'ì^<>,;.:-_òç@èé[{+*]}ù§~&# " + '\t'.ToString();
    private static int _numChars = 2;
    private static long _elapsedMethod1, _elapsedMethod2, _elapsedMethod3, _elapsedMethod4;

    public static void Main()
    {
        Console.Title = "BruteforcePoC | Made by https://github.com/ZygoteCode/";

        long totalMethod1 = 0, totalMethod2 = 0, totalMethod3 = 0, totalMethod4 = 0;
        long minMethod1 = long.MaxValue, minMethod2 = long.MaxValue, minMethod3 = long.MaxValue, minMethod4 = long.MaxValue;
        long maxMethod1 = long.MinValue, maxMethod2 = long.MinValue, maxMethod3 = long.MinValue, maxMethod4 = long.MinValue;
        Process.GetCurrentProcess().PriorityClass = ProcessPriorityClass.RealTime;

        foreach (ProcessThread thread in Process.GetCurrentProcess().Threads)
        {
            thread.PriorityLevel = ThreadPriorityLevel.Highest;
        }

        BruteForceMethod1();
        BruteForceMethod1();
        BruteForceMethod2();
        BruteForceMethod3();
        BruteForceMethod4();

        for (int i = 0; i < 20; i++)
        {
            BruteForceMethod1();
            totalMethod1 += _elapsedMethod1;

            if (_elapsedMethod1 < minMethod1)
            {
                minMethod1 = _elapsedMethod1;
            }

            if (_elapsedMethod1 > maxMethod1)
            {
                maxMethod1 = _elapsedMethod1;
            }

            BruteForceMethod2();
            totalMethod2 += _elapsedMethod2;

            if (_elapsedMethod2 < minMethod2)
            {
                minMethod2 = _elapsedMethod2;
            }

            if (_elapsedMethod2 > maxMethod2)
            {
                maxMethod2 = _elapsedMethod2;
            }

            BruteForceMethod3();
            totalMethod3 += _elapsedMethod3;

            if (_elapsedMethod3 < minMethod3)
            {
                minMethod3 = _elapsedMethod3;
            }

            if (_elapsedMethod3 > maxMethod3)
            {
                maxMethod3 = _elapsedMethod3;
            }

            BruteForceMethod4();
            totalMethod4 += _elapsedMethod4;

            if (_elapsedMethod4 < minMethod4)
            {
                minMethod4 = _elapsedMethod4;
            }

            if (_elapsedMethod4 > maxMethod4)
            {
                maxMethod4 = _elapsedMethod4;
            }
        }

        totalMethod1 = totalMethod1 / 20;
        totalMethod2 = totalMethod2 / 20;
        totalMethod3 = totalMethod3 / 20;
        totalMethod4 = totalMethod4 / 20;

        Console.WriteLine($"METHOD 1\r\n\r\nAverage timing: {totalMethod1}ms\r\nBest time: {minMethod1}ms\r\nWorst time: {maxMethod1}ms\r\n");
        Console.WriteLine($"METHOD 2\r\n\r\nAverage timing: {totalMethod2}ms\r\nBest time: {minMethod2}ms\r\nWorst time: {maxMethod2}ms\r\n");
        Console.WriteLine($"METHOD 3\r\n\r\nAverage timing: {totalMethod3}ms\r\nBest time: {minMethod3}ms\r\nWorst time: {maxMethod3}ms\r\n");
        Console.WriteLine($"METHOD 4\r\n\r\nAverage timing: {totalMethod4}ms\r\nBest time: {minMethod4}ms\r\nWorst time: {maxMethod4}ms\r\n");

        Console.WriteLine("Press the ENTER key in order to exit from the program.");
        Console.ReadLine();
    }

    public static void BruteForceMethod1()
    {
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();

        IEnumerable<string> allCombinations =
            Enumerable
                .Range(1, _numChars)
                .SelectMany(N => Enumerable.Repeat(_chars, N).CartesianProduct())
                .Select(combination => new string(combination.ToArray()));

        foreach (string s in allCombinations)
        {
            Console.WriteLine(s);
        }

        stopwatch.Stop();
        _elapsedMethod1 = stopwatch.ElapsedMilliseconds;
    }

    public static void BruteForceMethod2()
    {
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();

        int[] indices = new int[_numChars];

        while (true)
        {
            string combination = new string(indices.Select(i => _chars[i]).ToArray());
            Console.WriteLine(combination);

            int index = _numChars - 1;
            while (index >= 0 && indices[index] == _chars.Length - 1)
            {
                indices[index] = 0;
                index--;
            }

            if (index < 0)
            {
                break;
            }

            indices[index]++;
        }

        stopwatch.Stop();
        _elapsedMethod2 = stopwatch.ElapsedMilliseconds;
    }

    public static void BruteForceMethod3()
    {
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();

        GenerateCombinations("", 0);

        stopwatch.Stop();
        _elapsedMethod3 = stopwatch.ElapsedMilliseconds;
    }

    public static void BruteForceMethod4()
    {
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();

        GenerateCombinations();

        stopwatch.Stop();
        _elapsedMethod4 = stopwatch.ElapsedMilliseconds;
    }

    private static void GenerateCombinations(string prefix, int level)
    {
        if (level == _numChars)
        {
            Console.WriteLine(prefix);
            return;
        }

        foreach (char c in _chars)
        {
            GenerateCombinations(prefix + c, level + 1);
        }
    }

    private static void GenerateCombinations()
    {
        long numCombinations = (long)BigInteger.Pow(_chars.Length, _numChars);

        for (long i = 0; i < numCombinations; i++)
        {
            string combination = GetCombinationByIndex(i);
            Console.WriteLine(combination);
        }
    }

    private static string GetCombinationByIndex(long index)
    {
        char[] result = new char[_numChars];

        for (int i = 0; i < _numChars; i++)
        {
            int charValue = (int)(index % _chars.Length);
            result[i] = _chars[charValue];
            index /= _chars.Length;
        }

        return new string(result);
    }
}

public static class Extensions
{
    public static IEnumerable<IEnumerable<T>> CartesianProduct<T>(this IEnumerable<IEnumerable<T>> sequences)
    {
        IEnumerable<IEnumerable<T>> emptyProduct = new[] { Enumerable.Empty<T>() };
        return sequences.Aggregate(emptyProduct, (accumulator, sequence) => from accseq in accumulator from item in sequence select accseq.Concat(new[] { item }));
    }
}
