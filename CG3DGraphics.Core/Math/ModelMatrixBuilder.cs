using System.Numerics;

namespace CG3DGraphics.Core.Math;

public sealed class ModelMatrixBuilder
{
    private Matrix4x4 _acc = Matrix4x4.Identity;

    public ModelMatrixBuilder Translate(Vector3 t) => Apply(Transforms.Translation(t));
    public ModelMatrixBuilder Scale(Vector3 s) => Apply(Transforms.Scale(s));
    public ModelMatrixBuilder Scale(float s) => Apply(Transforms.Scale(s));
    public ModelMatrixBuilder RotateX(float angle) => Apply(Transforms.RotationX(angle));
    public ModelMatrixBuilder RotateY(float angle) => Apply(Transforms.RotationY(angle));
    public ModelMatrixBuilder RotateZ(float angle) => Apply(Transforms.RotationZ(angle));

    public ModelMatrixBuilder Reset()
    {
        _acc = Matrix4x4.Identity;
        return this;
    }

    public Matrix4x4 Build()
    {
        Matrix4x4 result = _acc;
        _acc = Matrix4x4.Identity;
        return result;
    }

    // Left-multiply so that call order equals application order.
    private ModelMatrixBuilder Apply(Matrix4x4 factor)
    {
        _acc = factor * _acc;
        return this;
    }
}
