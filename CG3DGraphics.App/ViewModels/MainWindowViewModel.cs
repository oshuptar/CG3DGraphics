using CG3DGraphics.App.ViewModels.Abstractions;

namespace CG3DGraphics.App.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    public CanvasControlViewModel CanvasControlViewModel { get; set; } = new();
}