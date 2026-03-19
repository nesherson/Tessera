using Avalonia.Collections;
using Tessera.App.Enumerations;

namespace Tessera.App.Interfaces;

public interface IShapeProperties
{
    IBrush StrokeColor { get; set; }
    IBrush Color { get; set; }
    double StrokeThickness { get; set; }
    double Opacity { get; set; }
    AvaloniaList<double> StrokeDashArray { get; }
    StrokeType StrokeType { get; set; }
    FillType FillType { get; set; }
    ShapeType ShapeType { get; set; }
}