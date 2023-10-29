using System.Security.Cryptography;
using EdcHost.Games;
using Xunit;

namespace EdcHost.Tests.IntegrationTests;

public partial class GameTests
{
    const int MaxHealth = 8;
    const int OreAccumulationInterval = 200;
    const int HealthBoostPrice = 32;
    const int AgilityBoostPrice = 32;

    [Fact]
    public void HealthAddTests()
    {
        List<Tuple<int, int>> diamond = new List<Tuple<int, int>>
        {
            Tuple.Create(1, 0),
            Tuple.Create(0, 1)
        };
        var game = IGame.Create(null,null,diamond);
        game.Start();

        // Act1 Accumulate diamond, EmeraldCount = 64
        game.Players[0].Place(1f, 0f);
        game.Tick();
        game.Players[0].Place(0f, 1f);
        game.Tick();
        for (int i = 0; i < 4 * OreAccumulationInterval; i++)
        {
            game.Tick();
        }
        game.Players[0].Move(1.4f, 0.4f);
        game.Tick();
        Assert.StrictEqual(MaximumItemCount, game.Players[0].EmeraldCount);

        // Act2 Trade for health, EmeraldCount = 4
        for(int i = 1; i < 8; i++)
        {
            game.Players[0].Trade(IPlayer.CommodityKindType.HealthPotion);
            game.Tick();
            Assert.StrictEqual(i + 1, game.Players[0].Health);
            Assert.StrictEqual(MaximumItemCount - 4 * i, game.Players[0].EmeraldCount);
        }
        Assert.False(game.Players[0].Trade(IPlayer.CommodityKindType.HealthPotion));
        game.Tick();
        Assert.True(game.Players[0].Trade(IPlayer.CommodityKindType.HealthBoost));
        game.Tick();
        Assert.StrictEqual(MaximumItemCount - 4 * 7 - HealthBoostPrice, game.Players[0].EmeraldCount);
        Assert.StrictEqual(MaxHealth + 3, game.Players[0].MaxHealth);
        Assert.StrictEqual(MaxHealth + 3, game.Players[0].Health);

        //Act3 Accumulate diamond, EmeraldCount = 64
        game.Players[0].Move(0.4f, 0.4f);
        game.Tick();
        game.Players[0].Move(0.4f, 1.4f);
        Assert.StrictEqual(MaximumItemCount, game.Players[0].EmeraldCount);

        //Act4 Trade for Agility
        game.Players[0].Trade(IPlayer.CommodityKindType.AgilityBoost);
        Assert.StrictEqual(1, game.Players[0].ActionPoints);
        Assert.StrictEqual(MaximumItemCount - AgilityBoostPrice, game.Players[0].EmeraldCount);

        //Act5 Trade for Strength
        Assert.False(game.Players[0].Trade(IPlayer.CommodityKindType.StrengthBoost));
        for (int i = 0; i < 4 * OreAccumulationInterval; i++)
        {
            game.Tick();
        }
        Assert.StrictEqual(MaximumItemCount, game.Players[0].EmeraldCount);
        Assert.True(game.Players[0].Trade(IPlayer.CommodityKindType.StrengthBoost));
        Assert.StrictEqual(2, game.Players[0].Strength);
        Assert.StrictEqual(0, game.Players[0].EmeraldCount);

        game.End();

    }
}
