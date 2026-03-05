using CommunityToolkit.Mvvm.ComponentModel;

namespace Tessera.App.Models;

public class RectangleShape : ShapeBase
{
    public override bool Intersects(Rect rect)
    {
        return rect.Intersects(new Rect(X, Y, Width, Height));
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