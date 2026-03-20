using CommunityToolkit.Mvvm.ComponentModel;

namespace Tessera.App.Models;

public partial class CanvasTransform : ObservableObject
{
    private const double ZoomFactor = 1.1;
    private const double MinScale = 0.1;
    private const double MaxScale = 8.0;
    
    [ObservableProperty]
    private Matrix _matrix = Matrix.Identity;

    public Point ToWorld(Point screenPoint) =>
        !Matrix.HasInverse ? screenPoint : screenPoint.Transform(Matrix.Invert());

    public void Pan(double deltaX, double deltaY) => Matrix *= Matrix.CreateTranslation(deltaX, deltaY);

    public void ZoomAt(Point screenPoint, double delta)
    {
        var factor = delta > 0 ? ZoomFactor : 1.0 / ZoomFactor;
        var newScale = Matrix.M22 * factor;

        if (newScale is < MinScale or > MaxScale)
            return;
        
        Matrix *= Matrix.CreateTranslation(-screenPoint.X, -screenPoint.Y) *
                  Matrix.CreateScale(factor, factor) * Matrix.CreateTranslation(screenPoint.X, screenPoint.Y);
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