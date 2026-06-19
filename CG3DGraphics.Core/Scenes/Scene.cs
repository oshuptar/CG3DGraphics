using CG3DGraphics.Core.Geometry;

namespace CG3DGraphics.Core.Scenes;

public sealed class Scene
{
    public List<Model> Models { get; } = new();
    public OrbitCamera Camera { get; } = new();
    public float FieldOfView { get; set; } = MathF.PI / 2f;
    public float Near { get; set; } = 0.1f;
    public float Far { get; set; } = 100f;
}
