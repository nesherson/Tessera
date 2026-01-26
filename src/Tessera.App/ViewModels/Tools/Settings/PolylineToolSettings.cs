using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using Tessera.App.Enumerations;

namespace Tessera.App.ViewModels;

public partial class PolylineToolSettings : ObservableObject
{
    [ObservableProperty]
    private double _strokeThickness = 3;

    [ObservableProperty]
    private Color _strokeColor = Colors.Black;
    
    [ObservableProperty]
    private PenLineJoin _selectedStrokeJoin  = PenLineJoin.Miter;

    [ObservableProperty]
    private PenLineCap _selectedStrokeCap = PenLineCap.Round;
    
    public List<PenLineJoin> AvailableStrokeJoins { get; } = Enum.GetValues<PenLineJoin>().ToList();
    public List<PenLineCap> AvailableStrokeCaps { get; } = Enum.GetValues<PenLineCap>().ToList();
}