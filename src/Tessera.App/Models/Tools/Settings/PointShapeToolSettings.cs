using Avalonia.Collections;
using Tessera.App.Interfaces;

namespace Tessera.App.Models;

public class PointShapeToolSettings : ToolSettingsBase, IShapeProperties
{
    
    public AvaloniaList<double> StrokeDashArray { get; set; }
}