public class Day1Tests
{
    [Fact]
    public void TestSampleInput()
    {
        string input = @"3   4
4   3
2   5
1   3
3   9
3   3";

        string result = Program.Solve(input);

        Assert.Equal("11", result);
    }

    [Fact]
    public void TestSolvePart2()
    {
        string input = @"3   4
4   3
2   5
1   3
3   9
3   3";

        string result = Program.SolvePart2(input);

        Assert.Equal("31", result);
    }
}