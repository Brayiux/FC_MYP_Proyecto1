using Controller.Data;
using Controller.Definitions.Interfaces;
using Model.Definitions.Delegates;

namespace Controller.Resources
{
    public class ClientConnectionHandler
    {
        #region Eventos
        public event MessageSentEventHandler MessageSent;
        #endregion

        #region Campos
        private readonly ClientConnection _client;

        private readonly IEncoder<string> _encoder;
        #endregion

        #region Construcción
        public ClientConnectionHandler(ClientConnection client, IEncoder<string> encoder)
        {
            ArgumentNullException.ThrowIfNull(client);
            ArgumentNullException.ThrowIfNull(encoder);
            _client = client;
            _encoder = encoder;
            client.Disconnected += Client_Disconnected;
        }

        #endregion

        #region Apoyo

        private void Client_Disconnected()
        {
            string? username = _client.User?.Username;

            if (username != null)
            {
                ChatData.Instance.RemoveUser(username);

                foreach (var room in _client.Rooms)
                {
                    room.Remove(username);
                }
            }
            ChatData.Instance.RemoveClient(_client.Id);
        }

        #endregion

    }
}
