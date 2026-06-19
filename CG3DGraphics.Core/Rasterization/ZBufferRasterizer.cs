using System.Drawing;
using System.Numerics;
using CG3DGraphics.Core.Pipeline;
using CGRasterization.Core.Buffers;

namespace CG3DGraphics.Core.Rasterization;

public sealed class ZBufferRasterizer
{
    private readonly record struct ScreenVertex(int X, int Y, float Z);

    private struct Edge
    {
        public int YMax;
        public float X;
        public float InvSlope; 
        public float Z;
        public float InvSlopeZ;
    }

    public void Fill(ProjectedTriangle triangle, FrameBuffer target)
    {
        var vertices = new[] { ToScreen(triangle.A), ToScreen(triangle.B), ToScreen(triangle.C) };

        ScanFill(vertices, (x, y, z) =>
        {
            if (x < 0 || y < 0 || x >= target.Color.Width || y >= target.Color.Height) return;
            int di = y * target.Color.Width + x;
            if (z >= target.Depth[di]) return; // points before the camer have pz <0 and z is 1/pz
            target.Depth[di] = z;
            PutPixel(target.Color, x, y, triangle.Color);
        });
    }

    private static ScreenVertex ToScreen(Vector3 v) => new((int)MathF.Round(v.X), (int)MathF.Round(v.Y), v.Z);

    // Scanline fill (active-edge table) carrying z linearly along edges and across each span.
    private static void ScanFill(IReadOnlyList<ScreenVertex> vertices, Action<int, int, float> writePixel)
    {
        int n = vertices.Count;
        if (n < 3) return;

        int[] indices = Enumerable.Range(0, n).OrderBy(i => vertices[i].Y).ToArray();
        int yMin = vertices[indices[0]].Y;
        int yMax = vertices[indices[^1]].Y;
        if (yMin == yMax) return;

        var aet = new List<Edge>();
        int k = 0;
        for (int y = yMin; y < yMax; y++)
        {
            while (k < n && vertices[indices[k]].Y == y)
            {
                int vi = indices[k];
                ScreenVertex cur = vertices[vi];
                ScreenVertex prev = vertices[(vi - 1 + n) % n];
                ScreenVertex next = vertices[(vi + 1) % n];
                if (prev.Y > y) aet.Add(MakeEdge(cur, prev));
                if (next.Y > y) aet.Add(MakeEdge(cur, next));
                k++;
            }

            aet = aet.OrderBy(e => e.X).ToList();
            for (int j = 0; j + 1 < aet.Count; j += 2)
            {
                Edge left = aet[j];
                Edge right = aet[j + 1];
                int xStart = (int)MathF.Ceiling(left.X);
                int xEnd = (int)MathF.Ceiling(right.X) - 1;
                float span = right.X - left.X;
                float dzdx = span > 0 ? (right.Z - left.Z) / span : 0f;
                for (int x = xStart; x <= xEnd; x++)
                    writePixel(x, y, left.Z + (x - left.X) * dzdx);
            }

            aet.RemoveAll(e => e.YMax == y + 1);
            for (int j = 0; j < aet.Count; j++)
            {
                Edge e = aet[j];
                e.X += e.InvSlope;
                e.Z += e.InvSlopeZ;
                aet[j] = e;
            }
        }
    }

    private static Edge MakeEdge(ScreenVertex start, ScreenVertex end)
    {
        float dy = end.Y - start.Y;
        return new Edge
        {
            YMax = end.Y,
            X = start.X,
            InvSlope = (end.X - start.X) / dy,
            Z = start.Z,
            InvSlopeZ = (end.Z - start.Z) / dy,
        };
    }

    private static void PutPixel(PixelBuffer buffer, int x, int y, Color color)
    {
        int i = y * buffer.Stride + x * buffer.BytesPerPixel;
        buffer.Pixels[i] = color.R;
        buffer.Pixels[i + 1] = color.G;
        buffer.Pixels[i + 2] = color.B;
        buffer.Pixels[i + 3] = color.A;
    }
}
