using System.Collections.Generic;
using System.Text.Json;
using EdcHost.ViewerServers.Messages;
using Xunit;

namespace EdcHost.Tests.UnitTests.ViewerServers.Messages
{
    public class HostConfigurationFromServerTests
    {
        [Fact]
        public void Constructor_SetsProperties()
        {
            // Arrange
            const string expectedMessageType = "HOST_CONFIGURATION_FROM_SERVER";
            List<int> expectedAvailableCameras = new List<int>();
            List<object> expectedAvailableSerialPorts = new List<object>();
            const string expectedMessage = "device info";

            // Act
            HostConfigurationFromServer configuration = new HostConfigurationFromServer(
                expectedAvailableCameras,
                expectedAvailableSerialPorts,
                expectedMessage
            );

            // Assert
            Assert.Equal(expectedMessageType, configuration.MessageType);
            Assert.Same(expectedAvailableCameras, configuration.AvailableCameras);
            Assert.Same(expectedAvailableSerialPorts, configuration.AvailableSerialPorts);
            Assert.Equal(expectedMessage, configuration.Message);
        }

        [Fact]
        public void SerializeToUtf8Bytes_Roundtrip()
        {
            // Arrange
            HostConfigurationFromServer originalConfiguration = new HostConfigurationFromServer(new List<int>(), new List<object>());
            string expectedJson = JsonSerializer.Serialize(originalConfiguration);

            // Act
            byte[] bytes = originalConfiguration.SerializeToUtf8Bytes();
            HostConfigurationFromServer? deserializedConfiguration = JsonSerializer.Deserialize<HostConfigurationFromServer>(bytes);
            Assert.NotNull(deserializedConfiguration);
            string deserializedJson = deserializedConfiguration.SerializeToString();

            // Assert
            Assert.Equal(expectedJson, deserializedJson);
        }

        [Fact]
        public void SerializeToString_Roundtrip()
        {
            // Arrange
            HostConfigurationFromServer originalConfiguration = new HostConfigurationFromServer(new List<int>(), new List<object>());
            string expectedJson = JsonSerializer.Serialize(originalConfiguration);

            // Act
            string jsonString = originalConfiguration.SerializeToString();
            HostConfigurationFromServer? deserializedConfiguration = JsonSerializer.Deserialize<HostConfigurationFromServer>(jsonString);
            Assert.NotNull(deserializedConfiguration);
            string deserializedJson = deserializedConfiguration.SerializeToString();

            // Assert
            Assert.Equal(expectedJson, deserializedJson);
        }
    }
}
