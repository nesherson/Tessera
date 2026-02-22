namespace Tessera.App.Helpers;

public class GeometryHelpers
{
    public static bool SegmentsIntersect(Point a, Point b, Point c, Point d)
    {
        var d1 = Cross(c, d, a);
        var d2 = Cross(c, d, b);
        var d3 = Cross(a, b, c);
        var d4 = Cross(a, b, d);

        if (((d1 > 0 && d2 < 0) || (d1 < 0 && d2 > 0)) &&
            ((d3 > 0 && d4 < 0) || (d3 < 0 && d4 > 0)))
            return true;
        
        // Collinear cases — point lies on the segment
        if (d1 == 0 && OnSegment(c, d, a)) return true;
        if (d2 == 0 && OnSegment(c, d, b)) return true;
        if (d3 == 0 && OnSegment(a, b, c)) return true;
        if (d4 == 0 && OnSegment(a, b, d)) return true;

        return false;
    }
    
    // Cross product of vectors (b-a) and (c-a)
    // Positive = c is left of AB, Negative = right, Zero = collinear
    private static double Cross(Point a, Point b, Point c)
    {
        return (b.X - a.X) * (c.Y - a.Y) - (b.Y - a.Y) * (c.X - a.X);
    }
    
    
    // Is point p on segment (a, b), assuming they're collinear?
    private static bool OnSegment(Point a, Point b, Point p)
    {
        return Math.Min(a.X, b.X) <= p.X && p.X <= Math.Max(a.X, b.X)
                                         && Math.Min(a.Y, b.Y) <= p.Y && p.Y <= Math.Max(a.Y, b.Y);
    }
}