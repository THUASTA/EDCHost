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

    public TimeSpan AccumulateOreInterval
    {
        get => OreKind switch
        {
            IMine.OreKindType.IronIngot => TimeSpan.FromSeconds(1),
            IMine.OreKindType.GoldIngot => TimeSpan.FromSeconds(4),
            IMine.OreKindType.Diamond => TimeSpan.FromSeconds(16),
            _ => throw new ArgumentOutOfRangeException(
                nameof(OreKind), $"No ore kind {OreKind}")
        };
    }

    /// <summary>
    /// Last time ore generated.
    /// </summary>
    public DateTime LastOreGeneratedTime { get; private set; }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="oreKind">The ore kind</param>
    /// <param name="position">The position</param>
    public Mine(IMine.OreKindType oreKind, IPosition<float> position)
    {
        AccumulatedOreCount = 0;
        OreKind = oreKind;
        Position = position;
        LastOreGeneratedTime = DateTime.Now;
    }

    /// <summary>
    /// Picks up some ore.
    /// </summary>
    /// <param name="count">The count of ore to pick up.</param>
    public void PickUpOre(int count)
    {
        if(AccumulatedOreCount < count)
        {
            throw new InvalidOperationException("No enough ore.");
        }
        AccumulatedOreCount -= count;
    }

    /// <summary>
    /// Generate ore automaticly.
    /// </summary>
    public void GenerateOre()
    {
        AccumulatedOreCount++;
        LastOreGeneratedTime = DateTime.Now;
    }

}
