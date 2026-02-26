using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Tessera.App.Models;

public partial class CanvasTransform : ObservableObject
{
    [ObservableProperty]
    private Matrix _matrix = Matrix.Identity;

    // [ObservableProperty]
    // private double _offsetX;
    //
    // [ObservableProperty]
    // private double _offsetY;
    //
    // [ObservableProperty]
    // private double _scale = 1;

    public Point ToWorld(Point screenPoint)
    {
        // var x = (screenPoint.X - OffsetX) / Scale;
        // var y = (screenPoint.Y - OffsetY) / Scale;
        //
        // return new Point(x, y);
        return !Matrix.HasInverse ? screenPoint : screenPoint.Transform(Matrix.Invert());
    }

    // public Point ToScreen(Point worldPoint)
    // {
    //     // var x = worldPoint.X * Scale + OffsetX;
    //     // var y = worldPoint.Y * Scale + OffsetY;
    //     //
    //     // return new Point(x, y);
    // }

    public void Pan(double deltaX, double deltaY)
    {
        // OffsetX += deltaX;
        // OffsetY += deltaY;
        Matrix *= Matrix.CreateTranslation(deltaX, deltaX);
    }

    public void ZoomAt(Point screenPoint, double zoomFactor)
    {
        // var worldBefore = ToWorld(screenPoint);
        //
        // Scale *= zoomFactor;
        // OffsetX = screenPoint.X - worldBefore.X * Scale;
        // OffsetY = screenPoint.Y - worldBefore.Y * Scale;
        Matrix *= Matrix.CreateTranslation(-screenPoint.X, -screenPoint.Y)
            * Matrix.CreateScale(zoomFactor, zoomFactor)
            * Matrix.CreateTranslation(screenPoint.X, screenPoint.Y);;
    }

    public void Reset()
    {
        // OffsetX = 0;
        // OffsetY = 0;
        // Scale = 1;
        Matrix = Matrix.Identity;
    }

    public void ResetZoom(Point screenPoint)
    {
        // var worldCenter = ToWorld(screenPoint);
        //
        // Scale = 1.0;
        //
        // OffsetX = screenPoint.X - worldCenter.X;
        // OffsetY = screenPoint.Y - worldCenter.Y;
    }
}