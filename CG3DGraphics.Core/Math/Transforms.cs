using System.Numerics;

namespace CG3DGraphics.Core.Math;

public static class Transforms
{
    public static Matrix4x4 Translation(Vector3 t) => new(
        1, 0, 0, t.X,
        0, 1, 0, t.Y,
        0, 0, 1, t.Z,
        0, 0, 0, 1);

    public static Matrix4x4 Scale(Vector3 s) => new(
        s.X, 0,   0,   0,
        0,   s.Y, 0,   0,
        0,   0,   s.Z, 0,
        0,   0,   0,   1);

    public static Matrix4x4 Scale(float s) => Scale(new Vector3(s));

    public static Matrix4x4 RotationX(float angle)
    {
        float c = MathF.Cos(angle), s = MathF.Sin(angle);
        return new(
            1, 0,  0, 0,
            0, c, -s, 0,
            0, s,  c, 0,
            0, 0,  0, 1);
    }

    public static Matrix4x4 RotationY(float angle)
    {
        float c = MathF.Cos(angle), s = MathF.Sin(angle);
        return new(
             c, 0, s, 0,
             0, 1, 0, 0,
            -s, 0, c, 0,
             0, 0, 0, 1);
    }

    public static Matrix4x4 RotationZ(float angle)
    {
        float c = MathF.Cos(angle), s = MathF.Sin(angle);
        return new(
            c, -s, 0, 0,
            s,  c, 0, 0,
            0,  0, 1, 0,
            0,  0, 0, 1);
    }

    public static Matrix4x4 View(Vector3 source, Vector3 target, Vector3 up)
    {
        Vector3 z = Vector3.Normalize(source - target);
        Vector3 x = Vector3.Normalize(Vector3.Cross(z, up));
        Vector3 y = Vector3.Cross(x, z);

        return new(
            x.X, x.Y, x.Z, -Vector3.Dot(x, source),
            y.X, y.Y, y.Z, -Vector3.Dot(y, source),
            z.X, z.Y, z.Z, -Vector3.Dot(z, source),
            0,   0,   0,    1);
    }

    public static Matrix4x4 Perspective(float range, float screenWidth, float screenHeight)
    {
        float cx = screenWidth / 2f;
        float f = cx / MathF.Tan(range / 2f);
        float cy = screenHeight / 2f;

        return new(
            -f, 0, cx, 0,
            0,  f, cy, 0,
            0,  0, 0,  1, 
            0,  0, 1,  0);  
    }

    public static Vector4 Apply(Matrix4x4 m, Vector4 v) => Vector4.Transform(v, Matrix4x4.Transpose(m));
}
