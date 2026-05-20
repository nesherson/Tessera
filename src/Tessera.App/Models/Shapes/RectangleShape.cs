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
        var normalizedX = (X - oldBounds.X) / oldBounds.Width;
        var normalizedY = (Y - oldBounds.Y) / oldBounds.Height;
        var normalizedXOne = (X + Width - oldBounds.X) / oldBounds.Width;
        var normalizedYOne = (Y + Height - oldBounds.Y) / oldBounds.Height;
        var newXPoint = newBounds.X + normalizedX * newBounds.Width;
        var newYPoint = newBounds.Y + normalizedY * newBounds.Height;
        var newXOne = newBounds.X + normalizedXOne * newBounds.Width;
        var newYOne = newBounds.Y + normalizedYOne * newBounds.Height;
        
        X = newXPoint;
        Y = newYPoint;
        Width = newXOne - newXPoint;
        Height = newYOne - newYPoint;
    }
}