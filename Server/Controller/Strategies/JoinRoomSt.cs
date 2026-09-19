using Controller.Data;
using Controller.Definitions.Abstracts;
using Controller.Resources;
using Model.Entities;

namespace Controller.Strategies
{
    public class JoinRoomSt : FindRoomARStrategyBase
    {
        public JoinRoomSt(string roomname) : base(roomname, "JOIN_ROOM")
        {
            
        }
        public async override Task ExecuteAsync(ClientConnection c)
        {
            if (!await HandleClientIdentificationValidationAsync(c))
                return;

            ChatRoom? room = ChatData.Instance.GetRoomOrNull(_roomname);

            // Si la sala no existe
            if (room == null)
            {
                await ReplyNoSuchRoomAsync(c);
                return;
            }

            // Si existe pero no fue invitado
            if (!room.IsInvited(c.User.Username))
            {
                await ReplyNotInvitedAsync(c);
                return;
            }

            // Si la sala existe y el usuario fue invitado:
            room.Add(c.User.Username);
            c.AddRoom(room);
            await ReplyJoinRoomSuccessfully(c);
            await NotifyAllRoomUsersUserJoined(c, room);

            
        }

        #region Apoyo

        /// <summary>
        /// Responde al <paramref name="client"/> que no se puede unir a la sala
        /// porque no ha sido invitado.
        /// </summary>
        /// <param name="client">Usuario al que se le responde.</param>
        /// <returns></returns>
        private async Task ReplyNotInvitedAsync(ClientConnection client)
        {
            MsgBuilder mb = new();
            string response = mb.WithType("RESPONSE")
                                .WithOperation("JOIN_ROOM")
                                .WithResult("NOT_INVITED")
                                .WithExtra(_roomname)
                                .Build();
            await SendMessageAsync(client, response);
        }

        /// <summary>
        /// Responde al <paramref name="client"/> que se ha logrado
        /// unir exitosamente a la sala.
        /// </summary>
        /// <param name="client">Usuario al que se le responde.</param>
        /// <returns></returns>
        private async Task ReplyJoinRoomSuccessfully(ClientConnection client)
        {
            MsgBuilder mb = new();
            string response = mb.WithType("RESPONSE")
                                .WithOperation("JOIN_ROOM")
                                .WithResult("SUCCESS")
                                .WithExtra(_roomname)
                                .Build();
            await SendMessageAsync(client, response);
        }

        /// <summary>
        /// Notifica al resto de usuarios diferentes de <paramref name="client"/> que este
        /// ha entrada a la <paramref name="room"/>.
        /// </summary>
        /// <param name="client">Usuario que entra a la sala.</param>
        /// <param name="room">Sala donde el usuario entra.</param>
        /// <returns></returns>
        private async Task NotifyAllRoomUsersUserJoined(ClientConnection client, ChatRoom room)
        {
            MsgBuilder mb = new();
            string notification = mb.WithType("JOINED_ROOM")
                                    .WithRoomname(_roomname)
                                    .WithUsername(client.User.Username)
                                    .Build();
            List<Task> tasks = [];
            foreach(var (username, _) in room.Members)
            {
                if (username.Equals(client.User.Username))
                    continue;
                ClientConnection user = ChatData.Instance.GetUserOrNull(username)!;
                tasks.Add(SendMessageAsync(user, notification));
            }

            await Task.WhenAll(tasks);
        }

        #endregion
    }
}
