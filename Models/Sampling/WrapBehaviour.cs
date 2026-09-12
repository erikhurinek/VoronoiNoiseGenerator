namespace VoronoiNoiseGenerator.Models;

/// <summary>
/// Defines the wrapping behavior for sample grids.
/// </summary>
public enum WrapBehaviour
{
    /// <summary>
    /// Do not wrap the distance or the sample coordinates.
    /// </summary>
    NoWrap,

    /// <summary>
    /// Wrap the distance, but keep the sample coordinates unwrapped.
    /// </summary>
    WrapDistanceUnwrapCoordinates,

    /// <summary>
    /// Wrap both the distance and the sample coordinates.
    /// </summary>
    WrapDistanceAndCoordinates
}