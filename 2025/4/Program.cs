using System;
using System.IO;
using System.Collections.Generic;

class Program
{
    static void Main(string[] args)
    {
        var inputPath = "input.txt";
        var lines = ReadInputLines(inputPath);
        Console.WriteLine("Day 4 — Part 1: " + SolvePart1(lines));
        Console.WriteLine("Day 4 — Part 2: " + SolvePart2(lines));
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

    // Part 1: count rolls accessible immediately (neighbor count < 4)
    static int SolvePart1(string[] lines)
    {
        if (lines == null || lines.Length == 0) return 0;

        int rows = lines.Length;
        int cols = 0;
        foreach (var l in lines) if (!string.IsNullOrEmpty(l) && l.Length > cols) cols = l.Length;
        if (cols == 0) return 0;

        var grid = new int[rows, cols];
        for (int i = 0; i < rows; i++)
        {
            var line = (lines[i] ?? string.Empty).TrimEnd('\r');
            for (int j = 0; j < cols; j++)
                grid[i, j] = (j < line.Length && line[j] == '@') ? 1 : 0;
        }

        int accessible = 0;
        for (int i = 0; i < rows; i++)
            for (int j = 0; j < cols; j++)
            {
                if (grid[i, j] == 0) continue;
                int neigh = 0;
                for (int di = -1; di <= 1; di++)
                    for (int dj = -1; dj <= 1; dj++)
                    {
                        if (di == 0 && dj == 0) continue;
                        int ni = i + di, nj = j + dj;
                        if (ni < 0 || ni >= rows || nj < 0 || nj >= cols) continue;
                        neigh += grid[ni, nj];
                    }
                if (neigh < 4) accessible++;
            }

        return accessible;
    }

    // Part 2: iteratively remove accessible rolls until none left; return total removed
    static int SolvePart2(string[] lines)
    {
        if (lines == null || lines.Length == 0) return 0;

        int rows = lines.Length;
        int cols = 0;
        foreach (var l in lines) if (!string.IsNullOrEmpty(l) && l.Length > cols) cols = l.Length;
        if (cols == 0) return 0;

        var grid = new int[rows, cols];
        for (int i = 0; i < rows; i++)
        {
            var line = (lines[i] ?? string.Empty).TrimEnd('\r');
            for (int j = 0; j < cols; j++)
                grid[i, j] = (j < line.Length && line[j] == '@') ? 1 : 0;
        }

        // initial neighbor counts (number of adjacent '@' excluding self)
        var neigh = new int[rows, cols];
        for (int i = 0; i < rows; i++)
            for (int j = 0; j < cols; j++)
            {
                if (grid[i, j] == 0) { neigh[i, j] = 0; continue; }
                int c = 0;
                for (int di = -1; di <= 1; di++)
                    for (int dj = -1; dj <= 1; dj++)
                    {
                        if (di == 0 && dj == 0) continue;
                        int ni = i + di, nj = j + dj;
                        if (ni < 0 || ni >= rows || nj < 0 || nj >= cols) continue;
                        c += grid[ni, nj];
                    }
                neigh[i, j] = c;
            }

        var q = new Queue<(int r, int c)>();
        for (int i = 0; i < rows; i++)
            for (int j = 0; j < cols; j++)
                if (grid[i, j] == 1 && neigh[i, j] < 4)
                    q.Enqueue((i, j));

        int removed = 0;
        while (q.Count > 0)
        {
            var (i, j) = q.Dequeue();
            if (grid[i, j] == 0) continue; // already removed
            // remove this roll
            grid[i, j] = 0;
            removed++;

            // decrement neighbor counts for adjacent cells; queue any that becomes accessible
            for (int di = -1; di <= 1; di++)
                for (int dj = -1; dj <= 1; dj++)
                {
                    if (di == 0 && dj == 0) continue;
                    int ni = i + di, nj = j + dj;
                    if (ni < 0 || ni >= rows || nj < 0 || nj >= cols) continue;
                    if (grid[ni, nj] == 0) continue;
                    neigh[ni, nj]--;
                    if (neigh[ni, nj] == 3) // just became <4
                        q.Enqueue((ni, nj));
                }
        }

        return removed;
    }
}