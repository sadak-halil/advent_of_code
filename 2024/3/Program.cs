using System.Diagnostics;
using System.Text.RegularExpressions;

public class Program
{
    public static void Main(string[] args)
    {
        var p1 = PartOne("input.txt");
        var p2 = PartTwo("input.txt");

        Console.WriteLine($"Part One: {p1.result} \t: {p1.ms}ms");
        Console.WriteLine($"Part Two: {p2.result} \t: {p2.ms}ms");
    }
    public static string ParseInput(string file)
    {
        string result = string.Join("", File.ReadAllLines(file));

        return result;
    }

    public static (int result, double ms) PartOne(string inputFile)
    {
        var sw = new Stopwatch();
        sw.Start();
        var text = ParseInput(inputFile);

        int result = 0;

        string pattern = @"mul\((\d{1,3}),(\d{1,3})\)";

        Regex regex = new Regex(pattern);

        MatchCollection matches = regex.Matches(text);

        foreach (Match match in matches)
        {
            // Extract X and Y from the match groups
            int x = int.Parse(match.Groups[1].Value);
            int y = int.Parse(match.Groups[2].Value);

            // Multiply X and Y and add the product to the result
            result += x * y;
        }
        sw.Stop();
        return (result, sw.ElapsedMilliseconds);
    }

    public static (int result, double ms) PartTwo(string inputFile)
    {
        var sw = new Stopwatch();
        sw.Start();

        int result = 0;

        string input = ParseInput(inputFile);

        string mulPattern = @"mul\((\d{1,3}),(\d{1,3})\)";
        string controlPattern = @"\b(do|don't)\(\)";

        bool doState = true; // Current "do/don't" state
        int position = 0;

        while (position < input.Length)
        {
            var controlMatch = Regex.Match(input.Substring(position), controlPattern);
            if (controlMatch.Success && controlMatch.Index == 0)
            {
                doState = controlMatch.Value == "do()"; //true if it is do(), false if it is don't()
                position += controlMatch.Length; // Move position forward to the end of the matched pattern
                continue;
            }

            var mulMatch = Regex.Match(input.Substring(position), mulPattern);
            if (mulMatch.Success && mulMatch.Index == 0)
            {
                if (doState)
                {
                    int x = int.Parse(mulMatch.Groups[1].Value);
                    int y = int.Parse(mulMatch.Groups[2].Value);
                    result += x * y;
                }
                position += mulMatch.Length; // Move position forward
                continue;
            }

            // If no match is found, move one character forward
            position++;
        }

        sw.Stop();
        return (result, sw.ElapsedMilliseconds);
    }
}