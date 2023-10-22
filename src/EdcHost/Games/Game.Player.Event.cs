namespace EdcHost.Games;

partial class Game : IGame
{
    /// <summary>
    /// Handle PlayerMoveEvent
    /// </summary>
    /// <param name="sender">Sender of the event</param>
    /// <param name="e">Event args</param>
    void HandlePlayerMoveEvent(object? sender, PlayerMoveEventArgs e)
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
            _logger.Warning($"Action failed: {exception}");
        }
    }

    /// <summary>
    /// Handle PlayerAttackEvent
    /// </summary>
    /// <param name="sender">Sender of the event</param>
    /// <param name="e">Event args</param>
    void HandlePlayerAttackEvent(object? sender, PlayerAttackEventArgs e)
    {
        if (e.Player.IsAlive == false)
        {
            _logger.Warning($"Player {e.Player.PlayerId} is dead. Action rejected.");
            return;
        }
        if (ElapsedTicks - _playerLastAttackTickList[e.Player.PlayerId] < AttackTickInterval(e.Player))
        {
            _logger.Warning(@$"Player {e.Player.PlayerId} has already attacked recently.
                Action rejected.");
            return;
        }
        if (IsAdjacent(ToIntPosition(e.Player.PlayerPosition), ToIntPosition(e.Position)) == false)
        {
            _logger.Warning(@$"Position ({e.Position.X}, {e.Position.Y})
                is not adjacent to player {e.Player.PlayerId}. Action rejected.");
            return;
        }
        if (IsValidPosition(ToIntPosition(e.Position)) == false)
        {
            _logger.Warning(@$"Position ({e.Position.X}, {e.Position.Y}) is not valid.
                Action rejected.");
            return;
        }
        if (IsAdjacent(ToIntPosition(e.Player.PlayerPosition), ToIntPosition(e.Position)) == false)
        {
            _logger.Warning(@$"Position ({e.Position.X}, {e.Position.Y})
                is not adjacent to player {e.Player.PlayerId}. Action rejected.");
            return;
        }

        bool attacked = false;
        for (int i = 0; i < PlayerNum; i++)
        {
            if (Players[i].PlayerId != e.Player.PlayerId && Players[i].IsAlive == true
                && IsSamePosition(ToIntPosition(e.Position),
                    ToIntPosition(Players[i].PlayerPosition)) == true)
            {
                Players[i].Hurt(e.Player.Strength);
                attacked = true;
            }
        }

        if (attacked == true)
        {
            _playerLastAttackTickList[e.Player.PlayerId] = ElapsedTicks;
        }
        else
        {
            if (GameMap.GetChunkAt(ToIntPosition(e.Position)).CanRemoveBlock == false)
            {
                _logger.Warning("Target chunk is empty. Action rejected.");
                return;
            }

            try
            {
                GameMap.GetChunkAt(ToIntPosition(e.Position)).RemoveBlock();
                Players[e.Player.PlayerId].DecreaseWoolCount();
                _playerLastAttackTickList[e.Player.PlayerId] = ElapsedTicks;

                if (GameMap.GetChunkAt(ToIntPosition(e.Position)).IsVoid == true)
                {
                    for (int i = 0; i < PlayerNum; i++)
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
                _logger.Warning($"Action failed: {exception}");
                return;
            }
        }
    }

    /// <summary>
    /// Handle PlayerPlaceEvent
    /// </summary>
    /// <param name="sender">Sender of the event</param>
    /// <param name="e">Event args</param>
    void HandlePlayerPlaceEvent(object? sender, PlayerPlaceEventArgs e)
    {
        if (e.Player.IsAlive == false)
        {
            _logger.Warning($"Player {e.Player.PlayerId} is dead. Action rejected.");
            return;
        }
        if (IsAdjacent(ToIntPosition(e.Player.PlayerPosition), ToIntPosition(e.Position)) == false)
        {
            _logger.Warning(@$"Position ({e.Position.X}, {e.Position.Y})
                is not adjecant to player {e.Player.PlayerId}. Action rejected.");
            return;
        }
        if (IsValidPosition(ToIntPosition(e.Position)) == false)
        {
            _logger.Warning(@$"Position ({e.Position.X}, {e.Position.Y}) is not valid.
                Action rejected.");
            return;
        }
        if (IsAdjacent(ToIntPosition(e.Player.PlayerPosition), ToIntPosition(e.Position)) == false)
        {
            _logger.Warning(@$"Position ({e.Position.X}, {e.Position.Y})
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
                _logger.Warning(@$"The chunk at position ({e.Position.X}, {e.Position.Y})
                    has already reached its maximum height. Action rejected.");
            }
        }
        catch (Exception exception)
        {
            _logger.Warning($"Action failed: {exception}");
            return;
        }
    }

    /// <summary>
    /// Handle PlayerDieEvent
    /// </summary>
    /// <param name="sender">Sender of the event</param>
    /// <param name="e">Event args</param>
    void HandlePlayerDieEvent(object? sender, PlayerDieEventArgs e)
    {
        if (_playerDeathTickList[e.Player.PlayerId] is not null)
        {
            _logger.Warning($"Player {e.Player.PlayerId} is already dead. Action rejected.");
            return;
        }

        _playerDeathTickList[e.Player.PlayerId] = ElapsedTicks;
    }
}
