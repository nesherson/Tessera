using Avalonia;
using Avalonia.Media;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Tessera.App.ViewModels;

public partial class TextShape : ShapeBase
{
    [ObservableProperty]
    private string _text = "";
    
    [ObservableProperty] 
    private double _fontSize;
    
    [ObservableProperty] 
    private bool _isEditing;
    
    [ObservableProperty]
    private FontFamily _fontFamily = new("Sans Serif Collection");
    
    public override bool Intersects(Rect rect)
    {
        var formattedText = new FormattedText(
            Text,
            System.Globalization.CultureInfo.CurrentCulture,
            FlowDirection.LeftToRight,
            new Typeface(FontFamily),
            FontSize,
            null);
        
        return rect.Intersects(new Rect(X, Y, formattedText.Width, formattedText.Height));
    }
}