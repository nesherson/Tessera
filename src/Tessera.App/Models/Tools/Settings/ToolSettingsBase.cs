using System.Collections.Generic;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using Tessera.App.Helpers;

namespace Tessera.App.Models;

public partial class ToolSettingsBase : ObservableObject
{
    [ObservableProperty]
    private List<IBrush> _availableColors = [..AppColors.DefaultPalette];

    [ObservableProperty]
    private List<ShapeSize> _availableSizes =
    [
        new() { Name = "S", Description = "Small", Thickness = 4 },
        new() { Name = "M", Description = "Medium", Thickness = 8 },
        new() { Name = "L", Description = "Large", Thickness = 12 },
        new() { Name = "XL", Description = "Extra large", Thickness = 18 }
    ];
    
    [ObservableProperty]
    private List<StrokeType> _availableStrokeTypes =
    [
        new()
        {
            Name = "Solid",
            Description = "Solid stroke",
            IconPath = "/Assets/Icons/circle.svg",
            DashArray = []
        },
        new()
        {
            Name = "Dotted",
            Description = "Dotted stroke",
            IconPath = "/Assets/Icons/circle-dotted.svg",
            DashArray = [1, 2]
        },
        new()
        {
            Name = "Dashed",
            Description = "Dashed stroke",
            IconPath = "/Assets/Icons/circle-dashed.svg",
            DashArray = [4, 4]
        }
    ];

    [ObservableProperty]
    private StrokeType _strokeType;
    
    [ObservableProperty]
    private IBrush _color;

    [ObservableProperty]
    private ShapeSize _size;
    
    [ObservableProperty]
    private double _opacity;

    protected ToolSettingsBase()
    {
        Color = AvailableColors.First();
        StrokeType = AvailableStrokeTypes.First();
        Size = AvailableSizes.First();
        Opacity = 1;
    }
}