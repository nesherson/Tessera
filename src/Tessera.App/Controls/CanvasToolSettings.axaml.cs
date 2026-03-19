using System.Collections.Generic;
using CommunityToolkit.Mvvm.Input;
using Tessera.App.Enumerations;
using Tessera.App.Helpers;
using Tessera.App.Interfaces;
using Tessera.App.Models;

namespace Tessera.App.Controls;

public partial class CanvasToolSettings : UserControl
{
    public static readonly StyledProperty<IShapeProperties> PropertiesProperty =
        AvaloniaProperty.Register<CanvasToolSettings, IShapeProperties>(nameof(Properties));

    public static readonly StyledProperty<List<IBrush>> AvailableColorsProperty = 
        AvaloniaProperty.Register<CanvasToolSettings, List<IBrush>>(
        nameof(AvailableColors));

    public static readonly StyledProperty<List<ShapeSize>> AvailableSizesProperty = 
        AvaloniaProperty.Register<CanvasToolSettings, List<ShapeSize>>(
        nameof(AvailableSizes));
    
    public static readonly StyledProperty<List<ToolListItem<StrokeType>>> AvailableStrokeTypesProperty = 
        AvaloniaProperty.Register<CanvasToolSettings, List<ToolListItem<StrokeType>>>(
            nameof(AvailableStrokeTypes));

    public static readonly StyledProperty<List<ToolListItem<FillType>>> AvailableFillTypesProperty = 
        AvaloniaProperty.Register<CanvasToolSettings, List<ToolListItem<FillType>>>(
        nameof(AvailableFillTypes));
    
    public static readonly StyledProperty<List<ToolListItem<ShapeType>>> AvailableShapeTypesProperty = 
        AvaloniaProperty.Register<CanvasToolSettings, List<ToolListItem<ShapeType>>>(
            nameof(AvailableShapeTypes));
    
    public static readonly StyledProperty<bool> IsShapePopupOpenProperty = 
        AvaloniaProperty.Register<CanvasToolSettings, bool>(
            nameof(IsShapePopupOpen));
    
    public static readonly StyledProperty<ToolListItem<ShapeType>> SelectedShapeTypeProperty = 
        AvaloniaProperty.Register<CanvasToolSettings, ToolListItem<ShapeType>>(
            nameof(SelectedShapeType));
    
    public CanvasToolSettings()
    {
        InitializeComponent();

        AvailableColors = [..AppColors.DefaultPalette];
        AvailableSizes = [
            new() { Name = "S", Description = "Small", Thickness = 4 },
            new() { Name = "M", Description = "Medium", Thickness = 8 },
            new() { Name = "L", Description = "Large", Thickness = 12 },
            new() { Name = "XL", Description = "Extra large", Thickness = 18 }
        ];
        AvailableStrokeTypes = [
            new()
            {
                Name = "Solid",
                Description = "Solid stroke",
                IconPath = Icons.Circle,
                Type = StrokeType.Solid
            },
            new()
            {
                Name = "Dotted",
                Description = "Dotted stroke",
                IconPath = Icons.CircleDotted,
                Type = StrokeType.Dotted
            },
            new()
            {
                Name = "Dashed",
                Description = "Dashed stroke",
                IconPath = Icons.CircleDashed,
                Type = StrokeType.Dashed
            }
        ];
        AvailableFillTypes =
        [
            new() { Name = "None", Description = "Fill - None", IconPath = Icons.Square, Type = FillType.None},
            new() { Name = "Semi", Description = "Fill - Semi", IconPath = Icons.SquareDuotone, Type = FillType.Semi },
            new() { Name = "Solid", Description = "Fill - Solid", IconPath = Icons.SquareFill, Type = FillType.Solid },
        ];
        AvailableShapeTypes =
        [
            new() { Name = "Rectangle", Description = "Rectangle", IconPath = Icons.Square, Type = ShapeType.Rectangle },
            new() { Name = "Ellipse", Description = "Ellipse", IconPath = Icons.Circle, Type = ShapeType.Ellipse}
        ];
        SelectedShapeType = AvailableShapeTypes[0];
    }
    
    public List<ToolListItem<FillType>> AvailableFillTypes
    {
        get => GetValue(AvailableFillTypesProperty);
        set => SetValue(AvailableFillTypesProperty, value);
    }

    public List<ShapeSize> AvailableSizes
    {
        get => GetValue(AvailableSizesProperty);
        set => SetValue(AvailableSizesProperty, value);
    }
    
    public List<ToolListItem<StrokeType>> AvailableStrokeTypes
    {
        get => GetValue(AvailableStrokeTypesProperty);
        set => SetValue(AvailableStrokeTypesProperty, value);
    }
    
    public List<ToolListItem<ShapeType>> AvailableShapeTypes
    {
        get => GetValue(AvailableShapeTypesProperty);
        set => SetValue(AvailableShapeTypesProperty, value);
    }
    
    public ToolListItem<ShapeType> SelectedShapeType
    {
        get => GetValue(SelectedShapeTypeProperty);
        set => SetValue(SelectedShapeTypeProperty, value);
    }

    public List<IBrush> AvailableColors
    {
        get => GetValue(AvailableColorsProperty);
        set => SetValue(AvailableColorsProperty, value);
    }

    public IShapeProperties Properties
    {
        get => GetValue(PropertiesProperty);
        set => SetValue(PropertiesProperty, value);
    }
    
    public bool IsShapePopupOpen
    {
        get => GetValue(IsShapePopupOpenProperty);
        set => SetValue(IsShapePopupOpenProperty, value);
    }
    
    [RelayCommand]
    private void OpenShapePopup()
    {
        IsShapePopupOpen = true;
    }

    private void SelectingItemsControl_OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (e.AddedItems.Count == 0)
            return;
        
        if (e.AddedItems[0] is not ToolListItem<ShapeType> item)
            return;

        if (Properties is null)
            return;
        
        Properties.ShapeType = item.Type;
    }
}