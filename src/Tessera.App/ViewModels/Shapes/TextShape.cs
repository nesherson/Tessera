using Avalonia;
using Avalonia.Media;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Tessera.App.ViewModels;

public partial class TextShape : ShapeBase
{
    [ObservableProperty]
    private string _text;
    
    [ObservableProperty] 
    private double _fontSize = 14;
    
    [ObservableProperty] 
    private IBrush _textColor = Brushes.Black;
    
    [ObservableProperty] 
    private bool _isEditing;
    
    public override bool Intersects(Rect rect)
    {
        return false;
    }
}