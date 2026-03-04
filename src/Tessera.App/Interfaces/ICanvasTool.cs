namespace Tessera.App.Interfaces;

public interface ICanvasTool
{
    void OnPointerPressed(Point screenPoint);
    void OnPointerMoved(Point screenPoint);
    void OnPointerReleased(Point screenPoint);
}