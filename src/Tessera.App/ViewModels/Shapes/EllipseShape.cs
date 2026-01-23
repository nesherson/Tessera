using Avalonia;
using Avalonia.Media;

namespace Tessera.App.ViewModels;

public class EllipseShape : ShapeBase
{
    public double Size { get; set; }
    
    public override bool Intersects(Rect rect)
    {
        return rect.Contains(new Point(X, Y));
    }
}