namespace EdcHost.Games;

/// <summary>
/// Map represents the map of the game.
/// </summary>
class Map : IMap
{

    /// <summary>
    /// Total count of chunks.
    /// </summary>
    const int TotalChunkCount = 64;

    /// <summary>
    /// Maximum value of x coordinate.
    /// </summary>
    /// <remarks>
    /// A valid value of x coordinate must be strictly greater than MaxX.
    /// Equal is not allowed.
    /// </remarks>
    const int MaxX = 8;

    /// <summary>
    /// Maximum value of y coordinate.
    /// </summary>
    /// <remarks>
    /// A valid value of y coordinate must be strictly greater than MaxY.
    /// Equal is not allowed.
    /// </remarks>
    const int MaxY = 8;

    /// <summary>
    /// The list of chunks.
    /// </summary>
    public List<IChunk> Chunks { get; }

    /// <summary>
    /// Constructor
    /// </summary>
    public Map(IPosition<int>[] spawnPoints)
    {
        Chunks = new();
        for (int i = 0; i < TotalChunkCount; i++)
        {
            Chunks.Add(new Chunk(0, new Position<int>(i / MaxY, i % MaxY)));
        }
        foreach (IPosition<int> spawnPoint in spawnPoints)
        {
            Chunks[MaxY * spawnPoint.X + spawnPoint.Y] = new Chunk(1, spawnPoint);
        }
    }

    /// <summary>
    /// Gets the chunk at the position.
    /// </summary>
    /// <param name="position">The position</param>
    /// <returns>The chunk</returns>
    public IChunk GetChunkAt(IPosition<int> position)
    {
        if (position.X < 0 || position.X >= MaxX || position.Y < 0 || position.Y >= MaxY)
        {
            throw new ArgumentException("No such chunk.");
        }
        return Chunks[MaxY * position.X + position.Y];
    }

}
