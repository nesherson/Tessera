using CommunityToolkit.Mvvm.ComponentModel;

namespace Tessera.App.Models;

public partial class TextShape : ShapeBase
{
    [ObservableProperty]
    private string _text = "";

    [ObservableProperty]
    private double _fontSize;

    [ObservableProperty]
    private bool _isEditing;
    
    [ObservableProperty]
    private bool _isInitializing;

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

    public override bool HitTest(Point worldPoint, double tolerance)
    {
        return new Rect(X, Y, Width, Height).Contains(worldPoint);
    }

    public override void Move(Vector delta)
    {
        X += delta.X;
        Y += delta.Y;
    }
}