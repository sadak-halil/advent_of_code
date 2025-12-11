using System;
using System.IO;

class Program
{
    static void Main(string[] args)
    {
        var inputPath = "input.txt";
        if (!File.Exists(inputPath))
        {
            Console.WriteLine($"No input file at '{inputPath}' — create one next to Program.cs");
            return;
        }

        var text = File.ReadAllText(inputPath).Trim();
        var lines = text.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

        var junctionBoxes = ParseCoords(lines);

        int pairsToConnect = 1000;
        if (args.Length > 0 && int.TryParse(args[0], out var parsed)) pairsToConnect = parsed;

        var prod = SolvePart1(junctionBoxes, pairsToConnect);
        Console.WriteLine("Day 8 — Part 1: " + prod);

        var part2 = SolvePart2(junctionBoxes);
        Console.WriteLine("Day 8 — Part 2: " + part2);
    }

    static List<(long x, long y, long z)> ParseCoords(string[] lines)
    {
        var list = new List<(long, long, long)>();
        foreach (var l in lines)
        {
            var p = l.Split(',', StringSplitOptions.RemoveEmptyEntries);
            if (long.TryParse(p[0].Trim(), out var x) && long.TryParse(p[1].Trim(), out var y) && long.TryParse(p[2].Trim(), out var z))
                list.Add((x, y, z));
        }
        return list;
    }

    static long SolvePart1(List<(long x, long y, long z)> junctionBoxes, int pairsToConnect)
    {
        int n = junctionBoxes.Count;
        if (n == 0) return 0;

        var pairs = new List<(long dist, int a, int b)>();
        pairs.Capacity = n * (n - 1) / 2;
        for (int i = 0; i < n; i++)
            for (int j = i + 1; j < n; j++)
            {
                var dx = junctionBoxes[i].x - junctionBoxes[j].x;
                var dy = junctionBoxes[i].y - junctionBoxes[j].y;
                var dz = junctionBoxes[i].z - junctionBoxes[j].z;
                long d2 = dx * dx + dy * dy + dz * dz;
                pairs.Add((d2, i, j));
            }

        pairs.Sort((u, v) => u.dist.CompareTo(v.dist));

        var circuits = new CircuitDSU(n);
        int taken = 0;
        foreach (var (dist, a, b) in pairs)
        {
            circuits.Union(a, b);
            taken++;
            if (taken >= pairsToConnect) break;
        }

        var sizes = new List<int>(n);
        for (int i = 0; i < n; i++) if (circuits.Find(i) == i) sizes.Add(circuits.Size(i));
        sizes.Sort((x, y) => y.CompareTo(x));
        // component sizes are collected below
        while (sizes.Count < 3) sizes.Add(1);
        long prod = 1;
        prod *= sizes[0];
        prod *= sizes[1];
        prod *= sizes[2];
        return prod;
    }

    static long SolvePart2(List<(long x, long y, long z)> junctionBoxes)
    {
        int n = junctionBoxes.Count;
        if (n == 0) return 0;

        var pairs = new List<(long dist, int a, int b)>();
        pairs.Capacity = n * (n - 1) / 2;
        for (int i = 0; i < n; i++)
            for (int j = i + 1; j < n; j++)
            {
                var dx = junctionBoxes[i].x - junctionBoxes[j].x;
                var dy = junctionBoxes[i].y - junctionBoxes[j].y;
                var dz = junctionBoxes[i].z - junctionBoxes[j].z;
                long d2 = dx * dx + dy * dy + dz * dz;
                pairs.Add((d2, i, j));
            }

        pairs.Sort((u, v) => u.dist.CompareTo(v.dist));

        var circuits = new CircuitDSU(n);
        int components = n;
        foreach (var (dist, a, b) in pairs)
        {
            // if union returns true, two different circuits merged
            if (circuits.Union(a, b))
            {
                components--;
                if (components == 1)
                {
                    // return product of X coordinates of these two junction boxes
                    return junctionBoxes[a].x * junctionBoxes[b].x;
                }
            }
        }

        return 0;
    }

    class DSU
    {
        int[] parent, size;
        public DSU(int n) { parent = new int[n]; size = new int[n]; for (int i = 0; i < n; i++) { parent[i] = i; size[i] = 1; } }
        public int Find(int x) { return parent[x] == x ? x : (parent[x] = Find(parent[x])); }
        public bool Union(int a, int b) { a = Find(a); b = Find(b); if (a == b) return false; if (size[a] < size[b]) { var t = a; a = b; b = t; } parent[b] = a; size[a] += size[b]; return true; }
        public int Size(int x) { return size[Find(x)]; }
    }

    class CircuitDSU : DSU
    {
        public CircuitDSU(int n) : base(n) { }
    }
}
