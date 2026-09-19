using Controller.Definitions.Interfaces;
using Controller.Resources;
using Model.Definitions.Delegates;

namespace Controller.Definitions.Abstracts
{
    public abstract class ARStrategyBase : IARStrategy
    {
        #region Eventos

        public event MessageSentEventHandler? MessageSent;
        #endregion

        private readonly EncoderUTF8 _encoder = new();
        

        public abstract Task ExecuteAsync(ClientConnection c);

        protected async Task SendMessageAsync(ClientConnection c, string msg)
        {
            await c.Socket.GetStream().WriteAsync(_encoder.Encode(msg));
            
            MessageSent?.Invoke(msg);
        }
        protected async Task ReplyInvalidAsync(ClientConnection c, MsgBuilder mb)
        {
            string response = BuildInvalidResponse(mb);
            await SendMessageAsync(c, response);
        }

        protected string BuildInvalidResponse(MsgBuilder mb)
        {
            return mb
                    .WithType("RESPONSE")
                    .WithOperation("INVALID")
                    .WithResult("INVALID")
                    .Build();
        }
        protected void DisconnectClient(ClientConnection c)
        {
            c.IsConnected = false;
            c.Socket.GetStream().Close();
            c.Socket.Close();
        }
    }
}
