using CommunityToolkit.Mvvm.ComponentModel;

namespace Tessera.App.Models;

public partial class CanvasTransform : ObservableObject
{
    [ObservableProperty]
    private Matrix _matrix = Matrix.Identity;

    public Point ToWorld(Point screenPoint) =>
        !Matrix.HasInverse ? screenPoint : screenPoint.Transform(Matrix.Invert());

    public void Pan(double deltaX, double deltaY) => Matrix *= Matrix.CreateTranslation(deltaX, deltaY);

    public void ZoomAt(Point screenPoint, double zoomFactor)
    {
        Matrix *= Matrix.CreateTranslation(-screenPoint.X, -screenPoint.Y) *
                  Matrix.CreateScale(zoomFactor, zoomFactor) * Matrix.CreateTranslation(screenPoint.X, screenPoint.Y);
    }

    public void Reset()
    {
        Matrix = Matrix.Identity;
    }

    public void ResetZoom(Point screenPoint)
    {
        var worldCenter = ToWorld(screenPoint);

        var offsetX = screenPoint.X - worldCenter.X;
        var offsetY = screenPoint.Y - worldCenter.Y;

        Matrix = Matrix.CreateTranslation(offsetX, offsetY);
    }
}