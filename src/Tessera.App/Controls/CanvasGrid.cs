using Tessera.App.Enumerations;

namespace Tessera.App.Controls;

public class CanvasGrid : Control
{
    public static readonly StyledProperty<double> GridSpacingProperty =
        AvaloniaProperty.Register<CanvasGrid, double>(nameof(GridSpacing));

    public static readonly StyledProperty<GridType> GridTypeProperty =
        AvaloniaProperty.Register<CanvasGrid, GridType>(nameof(GridType));

    public static readonly StyledProperty<IBrush> GridColorProperty =
        AvaloniaProperty.Register<CanvasGrid, IBrush>(nameof(GridColor));

    public static readonly StyledProperty<Matrix> MatrixProperty =
        AvaloniaProperty.Register<CanvasGrid, Matrix>(nameof(Matrix));

    private const double DotRadius = 1.0;
    private IPen _pen = new Pen(Brushes.Black, thickness: 0.5);

    static CanvasGrid()
    {
        AffectsRender<CanvasGrid>(MatrixProperty);
        AffectsRender<CanvasGrid>(GridSpacingProperty);
        AffectsRender<CanvasGrid>(GridTypeProperty);
    }

    public GridType GridType
    {
        get => GetValue(GridTypeProperty);
        set => SetValue(GridTypeProperty, value);
    }

    public double GridSpacing
    {
        get => GetValue(GridSpacingProperty);
        set => SetValue(GridSpacingProperty, value);
    }

    public IBrush GridColor
    {
        get => GetValue(GridColorProperty);
        set => SetValue(GridColorProperty, value);
    }
    
    public Matrix Matrix
    {
        get => GetValue(MatrixProperty);
        set => SetValue(MatrixProperty, value);
    }

    public override void Render(DrawingContext ctx)
    {
        base.Render(ctx);

        var viewWidth = Bounds.Width / Matrix.M11;
        var viewHeight = Bounds.Height / Matrix.M22;
        var worldLeft = -Matrix.M31 / Matrix.M22;
        var worldTop = -Matrix.M32 / Matrix.M22;

        var startX = Math.Floor(worldLeft / GridSpacing) * GridSpacing;
        var startY = Math.Floor(worldTop / GridSpacing) * GridSpacing;
        var endX = worldLeft + viewWidth + GridSpacing;
        var endY = worldTop + viewHeight + GridSpacing;

        switch (GridType)
        {
            case GridType.Dots:
                DrawDottedGrid(ctx, startX, startY, endX, endY);
                break;
            case GridType.Lines:
                DrawLineGrid(ctx, startX, startY, endX, endY);
                break;
            case GridType.Crosses:
                DrawCrossGrid(ctx, startX, startY, endX, endY);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);

        if (change.Property == GridColorProperty)
        {
            var newBrush = change.GetNewValue<IBrush>();

            _pen = new Pen(newBrush, 0.5);
        }
    }

    private void DrawLineGrid(DrawingContext ctx, double startX, double startY, double endX, double endY)
    {
        for (var x = startX; x < endX; x += GridSpacing)
        {
            var snappedX = Math.Round(x) + 0.5;

            ctx.DrawLine(_pen, new Point(snappedX, 0), new Point(snappedX, Bounds.Height));
        }

        for (var y = startY; y < endY; y += GridSpacing)
        {
            var snappedY = Math.Round(y) + 0.5;

            ctx.DrawLine(_pen, new Point(0, snappedY), new Point(Bounds.Width, snappedY));
        }
    }

    private void DrawDottedGrid(DrawingContext ctx, double startX, double startY, double endX, double endY)
    {
        for (var x = startX; x < endX; x += GridSpacing)
        {
            for (var y = startY; y < endY; y += GridSpacing)
            {
                ctx.DrawEllipse(GridColor, null, new Point(x, y), DotRadius, DotRadius);
            }
        }
    }

    private void DrawCrossGrid(DrawingContext ctx, double startX, double startY, double endX, double endY)
    {
        const double crossSize = 3.0;

        for (var x = startX; x < endX; x += GridSpacing)
        {
            for (var y = startY; y < endY; y += GridSpacing)
            {
                var snappedX = Math.Round(x) + 0.5;
                var snappedY = Math.Round(y) + 0.5;

                ctx.DrawLine(_pen, new Point(snappedX - crossSize, snappedY),
                    new Point(snappedX + crossSize, snappedY));

                ctx.DrawLine(_pen, new Point(snappedX, snappedY - crossSize),
                    new Point(snappedX, snappedY + crossSize));
            }
        }
    }
}