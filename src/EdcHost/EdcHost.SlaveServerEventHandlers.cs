using EdcHost.Games;
using EdcHost.SlaveServers;

namespace EdcHost;

partial class EdcHost : IEdcHost
{
    void HandlePlayerTryAttackEvent(object? sender, PlayerTryAttackEventArgs e)
    {
        try
        {
            // Store the event info to the queue
            this._playerEventQueue.Enqueue(e);

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
            _game.Players[playerId.Value].Attack(e.TargetChunkId / _mapWidth, e.TargetChunkId % _mapWidth);
        }
        catch (Exception exception)
        {
            Serilog.Log.Warning($"Action failed: {exception}");
        }
    }

    void HandlePlayerTryPlaceBlockEvent(object? sender, PlayerTryPlaceBlockEventArgs e)
    {
        try
        {
            // Store the event info to the queue
            this._playerEventQueue.Enqueue(e);

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
            _game.Players[playerId.Value].Place(e.TargetChunkId / _mapWidth, e.TargetChunkId % _mapWidth);
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
            // Store the event info to the queue
            this._playerEventQueue.Enqueue(e);

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

                case ItemKind.PotionOfHealing:
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
