using System.Collections.Generic;

namespace VoronoiNoiseGenerator.Models;

public interface ISampleGrid : ISampleCollection
{
    double Width { get; }
    double Height { get; }
    public int GridWidth { get; }
    public int GridHeight { get; }

    public bool Occupied(double x, double y);
}