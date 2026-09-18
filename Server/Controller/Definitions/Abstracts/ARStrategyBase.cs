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

        protected async void SendMessage(ClientConnection c, string msg)
        {
            await c.Socket.GetStream().WriteAsync(_encoder.Encode(msg));
            
            MessageSent?.Invoke(msg);
        }
    }
}
