using EdcHost.Games;
using Xunit;

namespace EdcHost.Tests.UnitTests.Games;
public class GameTests
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

    [Theory]
    [InlineData(0, 0)]
    [InlineData(1, 1)]
    [InlineData(2, 2)]
    [InlineData(3, 3)]
    [InlineData(7, 7)]
    [InlineData(9, 9)]
    public void GetChunkAt_DoNothing_ReturnsCorrectChunkPosition(int x, int y)
    {
        MockPosition expectedPosition = new MockPosition { X = x, Y = y };
        Map map = new Map();
        IChunk chunk = map.GetChunkAt(expectedPosition);
        Assert.NotNull(chunk);
        Assert.Equal(expectedPosition, chunk.Position);
    }
}