using Avalonia.Input;

namespace Tessera.App.Interfaces;

public interface ICanvasTool
{
    void OnPointerPressed(Point screenPoint, KeyModifiers modifiers);
    void OnPointerMoved(Point screenPoint);
    void OnPointerReleased(Point screenPoint);
    void OnActivated();
    void OnDeactivated();
}