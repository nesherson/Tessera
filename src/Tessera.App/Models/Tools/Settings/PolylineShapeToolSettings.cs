using System.Collections.Generic;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Tessera.App.Models;

public partial class PolylineShapeToolSettings : BaseToolSettings
{
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

    public PolylineShapeToolSettings()
    {
        Size = AvailableSizes.First();
        Color = AvailableColors.First();
        StrokeType = AvailableStrokeTypes.First();
    }
}