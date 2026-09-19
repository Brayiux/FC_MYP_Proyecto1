using Controller.Data;
using Controller.Definitions.Abstracts;
using Controller.Resources;
using Model.Entities;
using System.Threading.Tasks;

namespace Controller.Strategies
{
    public class DisconnectSt : ARStrategyBase
    {
        public DisconnectSt()
        {
            
        }
        public async override Task ExecuteAsync(ClientConnection c)
        {

            if (!ChatData.Instance.ExistsUser(c.User.Username))
            {
                await ReplyInvalidAsync(c);
            }
            else
            {
                var task1 = NotifyDisconnectedToChatAsync(c);

                var task2 = HandleDisconnectedToRoomsAsync(c);

                await Task.WhenAll([task1, task2]);
                
                c.ClearRooms();
                c.ClearPrivateChats();

                ChatData.Instance.RemoveUser(c.User.Username);
            }

            ChatData.Instance.RemoveClient(c.Id);

            DisconnectClient(c);
        }

        #region Apoyo

        /// <summary>
        /// Notifica a todos los usuarios del chat que <paramref name="client"/> se ha
        /// desconectado.
        /// </summary>
        /// <param name="client">Usuario que se desconecta</param>
        /// <returns></returns>
        private async Task NotifyDisconnectedToChatAsync(ClientConnection client)
        {
            MsgBuilder mb = new();
            string notification = mb.WithType("DISCONNECTED")
                                    .WithUsername(client.User.Username)
                                    .Build();
            List<Task> tasks = [];
            foreach (var (username, user) in ChatData.Instance.GetAllUsers())
            {
                if (user.Equals(client))
                    continue;
                tasks.Add(SendMessageAsync(user, notification));
            }
            await Task.WhenAll(tasks);
        }

        /// <summary>
        /// Notifica a todos los usuarios que comparten sala con <paramref name="client"/>
        /// que este se ha desconectado y lo saca del registro de todas las salas.
        /// </summary>
        /// <param name="client">Cliente que se está desconectando.</param>
        private async Task HandleDisconnectedToRoomsAsync(ClientConnection client)
        {
            MsgBuilder mb = new();
            List<Task> tasks = [];
            foreach (ChatRoom r in client.Rooms)
            {
                foreach (var (username, _) in r.Members)
                {
                    mb.Reset();
                    ClientConnection user = ChatData.Instance.GetUserOrNull(username)!;
                    string notification = mb.WithType("LEFT_ROOM")
                                            .WithRoomname(r.Roomname)
                                            .WithUsername(client.User.Username)
                                            .Build();

                    tasks.Add(SendMessageAsync(user, notification));
                }
                r.Remove(client.User.Username);
            }
            await Task.WhenAll(tasks);
        }

        #endregion

    }
}
