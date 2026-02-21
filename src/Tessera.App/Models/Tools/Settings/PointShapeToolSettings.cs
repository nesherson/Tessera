using System.Linq;

namespace Tessera.App.Models;

public class PointShapeToolSettings : ToolSettingsBase
{
    public PointShapeToolSettings()
    {
        Size = AvailableSizes.First();
        Color = AvailableColors.First();
    }
}