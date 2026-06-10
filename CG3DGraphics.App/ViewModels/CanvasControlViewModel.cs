using CG3DGraphics.App.ViewModels.Abstractions;

namespace CG3DGraphics.App.ViewModels;

public class CanvasControlViewModel : ViewModelBase
{
    public Canvas.Canvas Canvas { get; set; } = new(1200, 800);
}