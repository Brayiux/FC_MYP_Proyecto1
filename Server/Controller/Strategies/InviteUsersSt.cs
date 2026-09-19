using Controller.Data;
using Controller.Definitions.Abstracts;
using Controller.Resources;
using Model.Entities;
using System.Threading.Tasks;

namespace Controller.Strategies
{
    public class InviteUsersSt : FindRoomARStrategyBase
    {
        /// <summary>
        /// Lista de usuarios que el cliente desea invitar a la sala.
        /// </summary>
        private readonly IReadOnlyList<string> _users;

        public InviteUsersSt(string roomname, IReadOnlyList<string> users) : base(roomname, "INVITE")
        {
            _users = users;
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

            // Si no es miembro de la sala:

            if (!room.IsMember(c.User.Username))
            {
                // Lo desconectamos, pues el protocolo no especifica respuesta alguna.
                await ReplyInvalidAsync(c);
                ChatData.Instance.RemoveUser(c.User.Username);
                ChatData.Instance.RemoveClient(c.Id);
                DisconnectClient(c);
                return;
            }

            string? firstNI = GetFirstNotIdentifiedOrNull(_users);

            // Si algún usuario de los invitados no existe:

            if (firstNI != null)
            {
                await ReplyNoSuchUserAsync(c, firstNI);
                return;
            }

            // Enviamos los mensajes de invitaciones a los usuarios:

            await HandleInvitationsAsync(c, _users, room);

        }

        #region Apoyo

        /// <summary>
        /// Le responde al cliente que al menos un usuario no fue encontrado
        /// y regresa el <i>username</i> del primero que no encontró.
        /// </summary>
        /// <param name="client">Cliente a quien se responde.</param>
        /// <param name="firstNI">Primer usuario no identificado.</param>
        /// <returns></returns>
        private async Task ReplyNoSuchUserAsync(ClientConnection client, string firstNI)
        {
            MsgBuilder mb = new();
            string response = mb.WithType("RESPONSE")
                                .WithOperation("INVITE")
                                .WithResult("NO_SUCH_USER")
                                .WithExtra(firstNI)
                                .Build();
            await SendMessageAsync(client, response);
        }

        /// <summary>
        /// Notifica a los usuarios indicados que han sido invitados a la <paramref name="room"/>
        /// por <paramref name="client"/>, y los añade a su lista de invitados.
        /// </summary>
        /// <param name="client">Usuario que invita.</param>
        /// <param name="usernames">Nombres de usuario de los invitados.</param>
        /// <param name="room">Sala a la cual se les invita.</param>
        /// <returns></returns>
        private async Task HandleInvitationsAsync(ClientConnection client, IReadOnlyList<string> usernames, ChatRoom room)
        {
            MsgBuilder mb = new();
            string notification = mb.WithType("INVITATION")
                                    .WithUsername(client.User.Username)
                                    .WithRoomname(_roomname)
                                    .Build();
            List<Task> tasks = [];
            foreach (var username in usernames)
            {
                if (room.IsMember(username) || room.IsInvited(username))
                {
                    continue;
                }
                ClientConnection user = ChatData.Instance.GetUserOrNull(username)!;
                room.AddInvitation(user.User);
                var task = SendMessageAsync(user, notification);
                tasks.Add(task);
            }
            await Task.WhenAll(tasks);
        }

        /// <summary>
        /// Obtiene el primer cliente no identificado en el chat dada una lista de
        /// usernames.
        /// </summary>
        /// <param name="clients">Nombres de usuario a verificar.</param>
        /// <returns>El primer <i>username</i> que no fue identificado en el chat
        /// o <see langword="null"/> si todos fueron identificados.</returns>
        private string? GetFirstNotIdentifiedOrNull(IReadOnlyList<string> clients)
        {
            foreach (var c in clients)
            {
                if (!ChatData.Instance.ExistsUser(c))
                    return c;
            }
            return null;
        }
        #endregion

    }
}
