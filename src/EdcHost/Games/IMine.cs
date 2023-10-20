namespace EdcHost.Games;

/// <summary>
/// Mine represents a mine in the game.
/// </summary>
public interface IMine
{
    /// <summary>
    /// The ore kind.
    /// </summary>
    public enum OreKindType
    {
        IronIngot,
        GoldIngot,
        Diamond,
    }

    /// <summary>
    /// The count of accumulated ores.
    /// </summary>
    public int AccumulatedOreCount { get; }

    /// <summary>
    /// The kind of the ore.
    /// </summary>
    public OreKindType OreKind { get; }

    /// <summary>
    /// The position of the mine.
    /// </summary>
    public IPosition<float> Position { get; }

    /// <summary>
    /// How much time required to generate ore.
    /// </summary>
    public int AccumulateOreInterval { get; }

    /// <summary>
    /// Last time ore generated.
    /// </summary>
    public int LastOreGeneratedTick { get; }

    /// <summary>
    /// Picks up some ore.
    /// </summary>
    /// <param name="count">The count of ore to pick up.</param>
    public void PickUpOre(int count);

    /// <summary>
    /// Generate ore automaticly.
    /// </summary>
    public void GenerateOre(int tick);
}
