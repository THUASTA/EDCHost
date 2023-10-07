using System.Text.Json;
using EdcHost.ViewerServers.Messages;
using Xunit;

namespace EdcHost.Tests.UnitTests.ViewerServers.Messages;

public class ErrorTests
{
    private const string MessageTypeTest = "testType";
    private const string MessageTest = "testMessage";
    [Theory]
    [InlineData(-1)]
    [InlineData(1)]
    [InlineData(int.MaxValue)]
    [InlineData(int.MinValue)]
    public void ErrorCode_DoNothing_ReturnsConstructorValues(int err_code)
    {
        IError error = new Error(MessageTypeTest, err_code, MessageTest);
        Assert.Equal(MessageTypeTest, error.MessageType);
        Assert.Equal(err_code, error.ErrorCode);
        Assert.Equal(MessageTest, error.Message);
    }
    [Theory]
    [InlineData(-1)]
    [InlineData(1)]
    [InlineData(int.MaxValue)]
    [InlineData(int.MinValue)]
    public void SerializeToUtf8Bytes_CheckEqual(int err_code)
    {
        IError error = new Error(MessageTypeTest, err_code, MessageTest);
        byte[] TmpRes = error.SerializeToUtf8Bytes();
        using (MemoryStream ms = new MemoryStream(TmpRes))
        {
            object? object_tmp = JsonSerializer.Deserialize(ms, typeof(Error));
            Assert.NotNull(object_tmp);
            IError deserializedObject = (Error)object_tmp;
            Assert.Equivalent(error, deserializedObject);
        }
    }
    [Theory]
    [InlineData(-1)]
    [InlineData(1)]
    [InlineData(int.MaxValue)]
    [InlineData(int.MinValue)]
    public void SerializeToString_CheckEqual(int err_code)
    {
        IError error = new Error(MessageTypeTest, err_code, MessageTest);
        string TmpRes = error.SerializeToString();
        object? object_tmp = JsonSerializer.Deserialize(TmpRes, typeof(Error));
        Assert.NotNull(object_tmp);
        IError deserializedObject = (Error)object_tmp;
        Assert.Equivalent(error, deserializedObject);
    }
}
