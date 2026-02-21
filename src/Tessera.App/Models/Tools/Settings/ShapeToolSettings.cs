using System.Collections.Generic;
using System.Linq;
using Avalonia.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Tessera.App.Models;

public partial class ShapeToolSettings : ToolSettingsBase
{
    [ObservableProperty]
    private ShapeType _shapeType;
    
    [ObservableProperty]
    private IBrush _strokeColor =  Brushes.Black;
    
    [ObservableProperty]
    private List<ShapeType> _availableShapeTypes =
    [
        new() { Name = "Rectangle", Description = "Rectangle", IconPath = "/Assets/Icons/square.svg" },
        new() { Name = "Ellipse", Description = "Ellipse", IconPath = "/Assets/Icons/circle.svg" }
    ];

    [ObservableProperty]
    private bool _isShapePopupOpen;

    public ShapeToolSettings()
    {
        Size = AvailableSizes.First();
        Color = AvailableColors.First();
        StrokeType = AvailableStrokeTypes.First();
        ShapeType = AvailableShapeTypes.First();
    }

    [RelayCommand]
    private void OpenShapePopup()
    {
        IsShapePopupOpen = true;
    }
}