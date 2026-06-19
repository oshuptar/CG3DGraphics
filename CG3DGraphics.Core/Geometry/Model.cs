using System.Numerics;

namespace CG3DGraphics.Core.Geometry;

public sealed class Model
{
    public Mesh Mesh { get; }
    public Matrix4x4 LocalToWorld { get; set; }

    public Model(Mesh mesh, Matrix4x4 localToWorld)
    {
        Mesh = mesh;
        LocalToWorld = localToWorld;
    }
}
