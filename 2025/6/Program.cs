using System;
using System.IO;
using System.Numerics;
using System.Collections.Generic;
using System.Text;

class Program
{
    static void Main(string[] args)
    {
        var path = "input.txt";
        if (!File.Exists(path)) { Console.WriteLine("No input.txt"); return; }
        var lines = File.ReadAllLines(path);

        var total1 = Solve(lines, part2: false);
        Console.WriteLine("Day 6 — Part 1: " + total1);

        var total2 = Solve(lines, part2: true);
        Console.WriteLine("Day 6 — Part 2: " + total2);
    }

    static BigInteger Solve(string[] lines, bool part2)
    {
        if (lines == null || lines.Length == 0) return BigInteger.Zero;

        int rows = lines.Length;
        int cols = lines[0].Length;
        var grid = lines;

        var groups = FindGroups(grid, rows, cols);
        int opRow = rows - 1;

        BigInteger grandTotal = BigInteger.Zero;
        foreach (var (s, e) in groups)
        {
            char op = GetOperator(grid, opRow, s, e);
            if (op == '\0') continue;

            List<BigInteger> nums = part2 ? CollectNumbersPart2(grid, opRow, s, e) : CollectNumbersPart1(grid, opRow, s, e);
            if (nums.Count == 0) continue;

            BigInteger result = ApplyOperator(op, nums);
            grandTotal += result;
        }

        return grandTotal;
    }

    static List<(int s, int e)> FindGroups(string[] grid, int rows, int cols)
    {
        var isSpaceCol = new bool[cols];
        for (int c = 0; c < cols; c++)
        {
            bool allSpace = true;
            for (int r = 0; r < rows; r++)
            {
                if (grid[r][c] != ' ') { allSpace = false; break; }
            }
            isSpaceCol[c] = allSpace;
        }

        var groups = new List<(int s, int e)>();
        int ci = 0;
        while (ci < cols)
        {
            while (ci < cols && isSpaceCol[ci]) ci++;
            if (ci >= cols) break;
            int s = ci;
            while (ci < cols && !isSpaceCol[ci]) ci++;
            groups.Add((s, ci - 1));
        }
        return groups;
    }

    static char GetOperator(string[] grid, int opRow, int s, int e)
    {
        var opToken = grid[opRow].Substring(s, e - s + 1).Trim();
        if (string.IsNullOrEmpty(opToken)) return '\0';
        return opToken[0];
    }

    static List<BigInteger> CollectNumbersPart1(string[] grid, int opRow, int s, int e)
    {
        var nums = new List<BigInteger>();
        int w = e - s + 1;
        for (int r = 0; r < opRow; r++)
        {
            var token = grid[r].Substring(s, w).Trim();
            if (string.IsNullOrEmpty(token)) continue;
            var parts = token.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var p in parts)
            {
                if (BigInteger.TryParse(p, out var val)) nums.Add(val);
            }
        }
        return nums;
    }

    static List<BigInteger> CollectNumbersPart2(string[] grid, int opRow, int s, int e)
    {
        var nums = new List<BigInteger>();
        for (int c = e; c >= s; c--)
        {
            var sb = new StringBuilder();
            for (int r = 0; r < opRow; r++) sb.Append(grid[r][c]);
            var tok = sb.ToString().Trim();
            if (string.IsNullOrEmpty(tok)) continue;
            if (BigInteger.TryParse(tok, out var val)) nums.Add(val);
        }
        return nums;
    }

    static BigInteger ApplyOperator(char op, List<BigInteger> nums)
    {
        BigInteger result = (op == '+') ? BigInteger.Zero : BigInteger.One;
        if (op == '+')
        {
            foreach (var n in nums) result += n;
        }
        else
        {
            foreach (var n in nums) result *= n;
        }
        return result;
    }
}
