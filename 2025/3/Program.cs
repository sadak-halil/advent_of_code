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
        Console.WriteLine("Day 3 — Part 1: " + SolvePart1(lines));
        var part2 = SolvePart2(lines);
        Console.WriteLine("Day 3 — Part 2: " + part2.ToString());
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

        return text.Split('\n', StringSplitOptions.RemoveEmptyEntries);
    }

    static int SolvePart1(string[] lines)
    {
        if (lines == null || lines.Length == 0) return 0;
        const int K = 2;
        int total = 0;

        foreach (var raw in lines)
        {
            var s = (raw ?? "").Trim();
            if (s.Length < K) continue;

            // parse digits into ints
            var digits = new List<int>(s.Length);
            foreach (var ch in s) digits.Add(ch - '0');

            var stack = new List<int>(s.Length);
            int toRemove = digits.Count - K;
            foreach (var d in digits)
            {
                while (stack.Count > 0 && toRemove > 0 && stack[stack.Count - 1] < d)
                {
                    stack.RemoveAt(stack.Count - 1);
                    toRemove--;
                }
                stack.Add(d);
            }

            var best = stack.Count > K ? stack.GetRange(0, K).ToArray() : stack.ToArray();
            int val = best[0] * 10 + best[1];
            total += val;
        }

        return total;
    }

    static BigInteger SolvePart2(string[] lines)
    {
        if (lines == null || lines.Length == 0) return BigInteger.Zero;
        const int K = 12;
        BigInteger total = BigInteger.Zero;

        foreach (var raw in lines)
        {
            var s = (raw ?? "").Trim();
            if (s.Length < K) continue; // skip banks that cannot produce K digits

            // greedy stack to build max subsequence of length K
            var stack = new List<char>(K);
            int toRemove = s.Length - K; // how many digits we may drop
            foreach (var c in s)
            {
                while (stack.Count > 0 && toRemove > 0 && stack[stack.Count - 1] < c)
                {
                    stack.RemoveAt(stack.Count - 1);
                    toRemove--;
                }
                stack.Add(c);
            }

            // take the first K characters from the stack (may need to trim if we didn't remove enough)
            var bestChars = stack.Count > K ? stack.GetRange(0, K).ToArray() : stack.ToArray();
            var bestStr = new string(bestChars);

            // parse and add
            if (BigInteger.TryParse(bestStr, out var val))
                total += val;
        }

        return total;
    }
}
