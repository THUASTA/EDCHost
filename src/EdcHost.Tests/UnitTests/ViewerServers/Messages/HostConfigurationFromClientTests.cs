using System.Text.Json;
using EdcHost.ViewerServers.Messages;
using Xunit;

namespace EdcHost.Tests.UnitTests.ViewerServers.Messages;

public class HostConfigurationFromClientTests
{
    const string MessageTypeTest = "testType";
    const string TokenTest = "testToken";

    [Fact]
    public void Constructor_SetsProperties()
    {
        var config = new HostConfigurationFromClient(MessageTypeTest, TokenTest, new List<object>());
        Assert.Equal(MessageTypeTest, config.MessageType);
        Assert.Equal(TokenTest, config.Token);
    }
    [Fact]
    public void SerializeToUtf8Bytes_Roundtrip()
    {
        // Arrange
        IHostConfigurationFromClient originalConfig = new HostConfigurationFromClient(MessageTypeTest, TokenTest, new List<object>());

        // Act
        byte[] bytes = originalConfig.SerializeToUtf8Bytes();
        IHostConfigurationFromClient? deserializedConfig = JsonSerializer.Deserialize<HostConfigurationFromClient>(bytes);

        // Assert
        Assert.NotNull(deserializedConfig);
        Assert.Equivalent(originalConfig, deserializedConfig);
    }
    [Fact]
    public void SerializeToString_Roundtrip()
    {
        IHostConfigurationFromClient originalConfig = new HostConfigurationFromClient(MessageTypeTest, TokenTest, new List<object>()); // Set Players to null for simplicity.
        string jsonString = originalConfig.SerializeToString();
        IHostConfigurationFromClient? deserializedConfig = JsonSerializer.Deserialize<HostConfigurationFromClient>(jsonString);
        Assert.NotNull(deserializedConfig);
        Assert.Equivalent(originalConfig, deserializedConfig);
    }
}
