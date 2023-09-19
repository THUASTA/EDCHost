using EdcHost.Games;
using Xunit;

namespace EdcHost.Tests.UnitTests.Games;

public class MineTest
{
    public class MockPosition : IPosition<float>
    {
        public float X { get; set;}
        public float Y { get; set;}
    }

    [Fact]
    public void IntAccumulatedOreCount_NotGenerate_ReturnsCorrectValue()
    {
        Mine mine = new Mine(IMine.OreKindType.Diamond, new MockPosition{X = 0f,Y = 0f});
        Assert.Equal(0,mine.AccumulatedOreCount);
    }

    [Theory]
    [InlineData(IMine.OreKindType.IronIngot, IMine.OreKindType.IronIngot)]
    [InlineData(IMine.OreKindType.GoldIngot, IMine.OreKindType.GoldIngot)]
    [InlineData(IMine.OreKindType.Diamond, IMine.OreKindType.Diamond)]
    public void OreKind_ReturnsCorrectValue(IMine.OreKindType oreKindType, IMine.OreKindType expectedOreKind)
    {
        Mine mine = new Mine(oreKindType, new MockPosition {X = 0f, Y = 0f});
        Assert.Equal(expectedOreKind, mine.OreKind);
    }

    [Theory]
    [InlineData(0f, 0f)]
    [InlineData(2.5f, 2.5f)]
    [InlineData(-2.5f, -2.5f)]
    [InlineData(float.MinValue, float.MinValue)]
    [InlineData(float.MaxValue, float.MaxValue)]
    public void Position_DoNothing_ReturnsConstructorValue(float x, float y)
    {
        IPosition<float> expected = new MockPosition {X = x, Y = y};
        Mine mine = new Mine(IMine.OreKindType.Diamond, expected);
        IPosition<float> actual = mine.Position;
        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData(3, 3)]
    [InlineData (4, 4)]
    [InlineData (30, 30)]
    [InlineData (100, 100)]
    [InlineData (200, 200)]
    public void Generate_AccumulatedOreCountAdd_ReturnsCorrectValue(int generate, int expectedValue)
    {
        Mine mine = new Mine(IMine.OreKindType.Diamond, new MockPosition{X = 0f, Y = 0f});
        for(int i = 0; i < generate;i++)
        {
            mine.Generate();
        }
        Assert.Equal(expectedValue,mine.AccumulatedOreCount);
    }

    [Fact]
    public void PickUpOre_CountLessThanAccumulatedOreCount_ReturnsCorrctValue()
    {
        Mine mine = new Mine(IMine.OreKindType.Diamond ,new MockPosition { X = 0f,Y = 0f });
        for(int i =0; i < 200; i++)
        {
            mine.Generate();
        }
        int count = 64;
        int expectedValue = 136;
        mine.PickUpOre(count);
        Assert.Equal(expectedValue, mine.AccumulatedOreCount);
    }
    [Fact]
    public void PickUpOre_CountMoreThanAccumulatedOreCount_ReturnsCorrctValue()
    {
        Mine mine = new Mine(IMine.OreKindType.Diamond ,new MockPosition { X = 0f,Y = 0f });
        for(int i =0; i < 30; i++)
        {
            mine.Generate();
        }
        int count = 60;
        int expectedValue = 0;
        mine.PickUpOre(count);
        Assert.Equal(expectedValue, mine.AccumulatedOreCount);
    }

}
