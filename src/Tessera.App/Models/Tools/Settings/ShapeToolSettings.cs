using System.Collections.Generic;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Tessera.App.Helpers;

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
        new() { Name = "Rectangle", Description = "Rectangle", IconPath = Icons.Square },
        new() { Name = "Ellipse", Description = "Ellipse", IconPath = Icons.Circle }
    ];

    [ObservableProperty]
    private List<FillType> _availableFillTypes =
    [
        new() { Name = "None", Description = "Fill - None", IconPath = Icons.Square },
        new() { Name = "Semi", Description = "Fill - Semi", IconPath = Icons.SquareDuotone },
        new() { Name = "Solid", Description = "Fill - Solid", IconPath = Icons.SquareFill },
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