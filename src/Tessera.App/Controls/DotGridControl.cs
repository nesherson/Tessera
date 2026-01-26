using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;

namespace Tessera.App.Controls;

public class DotGridControl : Control
{
    private const double GridSpacing = 15.0;
    private const double DotRadius = 1.0;
    private static readonly IBrush DotBrush = Brushes.LightGray;
    
    public override void Render(DrawingContext context)
    {
        base.Render(context);

        var bounds = Bounds;
        var startX = CalculateCenteredOffset(bounds.Width);
        var startY = CalculateCenteredOffset(bounds.Height);

        for (var x = startX; x < bounds.Width; x += GridSpacing)
        {
            if (x > bounds.Width - GridSpacing) break;
            
            for (var y = startY; y < bounds.Height; y += GridSpacing)
            {
                if (y > bounds.Height - GridSpacing) break;
                
                var rect = new Rect(x - DotRadius, y - DotRadius, DotRadius * 2, DotRadius * 2);
                context.DrawEllipse(DotBrush, null, rect.Center, DotRadius, DotRadius);
            }
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