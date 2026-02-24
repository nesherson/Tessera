using CommunityToolkit.Mvvm.ComponentModel;

namespace Tessera.App.Models;

public partial class CanvasTransform : ObservableObject
{
    [ObservableProperty]
    private double _offsetX;

    [ObservableProperty]
    private double _offsetY;
    
    [ObservableProperty]
    private double _scale = 1;

    public Point ToWorld(Point screenPoint)
    {
        var x = (screenPoint.X - OffsetX) / Scale;
        var y = (screenPoint.Y - OffsetY) / Scale;

        return new Point(x, y);
    }

    public Point ToScreen(Point worldPoint)
    {
        var x = worldPoint.X * Scale + OffsetX;
        var y = worldPoint.Y * Scale + OffsetY;

        return new Point(x, y);
    }

    public void Pan(double deltaX, double deltaY)
    {
        OffsetX += deltaX;
        OffsetY += deltaY;
    }

    public void ZoomAt(Point screenPoint, double zoomFactor)
    {
        var worldBefore = ToWorld(screenPoint);

        Scale *= zoomFactor;
        OffsetX = screenPoint.X - worldBefore.X * Scale;
        OffsetY = screenPoint.Y - worldBefore.Y * Scale;
    }

    public void Reset()
    {
        OffsetX = 0;
        OffsetY = 0;
        Scale = 1;
    }
}