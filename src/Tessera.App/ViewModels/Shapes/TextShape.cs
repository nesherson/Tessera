using Avalonia;
using Avalonia.Media;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Tessera.App.ViewModels;

public partial class TextShape : ShapeBase
{
    [ObservableProperty]
    private string _text;
    
    [ObservableProperty] 
    private double _fontSize;
    
    [ObservableProperty] 
    private bool _isEditing;
    
    public override bool Intersects(Rect rect)
    {
        return false;
    }
}