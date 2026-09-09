using System.Collections.Generic;

namespace VoronoiNoiseGenerator.Models;

public interface ISampleCollection
{
    public void Add(double x, double y);
    IEnumerable<Neighbour> Neighbours(double x, double y, double distance);
}