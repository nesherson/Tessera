using Tessera.App.Enumerations;

namespace Tessera.App.Results;

public record CanvasSettingsResult(double GridSpacing, GridType GridType, IBrush GridColor);