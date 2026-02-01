using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using Tessera.App.Enumerations;

namespace Tessera.App.ViewModels;

public partial class ShapeToolSettings : ObservableObject
{
    [ObservableProperty] private ShapeType _selectedShapeType = ShapeType.Rectangle;
    [ObservableProperty] private double _thickness = 2.0;
    [ObservableProperty] private Color _color = Colors.Transparent;
    [ObservableProperty] private Color _strokeColor = Colors.Black;
    
    public List<ShapeType> AvailableShapes { get; } = Enum.GetValues<ShapeType>().ToList();
}