using Avalonia;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Tessera.App.Models;

public partial class CanvasTransform : ObservableObject
{
    [ObservableProperty]
    private double _offsetX;

    [ObservableProperty]
    private double _offsetY;

    public Point ToWorld(Point screenPoint)
    {
        var x = screenPoint.X - OffsetX;
        var y = screenPoint.Y - OffsetY;

        return new Point(x, y);
    }

    public Point ToScreen(Point worldPoint)
    {
        var x = worldPoint.X + OffsetX;
        var y = worldPoint.Y + OffsetY;

        return new Point(x, y);
    }

    public void Pan(double deltaX, double deltaY)
    {
        OffsetX += deltaX;
        OffsetY += deltaY;
    }

    public void Reset()
    {
        OffsetX = 0;
        OffsetY = 0;
    }
}