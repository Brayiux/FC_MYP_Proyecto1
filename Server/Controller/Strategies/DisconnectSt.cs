using Controller.Data;
using Controller.Definitions.Abstracts;
using Controller.Definitions.Interfaces;
using Controller.Resources;
using Model.Entities;

namespace Controller.Strategies
{
    public class DisconnectSt : ARStrategyBase
    {
        public DisconnectSt()
        {
            
        }
        public async override Task ExecuteAsync(ClientConnection c)
        {
            MsgBuilder mb = new();

            if (!ChatData.Instance.ExistsUser(c.User.Username))
            {
                await ReplyInvalidAsync(c, mb);
            }
            else
            {
                NotifyDisconnectedToAllUsers(c, mb);

                NotifyDisconnectedToAllUserRooms(c, mb);

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
        /// <param name="mb">Constructor del mensaje de notificación.</param>
        private void NotifyDisconnectedToAllUsers(ClientConnection client, MsgBuilder mb)
        {
            mb.Reset();
            foreach (var (username, user) in ChatData.Instance.GetAllUsers())
            {
                if (!user.Equals(client))
                {
                    string notification = mb.WithType("DISCONNECTED")
                                            .WithUsername(username)
                                            .Build();
                    _ = SendMessageAsync(user, notification);
                }
            }
        }

        /// <summary>
        /// Notifica a todos los usuarios que comparten sala con <paramref name="client"/>
        /// que este se ha desconectado.
        /// </summary>
        /// <param name="client">Cliente que se está desconectando.</param>
        /// <param name="mb">Constructor del mensaje de notificación.</param>
        private void NotifyDisconnectedToAllUserRooms(ClientConnection client, MsgBuilder mb)
        {
            mb.Reset();
            foreach (ChatRoom r in client.Rooms)
            {
                foreach (var (username, _) in r.Members)
                {
                    ClientConnection user = ChatData.Instance.GetUserOrNull(username)!;
                    string notification = mb.WithType("LEFT_ROOM")
                                            .WithRoomname(r.Roomname)
                                            .WithUsername(client.User.Username)
                                            .Build();

                    _ = SendMessageAsync(user, notification);
                }
            }
        }

        #endregion

    }
}
