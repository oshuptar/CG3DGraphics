using System.Numerics;

namespace CG3DGraphics.Core.Math;

public sealed class TransformationFactory
{
    private readonly ModelMatrixBuilder _builder = new();

    // rotation holds Euler angles in radians, applied X then Y then Z.
    public Matrix4x4 GetModelMatrix(
        Vector3? scale = null,
        Vector3? rotation = null,
        Vector3? translation = null)
    {
        _builder.Reset();
        if (scale is not null) _builder.Scale(scale.Value);
        if (rotation is not null) _builder.RotateX(rotation.Value.X).RotateY(rotation.Value.Y).RotateZ(rotation.Value.Z);
        if (translation is not null) _builder.Translate(translation.Value);
        return _builder.Build();
    }

    public Matrix4x4 GetCameraMatrix(Vector3 source, Vector3 target, Vector3 up)
        => Transforms.View(source, target, up);

    public Matrix4x4 GetPerspectiveMatrix(float range, float screenWidth, float screenHeight)
        => Transforms.Perspective(range, screenWidth, screenHeight);
}
