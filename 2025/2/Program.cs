using System;
using System.IO;
using System.Numerics;
using System.Collections.Generic;
using System.Text;

class Program
{
    static void Main(string[] args)
    {
        var inputPath = "input.txt";
        var lines = ReadInputLines(inputPath);
        Console.WriteLine("Day 2 — Part 1: " + SolvePart1(lines));
        Console.WriteLine("Day 2 — Part 2: " + SolvePart2(lines));
    }

    static string[] ReadInputLines(string path)
    {
        if (!File.Exists(path))
        {
            Console.WriteLine($"No input file at '{path}' — create one next to Program.cs");
            return Array.Empty<string>();
        }

        var text = File.ReadAllText(path).Trim();
        if (string.IsNullOrEmpty(text)) return Array.Empty<string>();

        // Input is a single long line of comma-separated ranges; return each range as an element.
        var parts = text.Split(',', StringSplitOptions.RemoveEmptyEntries);
        for (int i = 0; i < parts.Length; i++) parts[i] = parts[i].Trim();
        return parts;
    }

    static List<(long A, long B)> ParseRanges(string[] parts)
    {
        var ranges = new List<(long, long)>();
        foreach (var p in parts)
        {
            if (string.IsNullOrEmpty(p)) continue;
            var dash = p.IndexOf('-');
            if (dash < 0) continue;
            if (!long.TryParse(p.Substring(0, dash).Trim(), out var a)) continue;
            if (!long.TryParse(p.Substring(dash + 1).Trim(), out var b)) continue;
            if (a > b) continue;
            ranges.Add((a, b));
        }
        return ranges;
    }

    // Part 1: exact double-block (s repeated exactly twice)
    static string SolvePart1(string[] lines)
    {
        var ranges = ParseRanges(lines);
        var found = new HashSet<long>();
        foreach (var (A, B) in ranges)
        {
            for (long id = A; id <= B; id++)
            {
                var s = id.ToString();
                if ((s.Length & 1) == 1) continue; // odd length can't be double-block
                if (s[0] == '0') continue; // leading zero not allowed
                int half = s.Length / 2;
                if (s.Substring(0, half) == s.Substring(half, half))
                    found.Add(id);
            }
        }

        BigInteger sum = BigInteger.Zero;
        foreach (var v in found) sum += v;
        return sum.ToString();
    }

    // Part 2: any block repeated m>=2 times (string/divisors method)
    static string SolvePart2(string[] lines)
    {
        var ranges = ParseRanges(lines);
        var found = new HashSet<long>();
        foreach (var (A, B) in ranges)
        {
            for (long id = A; id <= B; id++)
            {
                var s = id.ToString();
                int L = s.Length;
                bool invalid = false;
                for (int k = 1; k <= L / 2 && !invalid; k++)
                {
                    if (L % k != 0) continue;
                    int m = L / k;
                    var block = s.Substring(0, k);
                    if (block[0] == '0') continue; // pattern cannot have leading zero
                    var sb = new StringBuilder(k * m);
                    for (int i = 0; i < m; i++) sb.Append(block);
                    if (sb.ToString() == s) invalid = true;
                }
                if (invalid) found.Add(id);
            }
        }

        BigInteger sum = BigInteger.Zero;
        foreach (var v in found) sum += v;
        return sum.ToString();
    }
}