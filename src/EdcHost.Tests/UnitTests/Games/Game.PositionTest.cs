using System.Reflection;
using EdcHost.Games;
using Xunit;

namespace EdcHost.Tests.UnitTests.Games;

public class Game_PositionTest
{
    public class MockIntPosition : IPosition<int>
    {
        public int X { get; set; }
        public int Y { get; set; }
    }

    public class MockFloatPosition : IPosition<float>
    {
        public float X { get; set; }
        public float Y { get; set; }
    }

    [Fact]
    public void ToIntPosition_DoNothing_ReturnsCorrectValue()
    {
        Game game = new Game();
        MethodInfo? toIntPositionMethod = typeof(Game).GetMethod("ToIntPosition", BindingFlags.NonPublic | BindingFlags.Instance);
        IPosition<float> fPosition = new MockFloatPosition { X = 2.5f, Y = 2.5f };
        IPosition<int> actualPosition = (IPosition<int>)toIntPositionMethod.Invoke(game, new object[] { fPosition });
        IPosition<int> expPosition = new MockIntPosition { X = 2, Y = 2 };
        Assert.Equal(expPosition.X, actualPosition.X);
        Assert.Equal(expPosition.Y, actualPosition.Y);
    }

    [Theory]
    [InlineData(0, 0)]
    [InlineData(1, 1)]
    [InlineData(2, 2)]
    [InlineData(7, 7)]
    public void IsValidPosition_ValidValue_ReturnsTrue(int x, int y)
    {
        Game game = new Game();
        MethodInfo? isValidPositionMethod = typeof(Game).GetMethod("IsValidPosition", BindingFlags.NonPublic | BindingFlags.Instance);
        IPosition<int> position = new MockIntPosition { X = x, Y = y };
        bool isValid = (bool)isValidPositionMethod.Invoke(game, new object[] { position });
        Assert.True(isValid);
    }

    [Theory]
    [InlineData(8, 8)]
    [InlineData(7, 8)]
    [InlineData(-1, -1)]
    [InlineData(-1, 0)]
    public void IsValidPosition_InvalidValue_ReturnsFalse(int x, int y)
    {
        Game game = new Game();
        MethodInfo? isValidPositionMethod = typeof(Game).GetMethod("IsValidPosition", BindingFlags.NonPublic | BindingFlags.Instance);
        IPosition<int> position = new MockIntPosition { X = x, Y = y };
        bool isValid = (bool)isValidPositionMethod.Invoke(game, new object[] { position });
        Assert.False(isValid);
    }

    [Theory]
    [InlineData(0, 0, 1, 2)]
    [InlineData(0, 0, 2, 0)]
    [InlineData(0, 0, 1, 1)]
    public void IsAdjacent_ReturnsTrue(int x1, int y1, int x2, int y2)
    {
        Game game = new Game();
        MethodInfo? isAdjacentMethod = typeof(Game).GetMethod("IsAdjacentPosition", BindingFlags.NonPublic | BindingFlags.Instance);
        IPosition<int> position1 = new MockIntPosition { X = x1, Y = y1 };
        IPosition<int> position2 = new MockIntPosition { X = x2, Y = y2 };
        bool isAdjacent = (bool)isAdjacentMethod.Invoke(game, new object[] { position1, position2 });
        Assert.True(isAdjacent);
    }

    [Fact]
    public void IsAdjacent_ReturnsFalse()
    {
        Game game = new Game();
        MethodInfo? isAdjacentMethod = typeof(Game).GetMethod("IsAdjacentPosition", BindingFlags.NonPublic | BindingFlags.Instance);
        IPosition<int> position1 = new MockIntPosition { X = 0, Y = 0 };
        IPosition<int> position2 = new MockIntPosition { X = 2, Y = 2 };
        bool isAdjacent = (bool)isAdjacentMethod.Invoke(game, new object[] { position1, position2 });
        Assert.False(isAdjacent);
    }

    [Fact]
    public void IsSamePosition_ReturnsTrue()
    {
        Game game = new Game();
        MethodInfo? isSamePositionMethod = typeof(Game).GetMethod("IsSamePosition", BindingFlags.NonPublic | BindingFlags.Instance);
        IPosition<int> position1 = new MockIntPosition { X = 0, Y = 0 };
        IPosition<int> position2 = new MockIntPosition { X = 0, Y = 0 };
        bool isAdjacent = (bool)isSamePositionMethod.Invoke(game, new object[] { position1, position2 });
        Assert.True(isAdjacent);
    }

    [Fact]
    public void IsSamePosition_ReturnsFalse()
    {
        Game game = new Game();
        MethodInfo? isSamePositionMethod = typeof(Game).GetMethod("IsSamePosition", BindingFlags.NonPublic | BindingFlags.Instance);
        IPosition<int> position1 = new MockIntPosition { X = 0, Y = 0 };
        IPosition<int> position2 = new MockIntPosition { X = 2, Y = 0 };
        bool isAdjacent = (bool)isSamePositionMethod.Invoke(game, new object[] { position1, position2 });
        Assert.False(isAdjacent);
    }

    [Theory]
    [InlineData(0f, 0f, 1f, 1f)]
    [InlineData(0f, 0f, 2.5f, 2.5f)]
    [InlineData(3f, 3f, 7.4f, 7.4f)]
    public void EucilidDistance_ReturnsCorrectValue(int x1, int y1, int x2, int y2)
    {
        Game game = new Game();
        MethodInfo? eucilidDistanceMethod = typeof(Game).GetMethod("EucilidDistance", BindingFlags.NonPublic | BindingFlags.Instance);
        IPosition<float> position1 = new MockFloatPosition { X = x1, Y = y1 };
        IPosition<float> position2 = new MockFloatPosition { X = x2, Y = y2 };
        double expValue = (double)eucilidDistanceMethod.Invoke(game, new object[] { position1, position2 });
        double actualValue = Math.Sqrt((x1 - x2) * (x1 - x2) + (y1 - y2) * (y1 - y2));
        Assert.True(Math.Abs(actualValue - expValue) < 0.00001);
    }
}


