namespace EdcHost.Games;

partial class Game : IGame
{
    /// <summary>
    /// Generate mines according to game config
    /// </summary>
    /// <param name="diamondMines">Diamond mine list</param>
    /// <param name="goldMines">Gold mine list</param>
    /// <param name="ironMines">Iron mine list</param>
    private void GenerateMines(List<Tuple<int, int>>? diamondMines,
        List<Tuple<int, int>>? goldMines, List<Tuple<int, int>>? ironMines)
    {
        IPosition<float>? position = null;
        IMine? mine = null;
        float offset = 0.4f;
        if (diamondMines is not null)
        {
            foreach (Tuple<int, int> minePos in diamondMines)
            {
                position = new Position<float>(
                    offset + (float)minePos.Item1, offset + (float)minePos.Item2
                );
                mine = new Mine(IMine.OreKindType.Diamond, position, 0);
                Mines.Add(mine);
            }
        }
        if (goldMines is not null)
        {
            foreach (Tuple<int, int> minePos in goldMines)
            {
                position = new Position<float>(
                    offset + (float)minePos.Item1, offset + (float)minePos.Item2
                );
                mine = new Mine(IMine.OreKindType.GoldIngot, position, 0);
                Mines.Add(mine);
            }
        }
        if (ironMines is not null)
        {
            foreach (Tuple<int, int> minePos in ironMines)
            {
                position = new Position<float>(
                    offset + (float)minePos.Item1, offset + (float)minePos.Item2
                );
                mine = new Mine(IMine.OreKindType.IronIngot, position, 0);
                Mines.Add(mine);
            }
        }
    }
}
