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
        if (CurrentStage != IGame.Stage.Running && CurrentStage != IGame.Stage.Battling)
        {
            // Do not handle player events when game is not running
            return;
        }

        try
        {
            lock (this)
            {
                if (e.Player.IsAlive == false && e.Player.HasBed == true
                    && ElapsedTicks - _playerDeathTickList[e.Player.PlayerId] > TicksBeforeRespawn
                    && IsSamePosition(
                        ToIntPosition(e.Position), e.Player.SpawnPoint
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
        }
        catch (Exception exception)
        {
            _logger.Error($"Action failed: {exception}");
        }
    }

    /// <summary>
    /// Handle PlayerAttackEvent
    /// </summary>
    /// <param name="sender">Sender of the event</param>
    /// <param name="e">Event args</param>
    void HandlePlayerAttackEvent(object? sender, PlayerAttackEventArgs e)
    {
        if (CurrentStage != IGame.Stage.Running && CurrentStage != IGame.Stage.Battling)
        {
            return;
        }

        lock (this)
        {
            if (e.Player.IsAlive == false)
            {
                _logger.Error($"Player {e.Player.PlayerId} is dead. Action rejected.");
                return;
            }
            if (ElapsedTicks - _playerLastAttackTickList[e.Player.PlayerId] < AttackTickInterval(e.Player))
            {
                _logger.Error(@$"Player {e.Player.PlayerId} has already attacked recently.
                Action rejected.");
                return;
            }

            if (IsAdjacent(ToIntPosition(e.Player.PlayerPosition), e.Position) == false)
            {
                _logger.Error(@$"Position ({e.Position.X}, {e.Position.Y})
                is not adjacent to player {e.Player.PlayerId}. Action rejected.");
                return;
            }
            if (IsValidPosition(e.Position) == false)
            {
                _logger.Error(@$"Position ({e.Position.X}, {e.Position.Y}) is not valid.
                Action rejected.");
                return;
            }

            bool attacked = false;
            for (int i = 0; i < PlayerNum; i++)
            {
                if (Players[i].PlayerId != e.Player.PlayerId && Players[i].IsAlive == true
                    && IsSamePosition(e.Position,
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
                if (GameMap.GetChunkAt(e.Position).CanRemoveBlock == false)
                {
                    _logger.Error("Target chunk is empty. Action rejected.");
                    return;
                }

                try
                {
                    GameMap.GetChunkAt(e.Position).RemoveBlock();
                    _playerLastAttackTickList[e.Player.PlayerId] = ElapsedTicks;

                    if (GameMap.GetChunkAt(e.Position).IsVoid == true)
                    {
                        for (int i = 0; i < PlayerNum; i++)
                        {
                            if (Players[i].HasBed == true && IsSamePosition(
                                Players[i].SpawnPoint, e.Position) == true)
                            {
                                Players[i].DestroyBed();
                            }
                        }
                    }
                }
                catch (Exception exception)
                {
                    _logger.Error($"Action failed: {exception}");
                    return;
                }
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
        if (CurrentStage != IGame.Stage.Running && CurrentStage != IGame.Stage.Battling)
        {
            return;
        }

        lock (this)
        {
            if (e.Player.IsAlive == false)
            {
                _logger.Error($"Player {e.Player.PlayerId} is dead. Action rejected.");
                return;
            }
            if (e.Player.WoolCount <= 0)
            {
                _logger.Error($"Player {e.Player.PlayerId} doesn't have enough wool. Action rejected.");
            }
            if (IsAdjacent(ToIntPosition(e.Player.PlayerPosition), e.Position) == false)
            {
                _logger.Error(@$"Position ({e.Position.X}, {e.Position.Y})
                is not adjecant to player {e.Player.PlayerId}. Action rejected.");
                return;
            }
            if (IsValidPosition(e.Position) == false)
            {
                _logger.Error(@$"Position ({e.Position.X}, {e.Position.Y}) is not valid.
                Action rejected.");
                return;
            }

            try
            {
                if (GameMap.GetChunkAt(e.Position).CanPlaceBlock == true)
                {
                    GameMap.GetChunkAt(e.Position).PlaceBlock();
                    Players[e.Player.PlayerId].DecreaseWoolCount();
                }
                else
                {
                    _logger.Error(@$"The chunk at position ({e.Position.X}, {e.Position.Y})
                    has already reached its maximum height. Action rejected.");
                }
            }
            catch (Exception exception)
            {
                _logger.Error($"Action failed: {exception}");
                return;
            }

        }
    }

    /// <summary>
    /// Handle PlayerDieEvent
    /// </summary>
    /// <param name="sender">Sender of the event</param>
    /// <param name="e">Event args</param>
    void HandlePlayerDieEvent(object? sender, PlayerDieEventArgs e)
    {
        if (CurrentStage != IGame.Stage.Running && CurrentStage != IGame.Stage.Battling)
        {
            return;
        }

        lock (this)
        {
            if (_playerDeathTickList[e.Player.PlayerId] is not null)
            {
                _logger.Error($"Player {e.Player.PlayerId} is already dead. Action rejected.");
                return;
            }

            _playerDeathTickList[e.Player.PlayerId] = ElapsedTicks;

        }
    }

    void HandlePlayerTradeEvent(object? sender, PlayerTradeEventArgs e)
    {
        bool succeed = false;

        if (Players[e.Player.PlayerId].IsAlive == false)
        {
            _logger.Error($"Failed to trade: player {e.Player.PlayerId} is dead.");
        }
        else if (IsSamePosition(
            Players[e.Player.PlayerId].SpawnPoint, ToIntPosition(Players[e.Player.PlayerId].PlayerPosition)
            ) == false)
        {
            _logger.Error($"Failed to trade: Player {e.Player.PlayerId} is not at home.");
        }
        else if (CurrentStage != IGame.Stage.Running)
        {
            _logger.Error($"Failed to trade: Player can only trade at stage {IGame.Stage.Running}");
        }
        else if (e.CommodityKind == IPlayer.CommodityKindType.Wool
            &&Players[e.Player.PlayerId].WoolCount >= MaximumItemCount)
        {
            _logger.Error($"Failed to trade: Player {e.Player.PlayerId} cannot hold more wools.");
        }

        else
        {
            int price = e.CommodityKind switch
            {
                IPlayer.CommodityKindType.AgilityBoost => 32,
                IPlayer.CommodityKindType.HealthBoost => 32,
                IPlayer.CommodityKindType.StrengthBoost => 64,
                IPlayer.CommodityKindType.Wool => 2,
                IPlayer.CommodityKindType.HealthPotion => 4,
                _ => throw new ArgumentOutOfRangeException(
                    $"No commodity {e.CommodityKind}."
                )
            };

            if (Players[e.Player.PlayerId].EmeraldCount < price)
            {
                _logger.Error(
                    $"Failed to trade: Player {e.Player.PlayerId} doesn't have enough emerald."
                );
            }
            else
            {
                Players[e.Player.PlayerId].EmeraldAdd(-price);
                succeed = true;
            }
        }

        PlayerTradeEventCallback?.Invoke(
            this, new PlayerTradeEventCallbackArgs(Players[e.Player.PlayerId], e.CommodityKind, succeed)
        );
    }
}
