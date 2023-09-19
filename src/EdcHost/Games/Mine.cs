namespace EdcHost.Games;

/// <summary>
/// Mine represents a mine in the game.
/// </summary>
public class Mine : IMine
{
    /// <summary>
    /// The count of accumulated ores.
    /// </summary>
    public int AccumulatedOreCount { get; private set; }

    /// <summary>
    /// The kind of the ore.
    /// </summary>
    public IMine.OreKindType OreKind { get; }

    /// <summary>
    /// The position of the mine.
    /// </summary>
    public IPosition<float> Position { get; }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="oreKind">The ore kind</param>
    /// <param name="position">THe position</param>
    public Mine(IMine.OreKindType oreKind, IPosition<float> position)
    {
        AccumulatedOreCount = 0;
        OreKind = oreKind;
        Position = position;
    }

    /// <summary>
    /// Picks up some ore.
    /// </summary>
    /// <param name="count">The count of ore to pick up.</param>
    public void PickUpOre(int count)
    {
        //TODO: Handle case AccumulatedOreCount < count
        AccumulatedOreCount = Math.Max(0, AccumulatedOreCount - count);
    }

    /// <summary>
    /// Generate ore automaticly.
    /// </summary>
    public void Generate()
    {
        AccumulatedOreCount++;
    }

}
