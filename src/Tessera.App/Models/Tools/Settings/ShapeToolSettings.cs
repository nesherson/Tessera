using System.Collections.Generic;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Tessera.App.Models;

public partial class ShapeToolSettings : ToolSettingsBase
{
    [ObservableProperty]
    private ShapeType _shapeType;
    
    [ObservableProperty]
    private FillType _fillType;
    
    [ObservableProperty]
    private IBrush _strokeColor =  Brushes.Black;
    
    [ObservableProperty]
    private List<ShapeType> _availableShapeTypes =
    [
        new() { Name = "Rectangle", Description = "Rectangle", IconPath = "/Assets/Icons/square.svg" },
        new() { Name = "Ellipse", Description = "Ellipse", IconPath = "/Assets/Icons/circle.svg" }
    ];

    [ObservableProperty]
    private List<FillType> _availableFillTypes =
    [
        new() { Name = "None", Description = "Fill - None", IconPath = "/Assets/Icons/square.svg" },
        new() { Name = "Semi", Description = "Fill - Semi", IconPath = "/Assets/Icons/square-duotone.svg" },
        new() { Name = "Solid", Description = "Fill - Solid", IconPath = "/Assets/Icons/square-fill.svg" },
    ];

    [ObservableProperty]
    private bool _isShapePopupOpen;

    public ShapeToolSettings()
    {
        ShapeType = AvailableShapeTypes.First();
        FillType = AvailableFillTypes.First();
    }

    [RelayCommand]
    private void OpenShapePopup()
    {
        IsShapePopupOpen = true;
    }
}