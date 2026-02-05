using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Tessera.App.Enumerations;

namespace Tessera.App.Controls;

public class CanvasGrid : Control
{
    public static readonly StyledProperty<Matrix> MatrixProperty = AvaloniaProperty.Register<CanvasGrid, Matrix>(
        nameof(Matrix));
    
    public static readonly StyledProperty<double> GridSpacingProperty = AvaloniaProperty.Register<CanvasGrid, double>(
        nameof(Matrix));
    
    public static readonly StyledProperty<GridType> GridTypeProperty = AvaloniaProperty.Register<CanvasGrid, GridType>(
        nameof(GridType));
    
    public static readonly StyledProperty<IBrush> GridColorProperty = AvaloniaProperty.Register<CanvasGrid, IBrush>(
        nameof(Matrix));
    
    private const double DotRadius = 1.0;
    private IPen _pen = new Pen(Brushes.Black, thickness: 0.5);
    
    static CanvasGrid()
    {
        AffectsRender<CanvasGrid>(MatrixProperty);
        AffectsRender<CanvasGrid>(GridSpacingProperty);
        AffectsRender<CanvasGrid>(GridTypeProperty);
        AffectsRender<CanvasGrid>(GridColorProperty);
    }
    
    public Matrix Matrix
    {
        get => GetValue(MatrixProperty);
        set => SetValue(MatrixProperty, value);
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
    
    public override void Render(DrawingContext ctx)
    {
        base.Render(ctx);
        
        if (GridSpacing < 10) return; 
        if (!Matrix.HasInverse) return;

        var translateX = Matrix.M31;
        var translateY = Matrix.M32;
        var offsetX = translateX % GridSpacing;
        var offsetY = translateY % GridSpacing;

        switch (GridType)
        {
            case GridType.Dots:
                DrawDottedGrid(ctx, offsetX, offsetY, Bounds.Width, Bounds.Height);
                break;
            case GridType.Lines:
                DrawLineGrid(ctx, offsetX, offsetY, Bounds.Width, Bounds.Height);
                break;
            case GridType.Crosses:
                DrawCrossGrid(ctx, offsetX, offsetY, Bounds.Width, Bounds.Height);
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

    private void DrawLineGrid(DrawingContext ctx, double offsetX, double offsetY, double width, double height)
    {
        for (var x = offsetX - GridSpacing; x < width; x += GridSpacing)
        {
            var snappedX = Math.Round(x) + 0.5;
            
            ctx.DrawLine(_pen, new Point(snappedX, 0), new Point(snappedX, Bounds.Height));
        }
        
        for (var y = offsetY - GridSpacing; y < height; y += GridSpacing)
        {
            var snappedY = Math.Round(y) + 0.5;
            
            ctx.DrawLine(_pen, new Point(0, snappedY), new Point(Bounds.Width, snappedY));
        }
    }

    private void DrawDottedGrid(DrawingContext ctx, double offsetX, double offsetY, double width, double height)
    {
        for (var x = offsetX - GridSpacing; x < width; x += GridSpacing)
        {
            for (var y = offsetY - GridSpacing; y < height; y += GridSpacing)
            {
                ctx.DrawEllipse(GridColor, null, new Point(x, y), DotRadius, DotRadius);
            }
        }
    }
    
    private void DrawCrossGrid(DrawingContext ctx, double offsetX, double offsetY, double width, double height)
    {
        const double crossSize = 3.0; 

        for (var x = offsetX - GridSpacing; x < width; x += GridSpacing)
        {
            for (var y = offsetY - GridSpacing; y < height; y += GridSpacing)
            {
                var snappedX = Math.Round(x) + 0.5;
                var snappedY = Math.Round(y) + 0.5;

                ctx.DrawLine(_pen, 
                    new Point(snappedX - crossSize, snappedY), 
                    new Point(snappedX + crossSize, snappedY));

                ctx.DrawLine(_pen, 
                    new Point(snappedX, snappedY - crossSize), 
                    new Point(snappedX, snappedY + crossSize));
            }
        }
    }
}