using System.Text.Json;
using EdcHost.ViewerServers.Messages;
using Xunit;

namespace EdcHost.Tests.UnitTests.ViewerServers.Messages;

public class CompetitionControlCommandTests
{
    const string MessageTypeTest = "testType";
    const string TokenTest = "testToken";
    const string CommandTest = "testMessage";
    [Fact]
    public void CompetitionControlCommand_DoNothing_ReturnsContructorValue()
    {
        ICompetitionControlCommand CCcommand = new CompetitionControlCommand(MessageTypeTest, TokenTest, CommandTest);
        Assert.Equal(MessageTypeTest, CCcommand.MessageType);
        Assert.Equal(TokenTest, CCcommand.Token);
        Assert.Equal(CommandTest, CCcommand.Command);
    }
    [Fact]
    public void SerializeToUtf8Bytes_CheckEqual()
    {
        ICompetitionControlCommand CCcommand = new CompetitionControlCommand(MessageTypeTest, TokenTest, CommandTest);
        byte[] TmpRes = CCcommand.SerializeToUtf8Bytes();
        using (MemoryStream ms = new MemoryStream(TmpRes))
        {
            object? object_tmp = JsonSerializer.Deserialize(ms, typeof(CompetitionControlCommand));
            Assert.NotNull(object_tmp);
            ICompetitionControlCommand deserializedObject = (CompetitionControlCommand)object_tmp;
            Assert.Equivalent(CCcommand, deserializedObject);
        }
    }
    [Fact]
    public void SerializeToString_CheckEqual()
    {
        ICompetitionControlCommand CCcommand = new CompetitionControlCommand(MessageTypeTest, TokenTest, CommandTest);
        string TmpRes = CCcommand.SerializeToString();
        object? object_tmp = JsonSerializer.Deserialize(TmpRes, typeof(CompetitionControlCommand));
        Assert.NotNull(object_tmp);
        ICompetitionControlCommand deserializedObject = (CompetitionControlCommand)object_tmp;
        Assert.Equivalent(CCcommand, deserializedObject);
    }
}
