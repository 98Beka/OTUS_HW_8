using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;

class Program {
    static void Main(string[] args) {
        var x1 = GetArray(100000);
        var x2 = GetArray(1000000);
        var x3 = GetArray(10000000);
        Check(x1);
        Check(x2);
        Check(x3);
        Console.Read();
    }

    private  static void Check(int[] x) {
        var timer = new Stopwatch();
        Console.WriteLine($"array lenght: {x.Length}");

        timer.Start();
        Sum(x);
        timer.Stop();
        Console.WriteLine("Simple " + timer.ElapsedMilliseconds);
        timer.Reset();


        timer.Start();
        ThreadSum(x);
        timer.Stop();
        Console.WriteLine("Thread " + timer.ElapsedMilliseconds);
        timer.Reset();

        timer.Start();
        ParallelSum(x);
        timer.Stop();
        Console.WriteLine("Parallel " + timer.ElapsedMilliseconds);

        Console.WriteLine();
    }

    private static long ParallelSum(int[] array) {
        var newArr = array.Select(i => (long)i).ToArray();
        var result = newArr.AsParallel().Sum();
        return result;
    }


    private static long ThreadSum(int[] array) {
        long result = 0;

        var splitArray = new List<int[]>();
        var z = array.Length / 10;

        for (int i = 0; i < 10; i++)
            splitArray.Add(array.Skip(z * i).Take(z).ToArray());

        List<Thread> threads = new List<Thread>();
        foreach (var tempArray in splitArray) {
            var thread = new Thread(() => result += Sum(tempArray));
            threads.Add(thread);
            thread.Start();
        }
        threads.ForEach(x => x.Join());

        return result;
    }

    private static long Sum(int[] array) {
        long result = 0;
        for (int i = 0; i < array.Length; i++) 
            result += array[i];
        return result;
    }

    private static int[] GetArray(int max) {
        var result = new int[max];
        for (int i = 1; i < max; i++)
            result[i] = i;
        return result;
    }
}
