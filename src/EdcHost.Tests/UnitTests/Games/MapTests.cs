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

    [Fact]
    public void Map_CorrectyInitialized()
    {
        Map map = new Map();
        Assert.NotNull(map.Chunks);
        Assert.Equal(64, map.Chunks.Count);
        for (int i = 0; i < 64; i++)
        {
            Assert.Equal(0, map.Chunks[i].Height);
            Assert.Equal(i / 8, map.Chunks[i].Position.X);
            Assert.Equal(i % 8, map.Chunks[i].Position.Y);
        }
    }

    [Fact]
    public void GetChunkAt_DoNothing_ReturnsCorrectChunkPosition()
    {
        Map map = new Map();
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
        Map map = new Map();
        var positionMock = new Mock<IPosition<int>>();
        positionMock.Setup(p => p.X).Returns(x);
        positionMock.Setup(p => p.Y).Returns(y);
        Assert.Throws<ArgumentException>(() => map.GetChunkAt(positionMock.Object));
    }
}
