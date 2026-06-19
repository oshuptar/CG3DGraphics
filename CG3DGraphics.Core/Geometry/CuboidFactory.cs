using System.Drawing;
using System.Numerics;

namespace CG3DGraphics.Core.Geometry;

public static class CuboidFactory
{
    public static Mesh Create(float width, float height, float depth)
    {
        float w = width, h = height, d = depth;
        var faces = new (Vector3[] corners, Color color)[]
        {
            (new[] { V(0,0,0), V(0,h,0), V(w,h,0), V(w,0,0) }, Color.Red),     // front  
            (new[] { V(0,0,d), V(w,0,d), V(w,h,d), V(0,h,d) }, Color.Lime),    // back   
            (new[] { V(0,0,0), V(0,0,d), V(0,h,d), V(0,h,0) }, Color.Blue),    // left   
            (new[] { V(w,0,0), V(w,h,0), V(w,h,d), V(w,0,d) }, Color.Yellow),  // right  
            (new[] { V(0,h,0), V(0,h,d), V(w,h,d), V(w,h,0) }, Color.Cyan),    // top
            (new[] { V(0,0,0), V(w,0,0), V(w,0,d), V(0,0,d) }, Color.Magenta), // bottom
        };

        var vertices = new Vector3[faces.Length * 4];
        var triangles = new Triangle[faces.Length * 2];
        for (int i = 0; i < faces.Length; i++)
        {
            int b = i * 4;
            faces[i].corners.CopyTo(vertices, b);
            triangles[i * 2] = new Triangle(b, b + 1, b + 2, faces[i].color);
            triangles[i * 2 + 1] = new Triangle(b, b + 2, b + 3, faces[i].color);
        }

        return new Mesh(vertices, triangles);
    }

    private static Vector3 V(float x, float y, float z) => new(x, y, z);
}
