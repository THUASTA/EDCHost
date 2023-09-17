using EdcHost.Games;
using moq;
using Xunit;

namespace EdcHost.Tests.UnitTests.Games;
public class GameTests
{
    public class MockPosition : IPosition
    {
        public int X { get; set;}
        public int Y { get; set;}
    }

    [Fact]
    public void Map_CorrectyInitialized()
    {
        Map map = new Map();
        Assert.NotNull(map.Chunks);
        Assert.Equal(64,map.Chunks.Count);
        for(int i=0;i<64;i++)
        {
            Assert.Equal(0,map.Chunks[i].Height);
            Assert.Equal(i/8,map.Chunks[i].X);
            Assert.Equak(i%8,map.Chunks[i].Y);
        }
    }

    [Theory]
    [InlineData(0, 0)]
    [InlineData(1, 1)]
    [InlineData(2, 2)]
    [InlineData(3, 3)]
    [InlineData(7, 7)]
    
    public class GetChunkAt_ReturnsCorrectChunkPositionX(int x, int expectedX)
    {
        Map map = new Map();
        IChunk chunk= map.GetChunkAt(new MockPosition(x,0));
        Assert.Equal(expectedX,chunk.Position.X);
    }

        [Theory]
    [InlineData(0, 0)]
    [InlineData(1, 1)]
    [InlineData(2, 2)]
    [InlineData(3, 3)]
    [InlineData(7, 7)]
    
    public class GetChunkAt_ReturnsCorrectChunkPositionX(int y, int expectedY)
    {
        Map map = new Map();
        IChunk chunk= map.GetChunkAt(new MockPosition(0,y));
        Assert.Equal(expectedY,chunk.Position.Y);
    }
}