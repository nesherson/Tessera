using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;

namespace Tessera.App.Controls;

public class DotGrid : Control
{
    public static readonly StyledProperty<Matrix> MatrixProperty = AvaloniaProperty.Register<DotGrid, Matrix>(
        nameof(Matrix));
    
    public static readonly StyledProperty<double> GridSpacingProperty = AvaloniaProperty.Register<DotGrid, double>(
        nameof(Matrix));
    
    private const double DotRadius = 1.0;
    private static readonly IBrush DotBrush = Brushes.LightGray;
    
    static DotGrid()
    {
        AffectsRender<DotGrid>(MatrixProperty);
        AffectsRender<DotGrid>(GridSpacingProperty);
    }
    
    public Matrix Matrix
    {
        get => GetValue(MatrixProperty);
        set => SetValue(MatrixProperty, value);
    }
    
    public double GridSpacing
    {
        get => GetValue(GridSpacingProperty);
        set => SetValue(GridSpacingProperty, value);
    }
    
    public override void Render(DrawingContext context)
    {
        base.Render(context);

        var bounds = Bounds;
        
        if (GridSpacing < 10) return; 
        if (!Matrix.HasInverse) return;

        var translateX = Matrix.M31;
        var translateY = Matrix.M32;
        var offsetX = translateX % GridSpacing;
        var offsetY = translateY % GridSpacing;
        
        for (var x = offsetX - GridSpacing; x < bounds.Width; x += GridSpacing)
        {
            for (var y = offsetY - GridSpacing; y < bounds.Height; y += GridSpacing)
            {
                context.DrawEllipse(DotBrush, null, new Point(x, y), DotRadius, DotRadius);
            }
        }
    }
}