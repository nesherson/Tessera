using System.Collections.Generic;
using System.Linq;
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

    private List<IBrush> _availableColors;
    public static readonly DirectProperty<CanvasToolSettings, List<IBrush>> AvailableColorsProperty =
        AvaloniaProperty.RegisterDirect<CanvasToolSettings, List<IBrush>>(
            nameof(AvailableColors), o => o.AvailableColors, (o, v) => o.AvailableColors = v);

    private List<ShapeSize> _availableSizes;
    public static readonly DirectProperty<CanvasToolSettings, List<ShapeSize>> AvailableSizesProperty =
        AvaloniaProperty.RegisterDirect<CanvasToolSettings, List<ShapeSize>>(
            nameof(AvailableSizes), o => o.AvailableSizes, (o, v) => o.AvailableSizes = v);

    private List<ToolListItem<StrokeType>> _availableStrokeTypes;
    public static readonly DirectProperty<CanvasToolSettings, List<ToolListItem<StrokeType>>> AvailableStrokeTypesProperty =
        AvaloniaProperty.RegisterDirect<CanvasToolSettings, List<ToolListItem<StrokeType>>>(
            nameof(AvailableStrokeTypes), o => o.AvailableStrokeTypes, (o, v) => o.AvailableStrokeTypes = v);

    private List<ToolListItem<FillType>> _availableFillTypes;
    public static readonly DirectProperty<CanvasToolSettings, List<ToolListItem<FillType>>> AvailableFillTypesProperty =
        AvaloniaProperty.RegisterDirect<CanvasToolSettings, List<ToolListItem<FillType>>>(
            nameof(AvailableFillTypes), o => o.AvailableFillTypes, (o, v) => o.AvailableFillTypes = v);

    private List<ToolListItem<ShapeType>> _availableShapeTypes;
    public static readonly DirectProperty<CanvasToolSettings, List<ToolListItem<ShapeType>>> AvailableShapeTypesProperty =
        AvaloniaProperty.RegisterDirect<CanvasToolSettings, List<ToolListItem<ShapeType>>>(
            nameof(AvailableShapeTypes), o => o.AvailableShapeTypes, (o, v) => o.AvailableShapeTypes = v);

    private bool _isShapePopupOpen;
    public static readonly DirectProperty<CanvasToolSettings, bool> IsShapePopupOpenProperty =
        AvaloniaProperty.RegisterDirect<CanvasToolSettings, bool>(
            nameof(IsShapePopupOpen), o => o.IsShapePopupOpen, (o, v) => o.IsShapePopupOpen = v);

    private ToolListItem<ShapeType> _selectedShapeType;
    public static readonly DirectProperty<CanvasToolSettings, ToolListItem<ShapeType>> SelectedShapeTypeProperty =
        AvaloniaProperty.RegisterDirect<CanvasToolSettings, ToolListItem<ShapeType>>(
            nameof(SelectedShapeType), o => o.SelectedShapeType, (o, v) => o.SelectedShapeType = v);

    private bool _showStrokeTypeOptions = true;
    public static readonly DirectProperty<CanvasToolSettings, bool> ShowStrokeTypeOptionsProperty =
        AvaloniaProperty.RegisterDirect<CanvasToolSettings, bool>(
            nameof(ShowStrokeTypeOptions), o => o.ShowStrokeTypeOptions, (o, v) => o.ShowStrokeTypeOptions = v);

    private bool _showShapeOptions;
    public static readonly DirectProperty<CanvasToolSettings, bool> ShowShapeOptionsProperty =
        AvaloniaProperty.RegisterDirect<CanvasToolSettings, bool>(
            nameof(ShowShapeOptions), o => o.ShowShapeOptions, (o, v) => o.ShowShapeOptions = v);
    
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
    
    public List<IBrush> AvailableColors
    {
        get => _availableColors;
        set => SetAndRaise(AvailableColorsProperty, ref _availableColors, value);
    }

    public List<ShapeSize> AvailableSizes
    {
        get => _availableSizes;
        set => SetAndRaise(AvailableSizesProperty, ref _availableSizes, value);
    }

    public List<ToolListItem<StrokeType>> AvailableStrokeTypes
    {
        get => _availableStrokeTypes;
        set => SetAndRaise(AvailableStrokeTypesProperty, ref _availableStrokeTypes, value);
    }

    public List<ToolListItem<FillType>> AvailableFillTypes
    {
        get => _availableFillTypes;
        set => SetAndRaise(AvailableFillTypesProperty, ref _availableFillTypes, value);
    }

    public List<ToolListItem<ShapeType>> AvailableShapeTypes
    {
        get => _availableShapeTypes;
        set => SetAndRaise(AvailableShapeTypesProperty, ref _availableShapeTypes, value);
    }

    public ToolListItem<ShapeType> SelectedShapeType
    {
        get => _selectedShapeType;
        set => SetAndRaise(SelectedShapeTypeProperty, ref _selectedShapeType, value);
    }

    public bool ShowStrokeTypeOptions
    {
        get => _showStrokeTypeOptions;
        set => SetAndRaise(ShowStrokeTypeOptionsProperty, ref _showStrokeTypeOptions, value);
    }

    public bool ShowShapeOptions
    {
        get => _showShapeOptions;
        set => SetAndRaise(ShowShapeOptionsProperty, ref _showShapeOptions, value);
    }

    public bool IsShapePopupOpen
    {
        get => _isShapePopupOpen;
        set => SetAndRaise(IsShapePopupOpenProperty, ref _isShapePopupOpen, value);
    }

    public IShapeProperties Properties
    {
        get => GetValue(PropertiesProperty);
        set => SetValue(PropertiesProperty, value);
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

    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);
        
        if (change.Property == PropertiesProperty)
        {
            ShowStrokeTypeOptions = Properties is not PointShapeToolSettings and not TextShapeToolSettings
                                    || (Properties is MultiShapePropertyProxy proxy1
                                    && proxy1.Shapes.Any(s => s is RectangleShape or EllipseShape or PolylineShape or LineShape));
            ShowShapeOptions = Properties is ShapeToolSettings
                || (Properties is MultiShapePropertyProxy proxy2
                    && proxy2.Shapes.Any(s => s is RectangleShape or EllipseShape));
        }
    }
}