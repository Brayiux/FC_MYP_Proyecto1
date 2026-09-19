using Controller.Data;
using Controller.Definitions.Abstracts;
using Controller.Resources;
using Model.Definitions.Enums;
using Model.Entities;

namespace Controller.Strategies
{
    public class RoomUsersSt : FindRoomARStrategyBase
    {

        public RoomUsersSt(string roomname) : base(roomname, "ROOM_USERS")
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
            // Si existe pero el usuario no es miembro
            if (!room.IsMember(c.User.Username))
            {
                await ReplyNotJoinedAsync(c);
                return;
            }

            // Si existe y es miembro:
            Dictionary<string, UserStatus> usersData = GetUsersDataInRoom(room);
            await ReplyRoomUsersAsync(c, usersData);

        }

        #region Apoyo

        /// <summary>
        /// Responde al usuario que no está unido a la sala y por tanto no puede
        /// pedir la lista de usuarios de la misma.
        /// </summary>
        /// <param name="client">Usuario al que se le responde.</param>
        /// <returns></returns>
        private async Task ReplyNotJoinedAsync(ClientConnection client)
        {
            MsgBuilder mb = new();
            string response = mb.WithType("RESPONSE")
                                .WithOperation("ROOM_USERS")
                                .WithResult("NOT_JOINED")
                                .WithExtra(_roomname)
                                .Build();
            await SendMessageAsync(client, response);
        }

        /// <summary>
        /// Responde y envía al usuario el diccionario de <i>username</i>-estado de los
        /// usuarios indicados.
        /// </summary>
        /// <param name="client">Usuario al que se le responde y hace el envío del diccionario.</param>
        /// <param name="usersData">Diccionario de nombres de usuario y estados.</param>
        /// <returns></returns>
        private async Task ReplyRoomUsersAsync(ClientConnection client, IReadOnlyDictionary<string, UserStatus> usersData)
        {
            MsgBuilder mb = new();
            string response = mb.WithType("ROOM_USER_LIST")
                                .WithRoomname(_roomname)
                                .WithUsers(usersData)
                                .Build();
            await SendMessageAsync(client, response);
        }
        
        /// <summary>
        /// Regresa todos los datos <i>username</i>-estado de los usuarios en una sala.
        /// </summary>
        /// <param name="room">Sala de donde se extraen los usuarios.</param>
        /// <returns></returns>
        private Dictionary<string, UserStatus> GetUsersDataInRoom(ChatRoom room)
        {
            Dictionary<string, UserStatus> usersData = [];

            foreach (var (username, user) in room.Members)
            {
                ClientConnection userC = ChatData.Instance.GetUserOrNull(username)!;
                usersData[username] = userC.User.Status;
            }
            return usersData;
        }

        #endregion
    }
}
