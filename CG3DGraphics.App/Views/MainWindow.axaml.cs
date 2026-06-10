using Avalonia.Controls;
using CG3DGraphics.App.ViewModels;

namespace CG3DGraphics.App.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        DataContext = new MainWindowViewModel();
        InitializeComponent();
    }
}