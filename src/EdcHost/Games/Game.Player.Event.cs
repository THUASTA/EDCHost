namespace EdcHost.Games;

public partial class Game : IGame
{
    /// <summary>
    /// Handle PlayerMoveEvent
    /// </summary>
    /// <param name="sender">Sender of the event</param>
    /// <param name="e">Event args</param>
    private void HandlePlayerMoveEvent(object? sender, PlayerMoveEventArgs e)
    {
        if (_startTime is null)
        {
            Serilog.Log.Warning("The game is not started yet. Action rejected.");
            return;
        }

        try
        {
            //TODO: Respawn dead player

            //Kill fallen player
            if (e.Player.IsAlive == true && IsValidPosition(ToIntPosition(e.Position)) == false)
            {
                e.Player.Hurt(InstantDeathDamage);
                return;
            }
            if (e.Player.IsAlive == true && _map.GetChunkAt(ToIntPosition(e.Position)).IsVoid == true)
            {
                e.Player.Hurt(InstantDeathDamage);
                return;
            }
        }
        catch (Exception exception)
        {
            Serilog.Log.Warning($"Action failed: {exception}");
        }
    }

    /// <summary>
    /// Handle PlayerAttackEvent
    /// </summary>
    /// <param name="sender">Sender of the event</param>
    /// <param name="e">Event args</param>
    private void HandlePlayerAttackEvent(object? sender, PlayerAttackEventArgs e)
    {
        if (_startTime is null)
        {
            Serilog.Log.Warning("The game is not started yet. Action rejected.");
            return;
        }
        if (e.Player.IsAlive == false)
        {
            Serilog.Log.Warning($"Player {e.Player.PlayerId} is dead. Action rejected.");
            return;
        }
        if (DateTime.Now - _playerLastAttackTime[e.Player.PlayerId] < AttackTimeInterval(e.Player))
        {
            Serilog.Log.Warning(@$"Player {e.Player.PlayerId} has already attacked recently.
                Action rejected.");
            return;
        }
        if (IsAdjacant(ToIntPosition(e.Player.PlayerPosition), ToIntPosition(e.Position)) == false)
        {
            Serilog.Log.Warning(@$"Position ({e.Position.X}, {e.Position.Y})
                is not adjacant to player {e.Player.PlayerId}. Action rejected.");
            return;
        }
        if (IsValidPosition(ToIntPosition(e.Position)) == false)
        {
            Serilog.Log.Warning(@$"Position ({e.Position.X}, {e.Position.Y}) is not valid.
                Action rejected.");
            return;
        }

        if (Opponent(e.Player).IsAlive == true && IsSamePosition(
            ToIntPosition(e.Position), ToIntPosition(Opponent(e.Player).PlayerPosition)) == true)
        {
            //Attack opponent
            _players[Opponent(e.Player).PlayerId].Hurt(e.Player.Strength);
            _playerLastAttackTime[e.Player.PlayerId] = DateTime.Now;
            return;
        }
        else
        {
            if (_map.GetChunkAt(ToIntPosition(e.Position)).CanRemoveBlock == false)
            {
                Serilog.Log.Warning("Target chunk is empty. Action rejected.");
                return;
            }

            try
            {
                _map.GetChunkAt(ToIntPosition(e.Position)).RemoveBlock();
                _playerLastAttackTime[e.Player.PlayerId] = DateTime.Now;
                if (_map.GetChunkAt(ToIntPosition(e.Position)).IsVoid == true)
                {
                    for (int i = 0; i < 2; i++)
                    {
                        if (_players[i].HasBed == true && IsSamePosition(
                            ToIntPosition(_players[i].SpawnPoint), ToIntPosition(e.Position)) == true)
                        {
                            //TODO: Destroy the player's bed
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                Serilog.Log.Warning($"Action failed: {exception}");
                return;
            }
        }
    }

    /// <summary>
    /// Handle PlayerPlaceEvent
    /// </summary>
    /// <param name="sender">Sender of the event</param>
    /// <param name="e">Event args</param>
    private void HandlePlayerPlaceEvent(object? sender, PlayerPlaceEventArgs e)
    {
        if (_startTime is null)
        {
            Serilog.Log.Warning("The game is not started yet. Action rejected.");
            return;
        }
        if (e.Player.IsAlive == false)
        {
            Serilog.Log.Warning($"Player {e.Player.PlayerId} is dead. Action rejected.");
            return;
        }
        if (IsAdjacant(ToIntPosition(e.Player.PlayerPosition), ToIntPosition(e.Position)) == false)
        {
            Serilog.Log.Warning(@$"Position ({e.Position.X}, {e.Position.Y})
                is not adjacant to player {e.Player.PlayerId}. Action rejected.");
            return;
        }
        if (IsValidPosition(ToIntPosition(e.Position)) == false)
        {
            Serilog.Log.Warning(@$"Position ({e.Position.X}, {e.Position.Y}) is not valid.
                Action rejected.");
            return;
        }

        try
        {
            if (_map.GetChunkAt(ToIntPosition(e.Position)).CanPlaceBlock == true)
            {
                _map.GetChunkAt(ToIntPosition(e.Position)).PlaceBlock();
            }
            else
            {
                Serilog.Log.Warning(@$"The chunk at position ({e.Position.X}, {e.Position.Y})
                    has already reached its maximum height. Action rejected.");
            }
        }
        catch (Exception exception)
        {
            Serilog.Log.Warning($"Action failed: {exception}");
            return;
        }
    }

    /// <summary>
    /// Handle PlayerDieEvent
    /// </summary>
    /// <param name="sender">Sender of the event</param>
    /// <param name="e">Event args</param>
    private void HandlePlayerDieEvent(object? sender, PlayerDieEventArgs e)
    {
        if (_startTime is null)
        {
            Serilog.Log.Warning("The game is not started yet. Action rejected.");
            return;
        }
        if (_playerDeathTime[e.Player.PlayerId] is not null)
        {
            Serilog.Log.Warning($"Player {e.Player.PlayerId} is already dead. Action rejected.");
            return;
        }

        _playerDeathTime[e.Player.PlayerId] = DateTime.Now;
    }
}
