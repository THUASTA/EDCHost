using System.Drawing;
using EdcHost.Games;
using Moq;
using Xunit;

namespace EdcHost.Tests.UnitTests.Games;
public class MapTests
{
    public class MockPosition : IPosition<int>
    {
        public int X { get; set; }
        public int Y { get; set; }
    }

    public static IPosition<int> point1 = new MockPosition{ X = 1, Y = 1 };
    public static IPosition<int> point2 = new MockPosition{ X = 2, Y = 2 };
    public static IPosition<int> point3 = new MockPosition{ X = 3, Y = 3 };
    public static IPosition<int> point4 = new MockPosition{ X = 4, Y = 4 };

    public IPosition<int>[] spawnPoints = new IPosition<int>[] {point1, point2, point3, point4};

    [Fact]
    public void Map_CorrectyInitialized()
    {
        Map map = new Map(spawnPoints);
        Assert.NotNull(map.Chunks);
        Assert.Equal(64, map.Chunks.Count);
        for (int i = 0; i < 64; i++)
        {
            Assert.Equal(0, map.Chunks[i].Height);
            Assert.Equal(i / 8, map.Chunks[i].Position.X);
            Assert.Equal(i % 8, map.Chunks[i].Position.Y);
        }
        Assert.Equal(1, map.Chunks[9].Height);
        Assert.Equal(1, map.Chunks[18].Height);
        Assert.Equal(1, map.Chunks[27].Height);
        Assert.Equal(1, map.Chunks[36].Height);
    }

    [Fact]
    public void GetChunkAt_DoNothing_ReturnsCorrectChunkPosition()
    {
        Map map = new Map(spawnPoints);
        MockPosition positionMock = new MockPosition { X = 2, Y = 3 };
        IChunk expectedChunk = map.Chunks[19];
        IChunk actualChunk = map.GetChunkAt(positionMock);
        Assert.Equal(expectedChunk, actualChunk);
    }

    [Theory]
    [InlineData(-1, 3)]
    [InlineData(2, -1)]
    [InlineData(8, 3)]
    [InlineData(2, 8)]
    public void GetChunkAt_ThrowsRightException(int x, int y)
    {
        Map map = new Map(spawnPoints);
        var positionMock = new Mock<IPosition<int>>();
        positionMock.Setup(p => p.X).Returns(x);
        positionMock.Setup(p => p.Y).Returns(y);
        Assert.Throws<ArgumentException>(() => map.GetChunkAt(positionMock.Object));
    }
}
