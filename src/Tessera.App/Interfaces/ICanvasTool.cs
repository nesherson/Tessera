namespace Tessera.App.Interfaces;

public interface ICanvasTool
{
    void OnPointerPressed(Point p);
    void OnPointerMoved(Point p);
    void OnPointerReleased(Point p);
}