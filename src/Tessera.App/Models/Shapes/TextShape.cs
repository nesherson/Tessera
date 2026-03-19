using CommunityToolkit.Mvvm.ComponentModel;
using HarfBuzzSharp;

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
    
    public double MinHeight => FontSize * 1.4;
    
    public override bool Intersects(Rect rect)
    {
        var formattedText = new FormattedText(
            Text,
            System.Globalization.CultureInfo.CurrentCulture,
            FlowDirection.LeftToRight,
            new Typeface(new FontFamily("Cascadia Mono")),
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

    public override Rect GetBounds()
    {
        return InflateForStroke(new Rect(X, Y, Width, Height));
    }

    public void UpdateBounds()
    {
        var lineCount = Text?.Split('\n').Length ?? 1;
        var lineHeight = FontSize * 1.4;
        var requiredHeight = lineCount * lineHeight;

        Height = Math.Max(requiredHeight, 16);
    }
}