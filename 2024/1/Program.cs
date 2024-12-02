public class Program
{
    public static void Main(string[] args)
    {
        string input = File.ReadAllText("input.txt");
        string part = args.Length > 0 ? args[0] : "1";
        string result = part == "2" ? SolvePart2(input) : Solve(input);

        Console.WriteLine($"Result: {result}");
    }
    public static (List<int>, List<int>) ParseInput(string input)
    {
        var lines = input.Split(['\r', '\n'], StringSplitOptions.RemoveEmptyEntries);
        var firstElements = new List<int>();
        var secondElements = new List<int>();

        foreach (var line in lines)
        {
            var parts = line.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length == 2)
            {
                firstElements.Add(int.Parse(parts[0]));
                secondElements.Add(int.Parse(parts[1]));
            }
        }

        return (firstElements, secondElements);
    }
    public static string Solve(string input)
    {
        var (firstElements, secondElements) = ParseInput(input);

        firstElements.Sort();
        secondElements.Sort();

        var differences = new List<int>();

        for (int i = 0; i < firstElements.Count; i++)
        {
            differences.Add(Math.Abs(firstElements[i] - secondElements[i]));
        }

        return differences.Sum().ToString();
    }


    public static string SolvePart2(string input)
    {
        var (firstElements, secondElements) = ParseInput(input);
        var similarityScores = new List<int>();

        foreach (var element in firstElements)
        {
            var count = secondElements.Count(e => e == element);
            similarityScores.Add(element * count);
        }

        var result = similarityScores.Sum().ToString();

        return result;
    }
}