using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Avalonia.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Tessera.App.Enumerations;
using Tessera.App.ViewModels.Results;

namespace Tessera.App.ViewModels;

public partial class CanvasSettingsViewModel : DialogViewModelBase
{
    private double? _gridSpacing;

    [ObservableProperty] 
    private GridType _selectedGridType;
    
    [ObservableProperty]
    private bool _gridSnapping;
    
    [ObservableProperty]
    private IBrush? _selectedGridColor;

    public IEnumerable<GridType> GridTypes => Enum.GetValues<GridType>();
    public Action<CanvasSettingsResult?>? OnResult { get; init; }
    
    [Required(ErrorMessage = "Grid spacing is required.")]
    [Range(10, 125, ErrorMessage = "Spacing must be between 10 and 125.")]
    public double? GridSpacing
    {
        get => _gridSpacing;
        set => SetProperty(ref _gridSpacing, value);
    }

    [RelayCommand]
    private void Save()
    {
        OnResult?.Invoke(new CanvasSettingsResult(GridSpacing ?? 20, 
            SelectedGridType, 
            SelectedGridColor ?? new SolidColorBrush(Colors.Black)));
    }

    [RelayCommand]
    private void Cancel()
    {
        OnResult?.Invoke(null);
    }
}