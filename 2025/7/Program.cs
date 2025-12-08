using System;
using System.IO;
using System.Numerics;
using System.Collections.Generic;

class Program
{
    static void Main(string[] args)
    {
        var inputPath = "input.txt";
        if (!File.Exists(inputPath)) { Console.WriteLine("No input.txt"); return; }
        var grid = File.ReadAllLines(inputPath);

        long splits = CountSplits(grid);
        Console.WriteLine("Part 1 — split events: " + splits);

        BigInteger timelines = CountTimelines(grid);
        Console.WriteLine("Part 2 — timelines: " + timelines);
    }

    static long CountSplits(string[] grid)
    {
        int rows = grid.Length;
        int cols = grid[0].Length;

        int startRow = 0;
        int startCol = grid[0].IndexOf('S');

        var active = new HashSet<int> { startCol };
        long splits = 0;

        for (int r = startRow + 1; r < rows; r++)
        {
            var next = new HashSet<int>();
            var line = grid[r];
            foreach (var c in active)
            {
                if (c < 0 || c >= cols) continue;
                char ch = line[c];
                if (ch == '^')
                {
                    splits++;
                    if (c - 1 >= 0) next.Add(c - 1);
                    if (c + 1 < cols) next.Add(c + 1);
                }
                else if (ch == '.')
                {
                    next.Add(c);
                }
            }
            if (next.Count == 0) break;
            active = next;
        }

        return splits;
    }

    static BigInteger CountTimelines(string[] grid)
    {
        int rows = grid.Length;
        int cols = grid[0].Length;

        int startCol = grid[0].IndexOf('S');

        var cur = new BigInteger[cols];
        cur[startCol] = 1;

        for (int r = 1; r < rows; r++)
        {
            var next = new BigInteger[cols];
            var line = grid[r];
            for (int c = 0; c < cols; c++)
            {
                var n = cur[c];
                char ch = line[c];
                if (ch == '^')
                {
                    if (c - 1 >= 0) next[c - 1] += n;
                    if (c + 1 < cols) next[c + 1] += n;
                }
                else if (ch == '.')
                {
                    next[c] += n;
                }
            }
            bool any = false;
            for (int i = 0; i < cols; i++) if (!next[i].IsZero) { any = true; break; }
            if (!any) break;
            cur = next;
        }

        BigInteger total = BigInteger.Zero;
        for (int c = 0; c < cols; c++) total += cur[c];
        return total;
    }
}
