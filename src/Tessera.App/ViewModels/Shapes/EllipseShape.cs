using Avalonia;

namespace Tessera.App.ViewModels;

public class EllipseShape : ShapeBase
{
    public override bool Intersects(Rect rect)
    {
        return rect.Intersects(new Rect(X, Y, Width, Height));
    }
}