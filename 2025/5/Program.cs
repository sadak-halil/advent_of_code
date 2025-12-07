using System;
using System.IO;
using System.Collections.Generic;

class Program
{
    static void Main()
    {
        var path = "input.txt";
        if (!File.Exists(path)) { Console.WriteLine("No input.txt"); return; }
        var text = File.ReadAllText(path).Replace("\r\n", "\n").TrimEnd('\n');
        var lines = text.Split('\n', StringSplitOptions.None);

        Console.WriteLine("Day 5 — Part 1: " + SolvePart1(lines));
        Console.WriteLine("Day 5 — Part 2: " + SolvePart2(lines));
    }

    static long SolvePart1(string[] lines)
    {
        var (ranges, ids) = ParseInput(lines);
        var merged = MergeRanges(ranges);
        long fresh = 0;
        foreach (var id in ids) if (IsFresh(merged, id)) fresh++;
        return fresh;
    }

    static (List<(long start, long end)> ranges, List<long> ids) ParseInput(string[] lines)
    {
        var ranges = new List<(long, long)>();
        var ids = new List<long>();
        int i = 0;
        for (; i < lines.Length; i++)
        {
            var s = (lines[i] ?? "").Trim();
            if (s == "") { i++; break; } // blank line separator
            var p = s.Split('-', StringSplitOptions.RemoveEmptyEntries);
            if (p.Length == 2 && long.TryParse(p[0], out var a) && long.TryParse(p[1], out var b))
                ranges.Add((Math.Min(a, b), Math.Max(a, b)));
        }
        for (; i < lines.Length; i++)
        {
            var s = (lines[i] ?? "").Trim();
            if (s == "") continue;
            if (long.TryParse(s, out var id)) ids.Add(id);
        }
        // Console.WriteLine($"Parsed {ranges.Count} ranges and {ids.Count} IDs.");
        // for (int j = 0; j < Math.Min(5, ranges.Count); j++)
        //     Console.WriteLine($"Range[{j}] = {ranges[j].Item1}-{ranges[j].Item2}");
        // for (int j = 0; j < Math.Min(5, ids.Count); j++)
        //     Console.WriteLine($"ID[{j}] = {ids[j]}");
        return (ranges, ids);
    }

    static List<(long start, long end)> MergeRanges(List<(long start, long end)> ranges)
    {
        var outList = new List<(long, long)>();
        if (ranges == null || ranges.Count == 0) return outList;
        ranges.Sort((x, y) => x.start.CompareTo(y.start));
        long curS = ranges[0].start, curE = ranges[0].end;
        for (int i = 1; i < ranges.Count; i++)
        {
            var (s, e) = ranges[i];
            if (s <= curE + 1) curE = Math.Max(curE, e); // merge overlapping/adjacent
            else { outList.Add((curS, curE)); curS = s; curE = e; }
        }
        outList.Add((curS, curE));
        return outList;
    }

    static bool IsFresh(List<(long start, long end)> merged, long id)
    {
        int lo = 0, hi = merged.Count - 1;
        while (lo <= hi)
        {
            int mid = (lo + hi) >> 1;
            var (s, e) = merged[mid];
            if (id < s) hi = mid - 1;
            else if (id > e) lo = mid + 1;
            else return true;
        }
        return false;
    }

    static long SolvePart2(string[] lines)
    {
        var (ranges, _) = ParseInput(lines);
        var merged = MergeRanges(ranges);
        long total = 0;
        foreach (var (s, e) in merged)
            total += (e - s + 1); // inclusive range length
        return total;
    }
}
