using System;
using System.IO;

class Program
{
    static void Main(string[] args)
    {
        var lines = File.ReadAllLines("input.txt");
        var pts = ParsePoints(lines);

        Console.WriteLine("Part 1: " + SolvePart1(pts));
        Console.WriteLine("Part 2: " + SolvePart2(pts));
    }

    static List<(int x, int y)> ParsePoints(string[] lines)
    {
        var list = new List<(int, int)>();
        foreach (var l in lines)
        {
            var p = l.Split(',', StringSplitOptions.RemoveEmptyEntries);
            if (int.TryParse(p[0].Trim(), out var x) && int.TryParse(p[1].Trim(), out var y))
                list.Add((x, y));
        }
        return list;
    }

    static long SolvePart1(List<(int x, int y)> points)
    {
        long best = 0;
        int n = points.Count;
        for (int i = 0; i < n; i++)
        {
            for (int j = i + 1; j < n; j++)
            {
                var (x1, y1) = points[i];
                var (x2, y2) = points[j];
                if (x1 == x2 || y1 == y2) continue; // skip lines
                long width = Math.Abs(x1 - x2) + 1;
                long height = Math.Abs(y1 - y2) + 1;
                long area = width * height;
                if (area > best) best = area;
            }
        }
        return best;
    }

    static long SolvePart2(List<(int x, int y)> redPoints)
    {
        int n = redPoints.Count;

        // collect unique Xs and Ys
        var xs = new SortedSet<int>();
        var ys = new SortedSet<int>();
        foreach (var (x, y) in redPoints) { xs.Add(x); ys.Add(y); }

        var xList = xs.ToList();
        var yList = ys.ToList();
        int W = xList.Count;
        int H = yList.Count;

        // Map original coordinate -> compressed index
        var xIndex = new Dictionary<int, int>();
        var yIndex = new Dictionary<int, int>();
        for (int i = 0; i < W; i++) xIndex[xList[i]] = i;
        for (int j = 0; j < H; j++) yIndex[yList[j]] = j;

        // allowed grid: true if red or green
        var allowed = new bool[H, W];

        // mark the red points as allowed
        foreach (var (x, y) in redPoints)
        {
            allowed[yIndex[y], xIndex[x]] = true;
        }

        // mark segments between consecutive red points (wrap)
        for (int i = 0; i < n; i++)
        {
            var (x1, y1) = redPoints[i];
            var (x2, y2) = redPoints[(i + 1) % n];
            if (x1 == x2)
            {
                int cx = xIndex[x1];
                int a = Math.Min(yIndex[y1], yIndex[y2]);
                int b = Math.Max(yIndex[y1], yIndex[y2]);
                for (int r = a; r <= b; r++) allowed[r, cx] = true;
            }
            else if (y1 == y2)
            {
                int cy = yIndex[y1];
                int a = Math.Min(xIndex[x1], xIndex[x2]);
                int b = Math.Max(xIndex[x1], xIndex[x2]);
                for (int c = a; c <= b; c++) allowed[cy, c] = true;
            }
        }

        // Fill interior of polygon (scanline on compressed rows)
        // Build edges in compressed coordinates (floating intersections not needed because coordinates align)
        var edges = new List<(int x1, int y1, int x2, int y2)>();
        for (int i = 0; i < n; i++)
        {
            var (x1, y1) = redPoints[i];
            var (x2, y2) = redPoints[(i + 1) % n];
            edges.Add((xIndex[x1], yIndex[y1], xIndex[x2], yIndex[y2]));
        }

        for (int row = 0; row < H; row++)
        {
            // find intersections of polygon edges with this compressed row (y == row)
            var inters = new List<int>();
            for (int e = 0; e < edges.Count; e++)
            {
                var (x1, y1, x2, y2) = edges[e];
                if (y1 == y2)
                {
                    if (y1 == row)
                    {
                        // whole horizontal edge lies on this row -> add endpoints
                        inters.Add(x1);
                        inters.Add(x2);
                    }
                }
                else
                {
                    // vertical edge — check if row between endpoints (inclusive low, exclusive high to avoid double count)
                    int a = Math.Min(y1, y2);
                    int b = Math.Max(y1, y2);
                    if (row >= a && row < b)
                    {
                        // vertical edge intersects at x = x1 (== x2)
                        inters.Add(x1);
                    }
                }
            }

            inters.Sort();
            for (int k = 0; k + 1 < inters.Count; k += 2)
            {
                int start = inters[k];
                int end = inters[k + 1];
                if (start > end) { var t = start; start = end; end = t; }
                for (int c = start; c <= end; c++) allowed[row, c] = true;
            }
        }

        // prefix sums over allowed
        var pref = new int[H + 1, W + 1];
        for (int r = 0; r < H; r++)
            for (int c = 0; c < W; c++)
                pref[r + 1, c + 1] = pref[r + 1, c] + pref[r, c + 1] - pref[r, c] + (allowed[r, c] ? 1 : 0);

        long best = 0;
        // Try all pairs of red points as opposite corners
        for (int i = 0; i < n; i++)
        {
            for (int j = i + 1; j < n; j++)
            {
                var (x1, y1) = redPoints[i];
                var (x2, y2) = redPoints[j];
                if (x1 == x2 || y1 == y2) continue;
                int c1 = Math.Min(xIndex[x1], xIndex[x2]);
                int c2 = Math.Max(xIndex[x1], xIndex[x2]);
                int r1 = Math.Min(yIndex[y1], yIndex[y2]);
                int r2 = Math.Max(yIndex[y1], yIndex[y2]);

                int allowedCount = pref[r2 + 1, c2 + 1] - pref[r1, c2 + 1] - pref[r2 + 1, c1] + pref[r1, c1];
                int totalCells = (r2 - r1 + 1) * (c2 - c1 + 1);
                if (allowedCount == totalCells)
                {
                    long width = Math.Abs(x1 - x2) + 1;
                    long height = Math.Abs(y1 - y2) + 1;
                    long area = width * height;
                    if (area > best) best = area;
                }
            }
        }

        return best;
    }
}
