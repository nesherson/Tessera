using Avalonia;
using Avalonia.Media;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Tessera.App.ViewModels;

public abstract partial class ShapeBase : ObservableObject
{
    [ObservableProperty] 
    private double _x;
    
    [ObservableProperty]
    private double _y;
    
    [ObservableProperty] 
    private double _width;
    
    [ObservableProperty]
    private double _height;
    
    [ObservableProperty] 
    private double _strokeThickness;
    
    [ObservableProperty] 
    private IBrush _color = Brushes.Black;
    
    [ObservableProperty] 
    private IBrush _strokeColor = Brushes.Black;
    
    [ObservableProperty]
    private bool _isVisible = true;
    
    public abstract bool Intersects(Rect rect);
}