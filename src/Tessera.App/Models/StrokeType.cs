using Avalonia.Collections;

namespace Tessera.App.Models;

public struct StrokeType
{
    public string Name { get; set; }
    public string Description { get; set; }
    public AvaloniaList<double> DashArray { get; set; }
    public string IconPath { get; set; }
}