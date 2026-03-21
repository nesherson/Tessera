namespace Tessera.App.Models;

public class PointShape : ShapeBase
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

    public override Rect GetBounds()
    {
        return InflateForStroke(new Rect(X, Y, Width, Height));
    }
}