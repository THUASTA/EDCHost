using System.Text.Json;
using System.Text.Json.Serialization;
using EdcHost.Games;
using EdcHost.ViewerServers.Messages;
using Moq;
using Xunit;
using Xunit.Abstractions;

namespace EdcHost.Tests.UnitTests.ViewerServers.Messages;

public class HostConfigurationFromClientTests
{
    private const string MessageTypeTest = "testType";
    private const string TokenTest = "testToken";

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
