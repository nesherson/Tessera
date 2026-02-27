using System.Threading.Tasks;
using Tessera.App.Enumerations;
using Tessera.App.Results;

namespace Tessera.App.Messages;

public class ShowCanvasSettingsDialogMessage
{
    public ShowCanvasSettingsDialogMessage(double gridSpacing, GridType gridType)
    {
        GridSpacing = gridSpacing;
        GridType = gridType;
    }
    
    public TaskCompletionSource<CanvasSettingsResult?> Tcs { get; } = new();
    public double GridSpacing { get; }
    public GridType GridType { get; }
}