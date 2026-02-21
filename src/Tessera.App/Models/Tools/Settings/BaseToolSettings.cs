using System.Collections.Generic;
using Avalonia.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using Tessera.App.Helpers;

namespace Tessera.App.Models;

public partial class BaseToolSettings : ObservableObject
{
    [ObservableProperty]
    private IBrush _color;

    [ObservableProperty]
    private ShapeSize _size;
    
    [ObservableProperty]
    private double _opacity = 1;
    
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
}