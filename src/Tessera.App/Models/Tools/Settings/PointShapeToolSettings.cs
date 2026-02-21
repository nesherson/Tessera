using System.Linq;

namespace Tessera.App.Models;

public class PointShapeToolSettings : BaseToolSettings
{
    public PointShapeToolSettings()
    {
        Size = AvailableSizes.First();
        Color = AvailableColors.First();
    }
}