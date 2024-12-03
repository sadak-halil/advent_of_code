using System.Diagnostics;


public class Program
{
    public static void Main(string[] args)
    {
        Console.WriteLine($"************* Day 2 START *************");

        var p1 = PartOne("input.txt");
        var p2 = PartTwo("input.txt");

        Console.WriteLine($"Part 1 Result: {p1.result} \t: {p1.ms}ms");
        Console.WriteLine($"Part 2 Result: {p2.result} \t: {p2.ms}ms");
        Console.WriteLine($"************* Day 2 DONE *************");
    }

    public static (long result, double ms) PartOne(string file)
    {
        long result = 0;
        var sw = new Stopwatch();
        sw.Start();

        var reports = ParseText(File.ReadAllLines(file));

        foreach (var report in reports)
        {
            if (ProcessReport(report.Value))
            {
                result++;
            }
        }

        sw.Stop();
        return (result, sw.Elapsed.TotalMilliseconds);
    }

    public static (long result, double ms) PartTwo(string file)
    {
        long result = 0;
        var sw = new Stopwatch();
        sw.Start();

        var reports = ParseText(File.ReadAllLines(file));

        foreach (var report in reports)
        {
            if (CalculateScore(report.Value) == 1)
            {
                result++;
            }
        }

        sw.Stop();
        return (result, sw.Elapsed.TotalMilliseconds);
    }

    public static int CalculateScore(List<int> report)
    {
        // Score is 1 if the report is safe without modification
        if (ProcessReport(report))
        {
            return 1;
        }

        // Try removing each level and check if the modified report becomes safe
        for (int i = 0; i < report.Count; i++)
        {
            var tempReport = report.Where((_, index) => index != i).ToList();
            if (ProcessReport(tempReport))
            {
                return 1;
            }
        }

        return 0; // Unsafe even after trying all removals
    }

    public static bool ProcessReport(List<int> report)
    {
        // Check if the report is strictly increasing or decreasing
        var isAsc = report.SequenceEqual(report.OrderBy(x => x));
        var isDesc = report.SequenceEqual(report.OrderByDescending(x => x));

        if (!isAsc && !isDesc)
        {
            return false; // Not monotonic
        }

        // Check the difference constraints between adjacent levels
        for (int i = 0; i < report.Count - 1; i++)
        {
            var diff = report[i] - report[i + 1];
            if (Math.Abs(diff) < 1 || Math.Abs(diff) > 3)
            {
                return false; // Invalid difference
            }
        }

        return true; // Report passes all checks
    }

    public static Dictionary<string, List<int>> ParseText(string[] text)
    {
        var results = new Dictionary<string, List<int>>();

        foreach (var line in text)
        {
            var parts = line.Split(" ", StringSplitOptions.RemoveEmptyEntries)
                            .Select(int.Parse)
                            .ToList();
            results.Add(line, parts);
        }

        return results;
    }
}