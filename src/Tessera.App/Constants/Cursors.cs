using Avalonia.Media.Imaging;
using Avalonia.Platform;

namespace Tessera.App.Constants;

public static class Cursors
{
    static Cursors()
    {
        PalmCursorBitmap = LoadScaled("avares://Tessera.App/Assets/Cursors/palm.png", 20, 20);
        DragCursorBitmap = LoadScaled("avares://Tessera.App/Assets/Cursors/drag.png", 20, 20);
    }
    
    public static Bitmap PalmCursorBitmap { get; }
    public static Bitmap DragCursorBitmap { get; }
    
    private static Bitmap LoadScaled(string uri, int width, int height)
    {
        using var stream = AssetLoader.Open(new Uri(uri));
        using var original = new Bitmap(stream);
        return original.CreateScaledBitmap(new PixelSize(width, height));
    }
}