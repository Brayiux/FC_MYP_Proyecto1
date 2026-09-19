using Controller.Data;
using Controller.Definitions.Abstracts;
using Controller.Resources;
using Model.Entities;

namespace Controller.Strategies
{
    public class LeaveRoomSt : FindRoomARStrategyBase
    {
        public LeaveRoomSt(string roomname) : base(roomname, "LEAVE_ROOM")
        {

        }
        public async override Task ExecuteAsync(ClientConnection c)
        {
            if (!await HandleClientIdentificationValidationAsync(c))
                return;

            ChatRoom? room = ChatData.Instance.GetRoomOrNull(_roomname);

            // Si la sala no existe:

            if (room == null)
            {
                await ReplyNoSuchRoomAsync(c);
                return;
            }

            // Si el usuario no es miembro:

            if (!room.IsMember(c.User.Username))
            {
                await ReplyNotJoinedAsync(c);
                return;
            }

            // Si la sala existe y es miembro:

            room.Remove(c.User.Username);
            await NotifyLeftRoom(c, room);

        }

        #region Apoyo

        /// <summary>
        /// Responde al usuario que no está unido a la sala y por tanto no puede
        /// dejarla.
        /// </summary>
        /// <param name="client">Usuario al que se le responde.</param>
        /// <returns></returns>
        private async Task ReplyNotJoinedAsync(ClientConnection client)
        {
            MsgBuilder mb = new();
            string response = mb.WithType("RESPONSE")
                                .WithOperation("LEAVE_ROOM")
                                .WithResult("NOT_JOINED")
                                .WithExtra(_roomname)
                                .Build();
            await SendMessageAsync(client, response);
        }

        /// <summary>
        /// Notifica a todos los miembros de una sala que un miembro ha salido.
        /// </summary>
        /// <param name="client">Usuario que ha salido de la sala.</param>
        /// <param name="room">Sala de donde se sale.</param>
        /// <returns></returns>
        private async Task NotifyLeftRoom(ClientConnection client, ChatRoom room)
        {
            MsgBuilder mb = new();
            string response = mb.WithType("LEAVE_ROOM")
                                .WithRoomname(_roomname)
                                .WithUsername(client.User.Username)
                                .Build();
            List<Task> task = [];
            foreach (var (username, user) in room.Members)
            {
                ClientConnection userC = ChatData.Instance.GetUserOrNull(username)!;
                task.Add(SendMessageAsync(userC, response));
            }
            await Task.WhenAll(task);

        }

        #endregion
    }
}
