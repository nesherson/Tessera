using Avalonia;
using Avalonia.Media;

namespace Tessera.App.ViewModels;

public class PointShape : ShapeBase
{
    public PointShape(double x, double y, double size, IBrush color)
    {
        X = x;
        Y = y;
        Size = size;
        Color = color;
    }
    
    public double Size { get; set; }
    
    public override bool Intersects(Rect rect)
    {
        return rect.Contains(new Point(X, Y));
    }
}