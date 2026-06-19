using CG3DGraphics.App.ViewModels.Abstractions;
using CommunityToolkit.Mvvm.Input;

namespace CG3DGraphics.App.ViewModels;

public class CanvasControlViewModel : ViewModelBase
{
    private const float AngleStep = 0.1f;
    private const float ZoomStep = 0.5f;

    public Canvas.Canvas Canvas { get; } = new(1200, 800);

    public RelayCommand RotateUpCommand { get; }
    public RelayCommand RotateDownCommand { get; }
    public RelayCommand RotateLeftCommand { get; }
    public RelayCommand RotateRightCommand { get; }
    public RelayCommand ZoomInCommand { get; }
    public RelayCommand ZoomOutCommand { get; }

    public CanvasControlViewModel()
    {
        RotateUpCommand = new RelayCommand(() => Canvas.Rotate(AngleStep, 0f));
        RotateDownCommand = new RelayCommand(() => Canvas.Rotate(-AngleStep, 0f));
        RotateLeftCommand = new RelayCommand(() => Canvas.Rotate(0f, AngleStep));
        RotateRightCommand = new RelayCommand(() => Canvas.Rotate(0f, -AngleStep));
        ZoomInCommand = new RelayCommand(() => Canvas.Zoom(-ZoomStep));
        ZoomOutCommand = new RelayCommand(() => Canvas.Zoom(ZoomStep));
    }
}
