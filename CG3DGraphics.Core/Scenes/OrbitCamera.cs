using System.Numerics;
using CG3DGraphics.Core.Math;

namespace CG3DGraphics.Core.Scenes;

public sealed class OrbitCamera
{
    public Vector3 Target { get; set; } = Vector3.Zero; // world-space point the camera looks at and orbits
    public Vector3 Up { get; set; } = Vector3.UnitY;
    public float Distance { get; set; } = 5f;
    public float AngleX { get; set; } // orbit angle around the X axis (radians)
    public float AngleY { get; set; } // orbit angle around the Y axis (radians)

    public Vector3 Source
    {
        get
        {
            // Start Distance behind the target, then orbit that offset around it.
            Matrix4x4 orbit = Transforms.RotationY(AngleY) * Transforms.RotationX(AngleX);
            Vector4 offset = Transforms.Apply(orbit, new Vector4(0, 0, -Distance, 0));
            return Target + new Vector3(offset.X, offset.Y, offset.Z);
        }
    }
}
