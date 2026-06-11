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

    public override double MinHeight => 16;
    public override double MinWidth => 32;

    public override bool Intersects(Rect rect) => 
        rect.Intersects(new Rect(X, Y, Width, Height));

    public override bool HitTest(Point worldPoint, double tolerance) => 
        GetBounds().Inflate(tolerance).Contains(worldPoint);

    public override void Move(Vector delta)
    {
        X += delta.X;
        Y += delta.Y;
    }

    public override Rect GetBounds()
    {
        return new Rect(X, Y, Width, Height);
    }

    public void UpdateBounds()
    {
        var lineCount = Text?.Split('\n').Length ?? 1;
        var lineHeight = FontSize * 1.4;
        var requiredHeight = lineCount * lineHeight;

        Height = Math.Max(requiredHeight, 16);
    }

    protected override void OnBoundsChanged(Rect oldBounds, Rect newBounds)
    {
        X = newBounds.X;
        Y = newBounds.Y;
        Width = newBounds.Width;
        Height = newBounds.Height;
        FontSize = newBounds.Height * 0.9;
    }
}