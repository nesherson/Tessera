using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;

namespace Tessera.App.Controls;

public class DotGrid : Control
{
    public static readonly StyledProperty<double> PanXProperty = AvaloniaProperty.Register<DotGrid, double>(
        nameof(PanX));

    public static readonly StyledProperty<double> PanYProperty = AvaloniaProperty.Register<DotGrid, double>(
        nameof(PanY));
    
    private const double GridSpacing = 15.0;
    private const double DotRadius = 1.0;
    private static readonly IBrush DotBrush = Brushes.LightGray;
    
    public double PanY
    {
        get => GetValue(PanYProperty);
        set => SetValue(PanYProperty, value);
    }

    public double PanX
    {
        get => GetValue(PanXProperty);
        set => SetValue(PanXProperty, value);
    }
    
    public override void Render(DrawingContext context)
    {
        // base.Render(context);
        //
        // var bounds = Bounds;
        // var startX = CalculateCenteredOffset(bounds.Width);
        // var startY = CalculateCenteredOffset(bounds.Height);
        //
        // for (var x = startX; x < bounds.Width; x += GridSpacing)
        // {
        //     if (x > bounds.Width - GridSpacing) break;
        //     
        //     for (var y = startY; y < bounds.Height; y += GridSpacing)
        //     {
        //         if (y > bounds.Height - GridSpacing) break;
        //         
        //         var rect = new Rect(x - DotRadius, y - DotRadius, DotRadius * 2, DotRadius * 2);
        //         context.DrawEllipse(DotBrush, null, rect.Center, DotRadius, DotRadius);
        //     }
        // }
        base.Render(context);

        var bounds = Bounds;
        
        // 1. Calculate the "Screen" spacing (dots get further apart as you zoom in)
        double visualSpacing = GridSpacing;

        // If dots are too close, don't draw them (performance & visual noise)
        if (visualSpacing < 10) return; 

        // 2. Calculate Offset to keep dots "locked" to the world
        //    (This math ensures the dots slide correctly when you pan)
        double offsetX = PanX % visualSpacing;
        double offsetY = PanY % visualSpacing;

        // 3. Loop and Draw
        //    We draw slightly outside the bounds to ensure smooth edges
        for (double x = offsetX - visualSpacing; x < bounds.Width; x += visualSpacing)
        {
            for (double y = offsetY - visualSpacing; y < bounds.Height; y += visualSpacing)
            {
                // Draw a simple circle directly to the screen buffer
                context.DrawGeometry(DotBrush, null, new EllipseGeometry(new Rect(x, y, DotRadius, DotRadius)));
            }
        }
    }
    
    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);
        if (change.Property == PanXProperty || 
            change.Property == PanYProperty)
        {
            InvalidateVisual();
        }
    }
    
    private double CalculateCenteredOffset(double totalSize)
    {
        var availableSpace = totalSize - (2 * GridSpacing);
        
        if (availableSpace <= 0) return totalSize / 2;
        
        var intervals = (int)Math.Floor(availableSpace / GridSpacing);
        var gridWidth = intervals * GridSpacing;
        var remainder = totalSize - gridWidth;
        
        return remainder / 2;
    }
}