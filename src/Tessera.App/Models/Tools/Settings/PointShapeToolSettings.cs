using System.Collections.Generic;
using System.Linq;
using Avalonia.Media;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Tessera.App.Models;

public partial class PointShapeToolSettings : ObservableObject
{
    [ObservableProperty]
    private double _pointThickness = 3;

    [ObservableProperty]
    private IBrush _pointColor;

    [ObservableProperty]
    private ShapeSize _pointSize;

    [ObservableProperty]
    private List<IBrush> _availableColors =
    [
        SolidColorBrush.Parse("#000000"), SolidColorBrush.Parse("#9fa8b2"), SolidColorBrush.Parse("#e085f4"),
        SolidColorBrush.Parse("#ae3ec9"), SolidColorBrush.Parse("#000000"), SolidColorBrush.Parse("#9fa8b2"),
        SolidColorBrush.Parse("#e085f4"), SolidColorBrush.Parse("#ae3ec9"), SolidColorBrush.Parse("#000000"),
        SolidColorBrush.Parse("#9fa8b2"), SolidColorBrush.Parse("#e085f4"), SolidColorBrush.Parse("#ae3ec9")
    ];

    [ObservableProperty]
    private List<ShapeSize> _availableSizes =
    [
        new() { Name = "S", Description = "Small", Thickness = 4 },
        new() { Name = "M", Description = "Medium", Thickness = 8 },
        new() { Name = "L", Description = "Large", Thickness = 12 },
        new() { Name = "XL", Description = "Extra large", Thickness = 18 }
    ];

    public PointShapeToolSettings()
    {
        PointSize = AvailableSizes.First();
        PointColor = AvailableColors.First();
    }
}

public struct ShapeSize
{
    public string Name { get; set; }
    public string Description { get; set; }
    public double Thickness { get; set; }
}