using Controller.Data;
using Controller.Definitions.Abstracts;
using Controller.Resources;
using Model.Entities;

namespace Controller.Strategies
{
    public class RoomMsgSt : FindRoomARStrategyBase
    {
        private readonly string _msg;

        public RoomMsgSt(string roomname, string msg) : base(roomname, "ROOM_TEXT")
        {
            _msg = msg;
        }
        public async override Task ExecuteAsync(ClientConnection c)
        {
            if (!await HandleClientIdentificationValidationAsync(c))
                return;

            ChatRoom? room = ChatData.Instance.GetRoomOrNull(_roomname);
            
            // Si el la sala no existe:

            if (room == null)
            {
                await ReplyInvalidAsync(c);
                return;
            }

            // Si existe, pero no es miembro:

            if (!room.IsMember(c.User.Username))
            {
                await ReplyNotJoinedAsync(c);
                return;
            }

            // Si existe y es miembro:

            await NotifyRoomTextFromAsync(c, room);
        }

        #region Apoyo


        /// <summary>
        /// Responde al usuario que no está unido a la sala y por tanto no enviar
        /// un mensaje en ella.
        /// </summary>
        /// <param name="client">Usuario al que se le responde.</param>
        /// <returns></returns>
        private async Task ReplyNotJoinedAsync(ClientConnection client)
        {
            MsgBuilder mb = new();
            string response = mb.WithType("RESPONSE")
                                .WithOperation("ROOM_TEXT")
                                .WithResult("NOT_JOINED")
                                .WithExtra(_roomname)
                                .Build();
            await SendMessageAsync(client, response);
        }

        /// <summary>
        /// Notifica a todos los miembros de una sala menos a <paramref name="client"/>
        /// este ha enviado un mensaje.
        /// </summary>
        /// <param name="client">Usuario que ha salido de la sala.</param>
        /// <param name="room">Sala de donde se sale.</param>
        /// <returns></returns>
        private async Task NotifyRoomTextFromAsync(ClientConnection client, ChatRoom room)
        {
            MsgBuilder mb = new();
            string response = mb.WithType("ROOM_TEXT_FROM")
                                .WithRoomname(_roomname)
                                .WithUsername(client.User.Username)
                                .Build();
            List<Task> task = [];
            foreach (var (username, user) in room.Members)
            {
                ClientConnection userC = ChatData.Instance.GetUserOrNull(username)!;
                if (username.Equals(client.User.Username))
                    continue;
                task.Add(SendMessageAsync(userC, response));
            }
            await Task.WhenAll(task);

        }

        #endregion

    }
}
