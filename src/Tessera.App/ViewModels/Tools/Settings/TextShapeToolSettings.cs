using Avalonia.Media;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Tessera.App.ViewModels;

public partial class TextShapeToolSettings : ObservableObject
{
    [ObservableProperty]
    private double _fontSize = 14;

    [ObservableProperty] 
    private IBrush _color = Brushes.Black;
}