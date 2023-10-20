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

    public int AccumulateOreInterval
    {
        get => OreKind switch
        {
            IMine.OreKindType.IronIngot => 20,
            IMine.OreKindType.GoldIngot => 80,
            IMine.OreKindType.Diamond => 320,
            _ => throw new ArgumentOutOfRangeException(
                nameof(OreKind), $"No ore kind {OreKind}")
        };
    }

    /// <summary>
    /// Last time ore generated.
    /// </summary>
    public int LastOreGeneratedTick { get; private set; }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="oreKind">The ore kind</param>
    /// <param name="position">The position</param>
    public Mine(IMine.OreKindType oreKind, IPosition<float> position, int tick)
    {
        AccumulatedOreCount = 0;
        OreKind = oreKind;
        Position = position;
        LastOreGeneratedTick = tick;
    }

    /// <summary>
    /// Picks up some ore.
    /// </summary>
    /// <param name="count">The count of ore to pick up.</param>
    public void PickUpOre(int count)
    {
        if (AccumulatedOreCount < count)
        {
            throw new InvalidOperationException("No enough ore.");
        }
        AccumulatedOreCount -= count;
    }

    /// <summary>
    /// Generate ore automaticly.
    /// </summary>
    public void GenerateOre(int tick)
    {
        AccumulatedOreCount++;
        LastOreGeneratedTick = tick;
    }

}
