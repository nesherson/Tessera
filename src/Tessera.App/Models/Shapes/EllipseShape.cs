namespace Tessera.App.Models;

public class EllipseShape : ShapeBase
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
    
    protected override void OnBoundsChanged(Rect oldBounds, Rect newBounds)
    {
        Width = newBounds.Width;
        Height = newBounds.Height;
        X = newBounds.X;
        Y = newBounds.Y;
    }

    public override Rect GetBounds() => new(X, Y, Width, Height);
}