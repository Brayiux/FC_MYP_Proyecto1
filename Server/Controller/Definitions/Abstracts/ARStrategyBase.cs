using Controller.Data;
using Controller.Definitions.Interfaces;
using Controller.Resources;
using Model.Definitions.Delegates;
using System.Threading.Tasks;

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
        protected async Task ReplyInvalidAsync(ClientConnection c)
        {
            MsgBuilder mb = new();

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

        protected async Task<bool> HandleClientIdentificationValidationAsync(ClientConnection c)
        {
            bool isIdentified = ChatData.Instance.ExistsUser(c.User.Username);

            if (isIdentified) return true;
           
            MsgBuilder mb = new();
            string response = mb.WithType("RESPONSE")
                                .WithOperation("INVALID")
                                .WithResult("NOT_IDENTIFIED")
                                .Build();

            await SendMessageAsync(c, response);
            c.Disconnect();

            return false;
        }
    }
}
