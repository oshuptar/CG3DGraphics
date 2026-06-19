using CGRasterization.Core.Buffers;

namespace CG3DGraphics.Core.Rasterization;

// Bundles the colour buffer with a matching depth (z') buffer of the same size.
public sealed class FrameBuffer
{
    public PixelBuffer Color { get; }
    public float[] Depth { get; }

    public FrameBuffer(PixelBuffer color)
    {
        Color = color;
        Depth = new float[color.Width * color.Height];
    }
}
