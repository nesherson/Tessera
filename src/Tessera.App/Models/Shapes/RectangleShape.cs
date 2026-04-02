using System.Diagnostics;
using Avalonia.Controls.Shapes;
using Tessera.App.Enumerations;

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

    public override Rect GetBounds() => 
        InflateForStroke(new Rect(X, Y, Width, Height));

    public override void Scale(ResizePoint resizePoint, Vector delta)
    {
        switch (resizePoint)
        {
            case ResizePoint.TopLeft:
            {
                var newHeight = Math.Max(10, Height - delta.Y);
                var newWidth = Math.Max(10, Width - delta.X);
                var newX  = X + (Width - newWidth);
                var newY  = Y + (Height - newHeight);
                
                Width = newWidth;
                Height = newHeight;
                X = newX;
                Y = newY;
                
                break;
            }
            case ResizePoint.BottomLeft:
            {
                var newWidth = Math.Max(10, Width - delta.X);
                var newX  = X + (Width - newWidth);
    
                Width = newWidth;
                Height = Math.Max(10, Height + delta.Y);
                X = newX;
                
                break;
            }
            case ResizePoint.TopRight:
            {
                var newHeight = Math.Max(10, Height - delta.Y);
                var newY  = Y + (Height - newHeight);
                
                Width = Math.Max(10, Width + delta.X);
                Height = newHeight;
                Y = newY;
                
                break;
            }
            case ResizePoint.BottomRight:
            {
                Width = Math.Max(10, Width + delta.X);
                Height = Math.Max(10, Height + delta.Y);
                
                break;
            }
        }
    }
}