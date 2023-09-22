using EdcHost.Games;
using Moq;
using Xunit;

namespace EdcHost.Tests.UnitTests.Games;

public interface IDateTime
{
    DateTime GetCurrentDateTime();
}

public class MineTest
{
    public class MockPosition : IPosition<float>
    {
        public float X { get; set; }
        public float Y { get; set; }
    }

    [Fact]
    public void IntAccumulatedOreCount_NotGenerate_ReturnsCorrectValue()
    {
        Mine mine = new Mine(IMine.OreKindType.Diamond, new MockPosition { X = 0f, Y = 0f });
        Assert.Equal(0, mine.AccumulatedOreCount);
    }

    [Theory]
    [InlineData(IMine.OreKindType.IronIngot, IMine.OreKindType.IronIngot)]
    [InlineData(IMine.OreKindType.GoldIngot, IMine.OreKindType.GoldIngot)]
    [InlineData(IMine.OreKindType.Diamond, IMine.OreKindType.Diamond)]
    public void OreKind_ReturnsCorrectValue(IMine.OreKindType oreKindType, IMine.OreKindType expectedOreKind)
    {
        Mine mine = new Mine(oreKindType, new MockPosition { X = 0f, Y = 0f });
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
        IPosition<float> expected = new MockPosition { X = x, Y = y };
        Mine mine = new Mine(IMine.OreKindType.Diamond, expected);
        IPosition<float> actual = mine.Position;
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void LastOreGeneratedTime_DoNothing_ReturnsCorrectTime()
    {
            IMine.OreKindType oreKind = IMine.OreKindType.IronIngot;
            var position = new MockPosition { X = 0f, Y = 0f };
            var mockMine = new Mock<Mine>(oreKind, position) { CallBase = true };
            var currentTime = new DateTime(2023, 9, 22, 0, 0, 0);
            mockMine.SetupGet(m => m.LastOreGeneratedTime).Returns(currentTime);
            mockMine.Object.GenerateOre();
            Assert.Equal(1, mockMine.Object.AccumulatedOreCount);
            Assert.Equal(currentTime, mockMine.Object.LastOreGeneratedTime);
    }

    [Theory]
    [InlineData(3, 3)]
    [InlineData(4, 4)]
    [InlineData(30, 30)]
    [InlineData(100, 100)]
    [InlineData(200, 200)]
    public void GenerateOre_AccumulatedOreCountAdd_ReturnsCorrectValue(int generate, int expectedValue)
    {
        Mine mine = new Mine(IMine.OreKindType.Diamond, new MockPosition { X = 0f, Y = 0f });
        for (int i = 0; i < generate; i++)
        {
            mine.GenerateOre();
        }
        TimeSpan timeDifference = DateTime.Now - mine.LastOreGeneratedTime;
        Assert.Equal(expectedValue, mine.AccumulatedOreCount);
        Assert.True(timeDifference.TotalSeconds < 0.01);
    }

    [Fact]
    public void PickUpOre_CountLessThanAccumulatedOreCount_ReturnsCorrctValue()
    {
        Mine mine = new Mine(IMine.OreKindType.Diamond, new MockPosition { X = 0f, Y = 0f });
        for (int i = 0; i < 200; i++)
        {
            mine.GenerateOre();
        }
        int count = 64;
        int expectedValue = 136;
        mine.PickUpOre(count);
        Assert.Equal(expectedValue, mine.AccumulatedOreCount);
    }
    
    [Fact]
    public void PickUpOre_CountMoreThanAccumulatedOreCount_ReturnsCorrctValue()
    {
        Mine mine = new Mine(IMine.OreKindType.Diamond, new MockPosition { X = 0f, Y = 0f });
        for (int i = 0; i < 30; i++)
        {
            mine.GenerateOre();
        }
        int count = 60;
        InvalidOperationException ex = Assert.Throws<InvalidOperationException>(()=>{mine.PickUpOre(count);});
        Assert.Equal("No enough ore.", ex.Message);
    }

}
/*
using Xunit;
using Moq;
using System;

namespace EdcHost.Games.Tests
{
    public class MineTests
    {
        [Fact]
        public void GenerateOre_ShouldIncreaseAccumulatedOreCountAndSetLastOreGeneratedTime()
        {
            // Arrange
            var oreKind = IMine.OreKindType.IronIngot;
            var position = new Position<float>(0f, 0f);

            // Create a mock of IMine with time mocking
            var mockMine = new Mock<Mine>(oreKind, position) { CallBase = true };
            var currentTime = new DateTime(2023, 9, 22, 10, 0, 0);
            mockMine.SetupGet(m => m.LastOreGeneratedTime).Returns(currentTime);

            // Act
            mockMine.Object.GenerateOre();

            // Assert
            Assert.Equal(1, mockMine.Object.AccumulatedOreCount);
            Assert.Equal(currentTime, mockMine.Object.LastOreGeneratedTime);
        }

        [Fact]
        public void PickUpOre_ShouldDecreaseAccumulatedOreCount()
        {
            // Arrange
            var oreKind = IMine.OreKindType.GoldIngot;
            var position = new Position<float>(1f, 1f);
            var initialOreCount = 5;
            var pickUpCount = 3;

            // Create a mock of IMine with time mocking
            var mockMine = new Mock<Mine>(oreKind, position) { CallBase = true };
            mockMine.Object.AccumulatedOreCount = initialOreCount;

            // Act
            mockMine.Object.PickUpOre(pickUpCount);

            // Assert
            Assert.Equal(initialOreCount - pickUpCount, mockMine.Object.AccumulatedOreCount);
        }
    }
}*/