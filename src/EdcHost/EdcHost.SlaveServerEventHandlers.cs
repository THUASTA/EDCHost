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
            switch ((Directions)e.TargetChunk)
            {
                case Directions.Up:
                    target = new Position<float>(current.X, current.Y + 1.0f);
                    _game.Players[playerId.Value].Attack(target.X, target.Y);
                    break;

                case Directions.Down:
                    target = new Position<float>(current.X, current.Y - 1.0f);
                    _game.Players[playerId.Value].Attack(target.X, target.Y);
                    break;

                case Directions.Left:
                    target = new Position<float>(current.X - 1.0f, current.Y);
                    _game.Players[playerId.Value].Attack(target.X, target.Y);
                    break;

                case Directions.Right:
                    target = new Position<float>(current.X + 1.0f, current.Y);
                    _game.Players[playerId.Value].Attack(target.X, target.Y);
                    break;

                case Directions.UpLeft:
                    target = new Position<float>(current.X - 1.0f, current.Y + 1.0f);
                    _game.Players[playerId.Value].Attack(target.X, target.Y);
                    break;

                case Directions.UpRight:
                    target = new Position<float>(current.X + 1.0f, current.Y + 1.0f);
                    _game.Players[playerId.Value].Attack(target.X, target.Y);
                    break;

                case Directions.DownLeft:
                    target = new Position<float>(current.X - 1.0f, current.Y - 1.0f);
                    _game.Players[playerId.Value].Attack(target.X, target.Y);
                    break;

                case Directions.DownRight:
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
            switch ((Directions)e.TargetChunk)
            {
                case Directions.Up:
                    target = new Position<float>(current.X, current.Y + 1.0f);
                    _game.Players[playerId.Value].Place(target.X, target.Y);
                    break;

                case Directions.Down:
                    target = new Position<float>(current.X, current.Y - 1.0f);
                    _game.Players[playerId.Value].Place(target.X, target.Y);
                    break;

                case Directions.Left:
                    target = new Position<float>(current.X - 1.0f, current.Y);
                    _game.Players[playerId.Value].Place(target.X, target.Y);
                    break;

                case Directions.Right:
                    target = new Position<float>(current.X + 1.0f, current.Y);
                    _game.Players[playerId.Value].Place(target.X, target.Y);
                    break;

                case Directions.UpLeft:
                    target = new Position<float>(current.X - 1.0f, current.Y + 1.0f);
                    _game.Players[playerId.Value].Place(target.X, target.Y);
                    break;

                case Directions.UpRight:
                    target = new Position<float>(current.X + 1.0f, current.Y + 1.0f);
                    _game.Players[playerId.Value].Place(target.X, target.Y);
                    break;

                case Directions.DownLeft:
                    target = new Position<float>(current.X - 1.0f, current.Y - 1.0f);
                    _game.Players[playerId.Value].Place(target.X, target.Y);
                    break;

                case Directions.DownRight:
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
            switch ((ItemList)e.Item)
            {
                //TODO: Trade

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
