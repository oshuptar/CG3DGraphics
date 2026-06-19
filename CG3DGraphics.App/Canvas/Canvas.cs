using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Avalonia;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using CG3DGraphics.App.Buffers;
using CGRasterization.Core.Buffers;
using CGRasterization.Core.Buffers.Enums;
using CG3DGraphics.Core.Pipeline;
using CG3DGraphics.Core.Rasterization;
using CG3DGraphics.Core.Scenes;

namespace CG3DGraphics.App.Canvas;

public class Canvas : INotifyPropertyChanged
{
    private DirectBitmap Bitmap { get; set; }
    private readonly Scene _scene = SceneFactory.CreateTwoCubes();
    private readonly VertexProcessor _processor = new();
    private readonly ZBufferRasterizer _rasterizer = new();
    private readonly FrameBuffer _frame;

    public OrbitCamera Camera => _scene.Camera;
    public WriteableBitmap? ImageSource
    {
        get => field;
        private set
        {
            if (ReferenceEquals(field, value))
                return;
            field = value;
            OnPropertyChanged();
        }
    }
    public int Width => Bitmap.Width;
    public int Height => Bitmap.Height;

    public Canvas(int width, int height)
    {
        byte[] bytes = new byte[width * height * 4];
        for (int i = 0; i < bytes.Length; i += 4)
        {
            bytes[i] = 255;
            bytes[i + 1] = 255;
            bytes[i + 2] = 255;
            bytes[i + 3] = 255; 
        }
        Bitmap = new DirectBitmap(width, height, new Vector(96, 96), PixelFormat.Rgba8888, bytes);
        Bitmap.UpdateBitmap();
        ImageSource = Bitmap.Bitmap;
        _frame = new FrameBuffer(GetPixelBuffer());
        RedrawScene();
    }

    private PixelBuffer GetPixelBuffer() => new(
            Bitmap.Width,
            Bitmap.Height, 
            Bitmap.Pixels,
            Bitmap.Stride,
            Bitmap.PixelFormat == PixelFormats.Gray8 ? ColorFormat.Grayscale : ColorFormat.Rgba);
    
    private void InvalidateImage()
    {
        var current = ImageSource;
        ImageSource = null;
        ImageSource = current;
    }

    public void RedrawScene()
    {
        ClearScene();
        foreach (ProjectedTriangle triangle in _processor.Process(_scene, Width, Height))
            _rasterizer.Fill(triangle, _frame);
        Bitmap.UpdateBitmap();
        InvalidateImage();
    }

    public void Rotate(float deltaX, float deltaY)
    {
        Camera.AngleX += deltaX;
        Camera.AngleY += deltaY;
        RedrawScene();
    }

    public void Zoom(float delta)
    {
        Camera.Distance = MathF.Max(1f, Camera.Distance + delta);
        RedrawScene();
    }

    private void ClearScene()
    {
        byte[] px = _frame.Color.Pixels;
        for (int i = 0; i + 3 < px.Length; i += _frame.Color.BytesPerPixel)
        {
            px[i] = 255;
            px[i + 1] = 255;
            px[i + 2] = 255;
            px[i + 3] = 255;
        }
        Array.Fill(_frame.Depth, float.PositiveInfinity);
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}