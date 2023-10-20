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
        try
        {
            if (e.Player.IsAlive == false && e.Player.HasBed == true
                && ElapsedTicks - _playerDeathTickList[e.Player.PlayerId] > TicksBeforeRespawn
                && IsSamePosition(
                    ToIntPosition(e.Position), ToIntPosition(e.Player.SpawnPoint)
                    ) == true)
            {
                Players[e.Player.PlayerId].Spawn(e.Player.MaxHealth);
                _playerDeathTickList[e.Player.PlayerId] = null;
            }

            //Kill fallen player. Use 'if' instead of 'else if' to avoid fake spawn.
            if (e.Player.IsAlive == true && IsValidPosition(ToIntPosition(e.Position)) == false)
            {
                Players[e.Player.PlayerId].Hurt(InstantDeathDamage);
                return;
            }
            if (e.Player.IsAlive == true && GameMap.GetChunkAt(ToIntPosition(e.Position)).IsVoid == true)
            {
                Players[e.Player.PlayerId].Hurt(InstantDeathDamage);
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
        if (e.Player.IsAlive == false)
        {
            Serilog.Log.Warning($"Player {e.Player.PlayerId} is dead. Action rejected.");
            return;
        }
        if (ElapsedTicks - _playerLastAttackTickList[e.Player.PlayerId] < AttackTickInterval(e.Player))
        {
            Serilog.Log.Warning(@$"Player {e.Player.PlayerId} has already attacked recently.
                Action rejected.");
            return;
        }
        if (IsAdjacent(ToIntPosition(e.Player.PlayerPosition), ToIntPosition(e.Position)) == false)
        {
            Serilog.Log.Warning(@$"Position ({e.Position.X}, {e.Position.Y})
                is not adjacent to player {e.Player.PlayerId}. Action rejected.");
            return;
        }
        if (IsValidPosition(ToIntPosition(e.Position)) == false)
        {
            Serilog.Log.Warning(@$"Position ({e.Position.X}, {e.Position.Y}) is not valid.
                Action rejected.");
            return;
        }
        if (IsAdjacent(ToIntPosition(e.Player.PlayerPosition), ToIntPosition(e.Position)) == false)
        {
            Serilog.Log.Warning(@$"Position ({e.Position.X}, {e.Position.Y})
                is not adjacent to player {e.Player.PlayerId}. Action rejected.");
            return;
        }

        if (Opponent(e.Player).IsAlive == true && IsSamePosition(
            ToIntPosition(e.Position), ToIntPosition(Opponent(e.Player).PlayerPosition)) == true)
        {
            //Attack opponent
            Players[Opponent(e.Player).PlayerId].Hurt(e.Player.Strength);
            _playerLastAttackTickList[e.Player.PlayerId] = ElapsedTicks;
            return;
        }
        else
        {
            if (GameMap.GetChunkAt(ToIntPosition(e.Position)).CanRemoveBlock == false)
            {
                Serilog.Log.Warning("Target chunk is empty. Action rejected.");
                return;
            }

            try
            {
                GameMap.GetChunkAt(ToIntPosition(e.Position)).RemoveBlock();
                Players[e.Player.PlayerId].DecreaseWoolCount();
                _playerLastAttackTickList[e.Player.PlayerId] = ElapsedTicks;

                if (GameMap.GetChunkAt(ToIntPosition(e.Position)).IsVoid == true)
                {
                    for (int i = 0; i < 2; i++)
                    {
                        if (Players[i].HasBed == true && IsSamePosition(
                            ToIntPosition(Players[i].SpawnPoint), ToIntPosition(e.Position)) == true)
                        {
                            Players[i].DestroyBed();
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
        if (e.Player.IsAlive == false)
        {
            Serilog.Log.Warning($"Player {e.Player.PlayerId} is dead. Action rejected.");
            return;
        }
        if (IsAdjacent(ToIntPosition(e.Player.PlayerPosition), ToIntPosition(e.Position)) == false)
        {
            Serilog.Log.Warning(@$"Position ({e.Position.X}, {e.Position.Y})
                is not adjecant to player {e.Player.PlayerId}. Action rejected.");
            return;
        }
        if (IsValidPosition(ToIntPosition(e.Position)) == false)
        {
            Serilog.Log.Warning(@$"Position ({e.Position.X}, {e.Position.Y}) is not valid.
                Action rejected.");
            return;
        }
        if (IsAdjacent(ToIntPosition(e.Player.PlayerPosition), ToIntPosition(e.Position)) == false)
        {
            Serilog.Log.Warning(@$"Position ({e.Position.X}, {e.Position.Y})
                is not adjacent to player {e.Player.PlayerId}. Action rejected.");
            return;
        }

        try
        {
            if (GameMap.GetChunkAt(ToIntPosition(e.Position)).CanPlaceBlock == true)
            {
                GameMap.GetChunkAt(ToIntPosition(e.Position)).PlaceBlock();
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
        if (_playerDeathTickList[e.Player.PlayerId] is not null)
        {
            Serilog.Log.Warning($"Player {e.Player.PlayerId} is already dead. Action rejected.");
            return;
        }

        _playerDeathTickList[e.Player.PlayerId] = ElapsedTicks;
    }
}
