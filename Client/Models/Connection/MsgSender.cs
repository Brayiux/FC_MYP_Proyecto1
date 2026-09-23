using Client.Models.Definitions;
using Client.Models.Resources;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace Client.Models.Connection
{
    /// <summary>
    /// Singleton que sirve como un punto de comunicación accesible
    /// con el servidor (a través de la conexión del cliente) sólo 
    /// para el envío de mensajes.
    /// </summary>
    public class MsgSender
    {
        #region Campos

        /// <summary>
        /// Única instancia de este emisor de mensajes.
        /// </summary>
        private static MsgSender? _instance;

        /// <summary>
        /// Configuraciones de serialización para los paquetes de mensajes
        /// al servidor.
        /// </summary>
        private JsonSerializerOptions _jso = new()
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };

        #endregion

        #region Propiedades

        /// <summary>
        /// Obtiene o crea la única instancia del emisor de mensajes.
        /// </summary>
        public static MsgSender Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new MsgSender();
                return _instance;
            }
        }

        #endregion

        #region Construcción
        /// <summary>
        /// Constructor privado para el patrón Singleton.
        /// </summary>
        private MsgSender() { }

        /// <summary>
        /// Obtiene una instancia de la conexión con el cliente.
        /// </summary>
        private static ClientConnection Connection => ClientConnection.Instance;

        #endregion

        #region Acceso público

        /// <summary>
        /// Envía un mensaje al servidor para desconectar al cliente.
        /// </summary>
        /// <param name="ct">Token de cancelación de la tarea.</param>
        /// <returns></returns>
        public async Task DisconnectAsync(CancellationToken ct = default)
        {
            if (!Connection.IsConnected)
                return;


            await SendSerializedMsgAsync(
                new()
                {
                    Type = "DISCONNECT",
                },
                ct);
        }

        /// <summary>
        /// Envía un mensaje al servidor para identificar al cliente
        /// con el username especificado.
        /// </summary>
        /// <param name="username">Nombre de usuario para la identificación.</param>
        /// <param name="ct">Token de cancelación de la tarea.</param>
        /// <returns></returns>
        public async Task IdentifyAsync(string username, CancellationToken ct = default)
        {
            if (!Connection.IsConnected)
                return;

            await SendSerializedMsgAsync(
                new()
                {
                    Type = "IDENTIFY",
                    Username = username
                },
                ct);
        }

        /// <summary>
        /// Envía un mensaje al servidor para cambiar el estado del cliente.
        /// </summary>
        /// <param name="newStatus">Nuevo estado del cliente.</param>
        /// <param name="ct">Token de cancelación de la tarea.</param>
        /// <returns></returns>
        public async Task ChangeStatusAsync(UserStatus newStatus, CancellationToken ct = default)
        {
            if (!Connection.IsConnected)
                return;

            await SendSerializedMsgAsync(
                new()
                {
                    Type = "STATUS",
                    Status = newStatus
                },
                ct);


        }

        /// <summary>
        /// Envía un mensaje al servidor para solicitar la lista de usuarios
        /// y estados conectados al chat.
        /// </summary>
        /// <param name="ct">Token de cancelación de la tarea.</param>
        /// <returns></returns>
        public async Task RequestUsersInChatAsync(CancellationToken ct = default)
        {
            if (!Connection.IsConnected)
                return;

            await SendSerializedMsgAsync(
                new()
                {
                    Type = "USERS"
                },
                ct);

        }


        /// <summary>
        /// Envía un mensaje al servidor para enviar un mensaje privado
        /// a un usuario especificado.
        /// </summary>
        /// <param name="username">Nombre de usuario del destinatario.</param>
        /// <param name="text">Mensaje para el destinatario.</param>
        /// <param name="ct">Token de cancelación de la tarea.</param>
        /// <returns></returns>
        public async Task TextToAsync(string username, string text, CancellationToken ct = default)
        {
            if (!Connection.IsConnected)
                return;

            await SendSerializedMsgAsync(
                new()
                {
                    Type = "TEXT",
                    Username = username,
                    Text = text
                },
                ct);
        }

        /// <summary>
        /// Envía un mensaje al servidor para enviar un mensaje público
        /// al chat.
        /// </summary>
        /// <param name="text">Mensaje que se envía al chat.</param>
        /// <param name="ct">Token de cancelación de la tarea.</param>
        /// <returns></returns>
        public async Task SendPublicTextAsync(string text, CancellationToken ct = default)
        {
            if (!Connection.IsConnected || Connection.User == null)
                return;

            await SendSerializedMsgAsync(
                new()
                {
                    Type = "PUBLIC_TEXT",
                    Username = Connection.User.Username,
                    Text = text
                },
                ct);
        }

        /// <summary>
        /// Envía un mensaje al servidor para crear una nueva sala
        /// con el nombre especificado.
        /// </summary>
        /// <param name="roomname">Nombre de la sala.</param>
        /// <param name="ct">Token de cancelación de la tarea.</param>
        /// <returns></returns>
        public async Task CreateNewRoomAsync(string roomname, CancellationToken ct = default)
        {
            if (!Connection.IsConnected)
                return;

            await SendSerializedMsgAsync(
                new()
                {
                    Type = "NEW_ROOM",
                    Roomname = roomname
                },
                ct);
        }

        /// <summary>
        /// Envía un mensaje al servidor para invitar a un usuario a una
        /// sala especificados.
        /// </summary>
        /// <param name="roomname">Nombre de la sala.</param>
        /// <param name="username">Nombre de usuario del invitado.</param>
        /// <param name="ct">Token de cancelación de la tarea.</param>
        /// <returns></returns>
        public async Task InviteToRoomAsync(string roomname, string username, CancellationToken ct = default)
        {
            if (!Connection.IsConnected)
                return;

            await SendSerializedMsgAsync(
                new()
                {
                    Type = "INVITE",
                    Roomname = roomname,
                    Username = username
                },
                ct);
        }

        /// <summary>
        /// Envía un mensaje al servidor para unirse a una sala
        /// especificada.
        /// </summary>
        /// <param name="roomname">Nombre de la sala.</param>
        /// <param name="ct">Token de cancelación de la tarea.</param>
        /// <returns></returns>
        public async Task JoinToRoomAsync(string roomname, CancellationToken ct = default)
        {
            if (!Connection.IsConnected)
                return;

            await SendSerializedMsgAsync(
                new()
                {
                    Type = "JOIN_ROOM",
                    Roomname = roomname
                },
                ct);
        }

        /// <summary>
        /// Envía un mensaje al servidor para solicitar los usuarios
        /// y sus estados dentro de una sala especificada.
        /// </summary>
        /// <param name="roomname">Nombre de la sala.</param>
        /// <param name="ct">Token de cancelación de la tarea.</param>
        /// <returns></returns>
        public async Task RequestRoomUsers(string roomname, CancellationToken ct = default)
        {
            if (!Connection.IsConnected)
                return;

            await SendSerializedMsgAsync(
                new()
                {
                    Type = "ROOM_USERS",
                    Roomname = roomname
                },
                ct);
        }

        /// <summary>
        /// Envía un mensaje al servidor para enviar un mensaje a una sala
        /// especificada.
        /// </summary>
        /// <param name="roomname">Nombre de la sala.</param>
        /// <param name="text">Mensaje que se envía a la sala.</param>
        /// <param name="ct">Token de cancelación de la tarea.</param>
        /// <returns></returns>
        public async Task TextToRoomAsync(string roomname, string text, CancellationToken ct = default)
        {
            if (!Connection.IsConnected)
                return;

            await SendSerializedMsgAsync(
                new()
                {
                    Type = "ROOM_TEXT",
                    Roomname = roomname,
                    Text = text
                },
                ct);
        }

        /// <summary>
        /// Envía un mensaje al servidor para salir de una sala especificada.
        /// </summary>
        /// <param name="roomname">Nombre de la sala.</param>
        /// <param name="ct">Token para la cancelación de la operación.</param>
        /// <returns></returns>
        public async Task LeaveFromRoomAsync(string roomname, CancellationToken ct = default)
        {
            if (!Connection.IsConnected)
                return;

            await SendSerializedMsgAsync(
                new()
                {
                    Type = "LEAVE_ROOM",
                    Roomname = roomname
                },
                ct);
        }


        #endregion

        #region Apoyo

        /// <summary>
        /// Envía, de manera asíncrona, un mensaje a la conexión
        /// del cliente.
        /// </summary>
        /// <param name="msgData">Contenedor de datos a serializar.</param>
        /// <param name="ct">Token de cancelación de la tarea.</param>
        /// <returns></returns>
        private async Task SendSerializedMsgAsync(MsgData msgData, CancellationToken ct)
        {
            await Connection.SendMsgAsync(GetSerialized(msgData), ct);
        }

        /// <summary>
        /// Obtiene un mensaje en formato Json a partir de un objeto
        /// <see cref="MsgData"/> con sus campos establecidos.
        /// </summary>
        /// <param name="msgData">Contenedor de datos para serializar.</param>
        /// <returns></returns>
        private string GetSerialized(MsgData msgData)
        {
            return JsonSerializer.Serialize(msgData, _jso);
        }

        #endregion

    }
}
