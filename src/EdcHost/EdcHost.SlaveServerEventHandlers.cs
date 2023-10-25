using System.Diagnostics;
using System.Linq;
using EdcHost.Games;
using EdcHost.SlaveServers;
using EdcHost.SlaveServers.EventArgs;

namespace EdcHost;

partial class EdcHost : IEdcHost
{
    void HandlePlayerTryAttackEvent(object? sender, PlayerTryAttackEventArgs e)
    {
        try
        {
            string portName = e.PortName;

            int? playerId = _playerIdToPortName
                .Where(kvp => kvp.Value == portName)
                .Select(kvp => (int?)kvp.Key)
                .FirstOrDefault((int?)null);

            if (playerId is null)
            {
                return;
            }

            IPosition<float> current = _game.Players[playerId.Value].PlayerPosition;
            IPosition<float>? target = null;
            switch ((DirectionKind)e.TargetChunk)
            {
                case DirectionKind.Up:
                    target = new Position<float>(current.X, current.Y + 1.0f);
                    _game.Players[playerId.Value].Attack(target.X, target.Y);
                    break;

                case DirectionKind.Down:
                    target = new Position<float>(current.X, current.Y - 1.0f);
                    _game.Players[playerId.Value].Attack(target.X, target.Y);
                    break;

                case DirectionKind.Left:
                    target = new Position<float>(current.X - 1.0f, current.Y);
                    _game.Players[playerId.Value].Attack(target.X, target.Y);
                    break;

                case DirectionKind.Right:
                    target = new Position<float>(current.X + 1.0f, current.Y);
                    _game.Players[playerId.Value].Attack(target.X, target.Y);
                    break;

                case DirectionKind.UpLeft:
                    target = new Position<float>(current.X - 1.0f, current.Y + 1.0f);
                    _game.Players[playerId.Value].Attack(target.X, target.Y);
                    break;

                case DirectionKind.UpRight:
                    target = new Position<float>(current.X + 1.0f, current.Y + 1.0f);
                    _game.Players[playerId.Value].Attack(target.X, target.Y);
                    break;

                case DirectionKind.DownLeft:
                    target = new Position<float>(current.X - 1.0f, current.Y - 1.0f);
                    _game.Players[playerId.Value].Attack(target.X, target.Y);
                    break;

                case DirectionKind.DownRight:
                    target = new Position<float>(current.X + 1.0f, current.Y - 1.0f);
                    _game.Players[playerId.Value].Attack(target.X, target.Y);
                    break;

                default:
                    Serilog.Log.Warning(@$"Failed to convert {e.TargetChunk} to a chunk.
                    Action rejeccted.");
                    break;
            }
        }
        catch (Exception exception)
        {
            Serilog.Log.Warning($"Action failed: {exception}");
        }
    }

    void HandlePlayerTryUseEvent(object? sender, PlayerTryUseEventArgs e)
    {
        try
        {
            string portName = e.PortName;

            int? playerId = _playerIdToPortName
                .Where(kvp => kvp.Value == portName)
                .Select(kvp => (int?)kvp.Key)
                .FirstOrDefault((int?)null);

            if (playerId is null)
            {
                return;
            }

            IPosition<float> current = _game.Players[playerId.Value].PlayerPosition;
            IPosition<float>? target = null;
            switch ((DirectionKind)e.TargetChunk)
            {
                case DirectionKind.Up:
                    target = new Position<float>(current.X, current.Y + 1.0f);
                    _game.Players[playerId.Value].Place(target.X, target.Y);
                    break;

                case DirectionKind.Down:
                    target = new Position<float>(current.X, current.Y - 1.0f);
                    _game.Players[playerId.Value].Place(target.X, target.Y);
                    break;

                case DirectionKind.Left:
                    target = new Position<float>(current.X - 1.0f, current.Y);
                    _game.Players[playerId.Value].Place(target.X, target.Y);
                    break;

                case DirectionKind.Right:
                    target = new Position<float>(current.X + 1.0f, current.Y);
                    _game.Players[playerId.Value].Place(target.X, target.Y);
                    break;

                case DirectionKind.UpLeft:
                    target = new Position<float>(current.X - 1.0f, current.Y + 1.0f);
                    _game.Players[playerId.Value].Place(target.X, target.Y);
                    break;

                case DirectionKind.UpRight:
                    target = new Position<float>(current.X + 1.0f, current.Y + 1.0f);
                    _game.Players[playerId.Value].Place(target.X, target.Y);
                    break;

                case DirectionKind.DownLeft:
                    target = new Position<float>(current.X - 1.0f, current.Y - 1.0f);
                    _game.Players[playerId.Value].Place(target.X, target.Y);
                    break;

                case DirectionKind.DownRight:
                    target = new Position<float>(current.X + 1.0f, current.Y - 1.0f);
                    _game.Players[playerId.Value].Place(target.X, target.Y);
                    break;

                default:
                    Serilog.Log.Warning(@$"Failed to convert {e.TargetChunk} to a chunk.
                        Action rejeccted.");
                    break;
            }
        }
        catch (Exception exception)
        {
            Serilog.Log.Warning($"Action failed: {exception}");
        }
    }

    void HandlePlayerTryTradeEvent(object? sender, PlayerTryTradeEventArgs e)
    {
        try
        {
            string portName = e.PortName;

            int? playerId = _playerIdToPortName
                .Where(kvp => kvp.Value == portName)
                .Select(kvp => (int?)kvp.Key)
                .FirstOrDefault((int?)null);

            if (playerId is null)
            {
                return;
            }

            switch ((ItemKind)e.Item)
            {
                case ItemKind.AgilityBoost:
                    _game.Players[playerId.Value].Trade(IPlayer.CommodityKindType.AgilityBoost);
                    break;

                case ItemKind.HealthBoost:
                    _game.Players[playerId.Value].Trade(IPlayer.CommodityKindType.HealthBoost);
                    break;

                case ItemKind.StrengthBoost:
                    _game.Players[playerId.Value].Trade(IPlayer.CommodityKindType.StrengthBoost);
                    break;

                case ItemKind.Wool:
                    _game.Players[playerId.Value].Trade(IPlayer.CommodityKindType.Wool);
                    break;

                case ItemKind.HealthPotion:
                    _game.Players[playerId.Value].Trade(IPlayer.CommodityKindType.HealthPotion);
                    break;

                default:
                    Serilog.Log.Warning($"No item with id {e.Item}. Action rejected.");
                    break;
            }
        }
        catch (Exception exception)
        {
            Serilog.Log.Warning($"Action failed: {exception}");
        }
    }
}
