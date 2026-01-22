using Avalonia;
using Avalonia.Media;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Tessera.App.ViewModels;

public abstract partial class ShapeBase : ObservableObject
{
    [ObservableProperty] private double _x;
    [ObservableProperty] private double _y;
    [ObservableProperty] private IBrush _color = Brushes.Black;
    
    public abstract bool Intersects(Rect rect);
}