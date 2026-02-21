using System.Linq;

namespace Tessera.App.Models;

public class PolylineShapeToolSettings : ToolSettingsBase
{
    public PolylineShapeToolSettings()
    {
        Size = AvailableSizes.First();
        Color = AvailableColors.First();
        StrokeType = AvailableStrokeTypes.First();
    }
}