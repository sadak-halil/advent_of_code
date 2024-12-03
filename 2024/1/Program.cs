using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

public class Program
{
    public static void Main(string[] args)
    {
        Console.WriteLine($"************* Day 1 START *************");

        var p1 = PartOne("input.txt");
        var p2 = PartTwo("input.txt");

        Console.WriteLine($"Part 1 Result: {p1.result} \t: {p1.ms}ms");
        Console.WriteLine($"Part 2 Result: {p2.result} \t: {p2.ms}ms");
        Console.WriteLine($"************* Day 1 DONE *************");
    }

    public static (long result, double ms) PartOne(string file)
    {
        long result = 0;
        var sw = new Stopwatch();
        sw.Start();

        var (firstElements, secondElements) = ParseInput(File.ReadAllText(file));

        // Sort the lists
        firstElements.Sort();
        secondElements.Sort();

        // Calculate absolute differences
        var differences = new List<int>();
        for (int i = 0; i < firstElements.Count; i++)
        {
            differences.Add(Math.Abs(firstElements[i] - secondElements[i]));
        }

        // Sum the differences
        result = differences.Sum();

        sw.Stop();
        return (result, sw.Elapsed.TotalMilliseconds);
    }

    public static (long result, double ms) PartTwo(string file)
    {
        long result = 0;
        var sw = new Stopwatch();
        sw.Start();

        var (firstElements, secondElements) = ParseInput(File.ReadAllText(file));

        // Calculate similarity scores
        var similarityScores = new List<int>();
        foreach (var element in firstElements)
        {
            var count = secondElements.Count(e => e == element);
            similarityScores.Add(element * count);
        }

        // Sum the similarity scores
        result = similarityScores.Sum();

        sw.Stop();
        return (result, sw.Elapsed.TotalMilliseconds);
    }

    public static (List<int> firstElements, List<int> secondElements) ParseInput(string input)
    {
        var firstElements = new List<int>();
        var secondElements = new List<int>();

        // Split input into lines and parse each line
        var lines = input.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
        foreach (var line in lines)
        {
            var parts = line.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length == 2)
            {
                firstElements.Add(int.Parse(parts[0]));
                secondElements.Add(int.Parse(parts[1]));
            }
        }

        return (firstElements, secondElements);
    }
}