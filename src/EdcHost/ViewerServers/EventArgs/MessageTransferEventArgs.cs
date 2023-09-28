using EdcHost.ViewerServers.Messages;

namespace EdcHost.ViewerServers.EventArgs;

public class MessageTransferEventArgs : System.EventArgs
{
    public IMessage Message { get; }
    public MessageTransferEventArgs(IMessage message)
    {
        Message = message;
    }
}
