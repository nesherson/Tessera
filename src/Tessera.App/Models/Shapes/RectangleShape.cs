namespace Tessera.App.Models;

public class RectangleShape : ShapeBase
{
    public override bool Intersects(Rect rect)
    {
        return rect.Intersects(new Rect(X, Y, Width, Height));
    }

    public override bool HitTest(Point worldPoint, double tolerance) =>
        GetBounds().Inflate(tolerance).Contains(worldPoint);

    public override void Move(Vector delta)
    {
        X += delta.X;
        Y += delta.Y;
    }

    public override Rect GetBounds() => new(X, Y, Width, Height);
    
    protected override void OnBoundsChanged(Rect oldBounds, Rect newBounds)
    {
        var currentRect = new Rect(X, Y, Width, Height);
        var newRect = Remap(currentRect, oldBounds, newBounds);
        
        X = newRect.X;
        Y = newRect.Y;
        Width = newRect.Width;
        Height = newRect.Height;
    }
}