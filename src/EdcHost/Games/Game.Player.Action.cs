namespace EdcHost.Games;

public partial class Game : IGame
{
    //TODO: Build actions from EventArgs

    /// <summary>
    /// Player attack action.
    /// </summary>
    public class PlayerAttackAction
    {
        public IPlayer Player { get; }
        public IPosition<int> Target { get; }

        public PlayerAttackAction(IPlayer player, IPosition<int> target)
        {
            Player = player;
            Target = target;
        }
    }

    /// <summary>
    /// Player trade action.
    /// </summary>
    public class PlayerTradeAction
    {
        public IPlayer Player { get; }
        public IPlayer.CommodityKindType CommodityKind { get; }

        public PlayerTradeAction(IPlayer player, IPlayer.CommodityKindType commodityKind)
        {
            Player = player;
            CommodityKind = commodityKind;
        }
    }

    /// <summary>
    /// Player place block action.
    /// </summary>
    public class PlayerPlaceBlockAction
    {
        public IPlayer Player { get; }
        public IPosition<int> Target { get; }

        public PlayerPlaceBlockAction(IPlayer player, IPosition<int> target)
        {
            Player = player;
            Target = target;
        }
    }

    /// <summary>
    /// Player move action.
    /// </summary>
    public class PlayerMoveAction
    {
        public IPlayer Player { get; }
        public IPosition<float> OldPosition { get; }
        public IPosition<float> NewPosition { get; }

        public PlayerMoveAction(
            IPlayer player, IPosition<float> oldPosition, IPosition<float> newPosition)
        {
            Player = player;
            OldPosition = oldPosition;
            NewPosition = newPosition;
        }
    }
}
