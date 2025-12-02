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
        int pos = 50;
        int countAtZero = 0;

        foreach (var raw in lines)
        {
            var l = raw?.Trim();
            if (string.IsNullOrEmpty(l)) continue;

            char dir = l[0];
            if (!int.TryParse(l.Substring(1).Trim(), out var dist)) continue;

            dist %= 100; // reduces rotations larger than 100
            if (dir == 'L' || dir == 'l')
            {
                pos = (pos - dist) % 100;
            }
            else if (dir == 'R' || dir == 'r')
            {
                pos = (pos + dist) % 100;
            }
            else
            {
                continue;
            }

            if (pos < 0) pos += 100; // fix negative results
            if (pos == 0) countAtZero++;
        }

        return countAtZero.ToString();
    }

    static string SolvePart2(string[] lines)
    {
        int pos = 50;
        long countHits = 0;

        foreach (var line in lines)
        {
            var l = line?.Trim();
            if (string.IsNullOrEmpty(l)) continue;

            char direction = l[0];
            if (!long.TryParse(l.Substring(1).Trim(), out var dist)) continue;

            // compute how many times 0 is hit during this rotation
            if (direction == 'R' || direction == 'r')
            {
                // steps needed from current pos to first 0 when moving right
                int needed = (100 - pos) % 100;
                if (needed == 0) needed = 100;
                if (dist >= needed)
                    countHits += 1 + (dist - needed) / 100;
                pos = (int)((pos + dist) % 100);
            }
            else if (direction == 'L' || direction == 'l')
            {
                // steps needed from current pos to first 0 when moving left
                int needed = pos == 0 ? 100 : pos;
                if (dist >= needed)
                    countHits += 1 + (dist - needed) / 100;
                pos = (int)((pos - (dist % 100)) % 100);
                if (pos < 0) pos += 100;
            }
            else
            {
                continue;
            }
        }

        return countHits.ToString();
    }
}
