using System.Numerics;
using CG3DGraphics.Core.Geometry;
using CG3DGraphics.Core.Math;

namespace CG3DGraphics.Core.Scenes;

public static class SceneFactory
{
    public static Scene CreateTwoCubes()
    {
        var scene = new Scene();

        scene.Models.Add(Cube(size: 1.5f,
            position: new Vector3(-1.5f, 0f, -0.5f),
            rotation: new Vector3(0.3f, 0.5f, 0f)));

        scene.Models.Add(Cube(size: 1.5f,
            position: new Vector3(1.5f, 0f, 0.5f),
            rotation: new Vector3(0.2f, -0.4f, 0.1f)));

        return scene;
    }

    private static Model Cube(float size, Vector3 position, Vector3 rotation)
    {
        Mesh mesh = CuboidFactory.Create(size, size, size);
        float half = size / 2f;

        Matrix4x4 localToWorld = new ModelMatrixBuilder()
            .Translate(new Vector3(-half, -half, -half)) // centre the corner-origin cube
            .RotateX(rotation.X)
            .RotateY(rotation.Y)
            .RotateZ(rotation.Z)
            .Translate(position)
            .Build();

        return new Model(mesh, localToWorld);
    }
}
