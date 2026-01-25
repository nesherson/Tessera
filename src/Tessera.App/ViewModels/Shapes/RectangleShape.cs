using Avalonia;

namespace Tessera.App.ViewModels;

public class RectangleShape : ShapeBase
{
    public override bool Intersects(Rect rect)
    {
        return false;
    }
}