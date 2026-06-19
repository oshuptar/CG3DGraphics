using System.Drawing;
using System.Numerics;

namespace CG3DGraphics.Core.Pipeline;

// Each vertex is (screenX, screenY, depth), where depth = z' = 1/pz and is linear in screen space.
public readonly record struct ProjectedTriangle(Vector3 A, Vector3 B, Vector3 C, Color Color);
