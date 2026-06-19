using System.Numerics;
using CG3DGraphics.Core.Geometry;
using CG3DGraphics.Core.Math;
using CG3DGraphics.Core.Scenes;

namespace CG3DGraphics.Core.Pipeline;

public sealed class VertexProcessor
{
    private readonly TransformationFactory _transforms = new();

    public List<ProjectedTriangle> Process(Scene scene, int screenWidth, int screenHeight)
    {
        Matrix4x4 view = _transforms.GetCameraMatrix(scene.Camera.Source, scene.Camera.Target, scene.Camera.Up);
        Matrix4x4 projection = _transforms.GetPerspectiveMatrix(scene.FieldOfView, screenWidth, screenHeight);

        var result = new List<ProjectedTriangle>();
        foreach (Model model in scene.Models)
            ProjectModel(model, view * model.LocalToWorld, projection, scene.Near, result);
        return result;
    }

    private static void ProjectModel(Model model, Matrix4x4 modelView, Matrix4x4 projection, float near, List<ProjectedTriangle> output)
    {
        Vector3[] vertices = model.Mesh.Vertices;
        int n = vertices.Length;

        var cameraZ = new float[n];
        var screen = new Vector3[n];
        for (int i = 0; i < n; i++)
        {
            Vector4 camera = Transforms.Apply(modelView, new Vector4(vertices[i], 1f));
            cameraZ[i] = camera.Z;
            Vector4 clip = Transforms.Apply(projection, camera);
            Vector4 ndc = clip / clip.W;
            screen[i] = new Vector3(ndc.X, ndc.Y, ndc.Z);
        }

        foreach (Triangle t in model.Mesh.Triangles)
        {
            // drop the whole triangle if any vertex is at or behind the near plane.
            // > -near is due to the fact that + z axis in camera is towards the user (from screen)
            if (cameraZ[t.A] > -near || cameraZ[t.B] > -near || cameraZ[t.C] > -near)
                continue;
            output.Add(new ProjectedTriangle(screen[t.A], screen[t.B], screen[t.C], t.Color));
        }
    }
}
