using Controller.Data;
using Controller.Definitions.Interfaces;
using Model.Definitions.Delegates;
using Model.Entities;
using System.Threading.Tasks;

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

        private async void Client_Disconnected()
        {
            string? username = _client.User?.Username;

            if (username != null)
            {

                await NotifyDisconnectedToChatAsync();

                await HandleDisconnectedToRoomsAsync();

                ChatData.Instance.RemoveUser(username);
            }
            ChatData.Instance.RemoveClient(_client.Id);
        }

        private async Task SendMessageAsync(ClientConnection client, string msg)
        {
            await client.Socket.GetStream().WriteAsync(_encoder.Encode(msg));
        }

        /// <summary>
        /// Notifica a todos los usuarios que comparten sala con el usuario que
        /// este se ha desconectado y lo saca del registro de todas las salas.
        /// </summary>
        private async Task HandleDisconnectedToRoomsAsync()
        {
            MsgBuilder mb = new();
            List<Task> tasks = [];
            foreach (ChatRoom r in _client.Rooms)
            {
                foreach (var (username, _) in r.Members)
                {
                    if (username.Equals(_client.User.Username))
                        continue;
                    mb.Reset();
                    ClientConnection user = ChatData.Instance.GetUserOrNull(username)!;
                    string notification = mb.WithType("LEFT_ROOM")
                                            .WithRoomname(r.Roomname)
                                            .WithUsername(_client.User.Username)
                                            .Build();

                    tasks.Add(SendMessageAsync(user, notification));
                }
                r.Remove(_client.User.Username);
            }
            await Task.WhenAll(tasks);
        }

        /// <summary>
        /// Notifica a todos los usuarios del chat que el usuario se ha
        /// desconectado.
        /// </summary>
        /// <returns></returns>
        private async Task NotifyDisconnectedToChatAsync()
        {
            MsgBuilder mb = new();
            string notification = mb.WithType("DISCONNECTED")
                                    .WithUsername(_client.User.Username)
                                    .Build();
            List<Task> tasks = [];
            foreach (var (username, user) in ChatData.Instance.GetAllUsers())
            {
                if (user.Equals(_client))
                    continue;
                tasks.Add(SendMessageAsync(user, notification));
            }
            await Task.WhenAll(tasks);
        }

        #endregion

    }
}
