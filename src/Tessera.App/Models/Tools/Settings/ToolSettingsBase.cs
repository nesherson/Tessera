using Avalonia.Collections;
using CommunityToolkit.Mvvm.ComponentModel;
using Tessera.App.Enumerations;
using Tessera.App.Helpers;
using Tessera.App.Interfaces;

namespace Tessera.App.Models;

public partial class ToolSettingsBase : ObservableObject, IShapeProperties
{
    [ObservableProperty]
    private IBrush _color = AppColors.BlackBrush;

    [ObservableProperty]
    private double _opacity = 1;

    [ObservableProperty]
    private IBrush _strokeColor = AppColors.BlackBrush;

    [ObservableProperty]
    private double _strokeThickness = 12;

    [ObservableProperty]
    private ShapeType _shapeType;
    
    public StrokeType StrokeType { get; set; }
    public FillType FillType { get; set; }

    public AvaloniaList<double> StrokeDashArray => StrokeType switch
    {
        StrokeType.Solid => [],
        StrokeType.Dashed => [1,2],
        StrokeType.Dotted => [4,4],
        _ => []
    };
}