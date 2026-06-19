using System.Numerics;

namespace CG3DGraphics.Core.Geometry;

public sealed class Mesh
{
    public Vector3[] Vertices { get; }
    public Triangle[] Triangles { get; }

    public Mesh(Vector3[] vertices, Triangle[] triangles)
    {
        Vertices = vertices;
        Triangles = triangles;
    }
}
