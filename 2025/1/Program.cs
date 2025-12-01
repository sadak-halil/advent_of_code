using System;
using System.IO;

class Program
{
    static void Main(string[] args)
    {
        var inputPath = "input.txt";
        var lines = ReadInputLines(inputPath);
        Console.WriteLine("Day 1 — Part 1: " + SolvePart1(lines));
        Console.WriteLine("Day 1 — Part 2: " + SolvePart2(lines));
    }

    static string[] ReadInputLines(string path)
    {
        if (!File.Exists(path))
        {
            Console.WriteLine($"No input file at '{path}' — create one next to Program.cs");
            return Array.Empty<string>();
        }
        return File.ReadAllLines(path);
    }

    static string SolvePart1(string[] lines)
    {
        long sum = 0;
        foreach (var l in lines)
            if (long.TryParse(l.Trim(), out var v)) sum += v;
        return sum.ToString();
    }

    static string SolvePart2(string[] lines)
    {
        return "TODO";
    }
}
