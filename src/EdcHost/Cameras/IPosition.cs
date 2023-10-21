namespace EdcHost.Cameras;

/// <summary>
/// Position represents a position in 2D space.
/// </summary>
/// <typeparam name="T">The type of the position.</typeparam>
public interface IPosition<T>
{
    /// <summary>
    /// The X coordinate of the position.
    /// </summary>
    T X { get; set; }

    /// <summary>
    /// The Y coordinate of the position.
    /// </summary>
    T Y { get; set; }
}
