using Client.Models.Definitions;
using System;
using System.Collections.Generic;

namespace Client.Models.Entities
{
    /// <summary>
    /// Contiene los datos de una sala del chat que lleva un registro de
    /// miembros e invitados, y contiene eventos que se desencadenan con
    /// sus operaciones.
    /// </summary>
    public class ChatRoom
    {
        #region Eventos

        /// <summary>
        /// Ocurre cuando la sala ha removido su último miembro.
        /// </summary>
        public event EmptiedRoomEventHandler? EmptiedRoom;

        #endregion


        #region Campos

        /// <summary>
        /// Diccionario username-usuario para miembros de la sala.
        /// </summary>
        private readonly Dictionary<string, ChatUser> _members = [];
        /// <summary>
        /// Diccionario username-usuario para invitados a la sala.
        /// </summary>
        private readonly Dictionary<string, ChatUser> _guests = [];

        #endregion

        #region Propiedades

        /// <summary>
        /// Obtiene el nombre de la sala.
        /// </summary>
        public string Roomname { get; }

        /// <summary>
        /// Obtiene la lista de miembros de la sala.
        /// </summary>
        public IReadOnlyDictionary<string, ChatUser> Members => _members;

        /// <summary>
        /// Obtiene la lista de invitados a la sala.
        /// </summary>
        public IReadOnlyDictionary<string, ChatUser> Guests => _guests;

        #endregion

        #region Construcción

        /// <summary>
        /// Construye una sala del chat con el nombre indicado
        /// y lo valida.
        /// </summary>
        /// <param name="roomname"></param>
        public ChatRoom(string roomname)
        {
            ValidateRoomname(roomname);
            Roomname = roomname;
        }

        #endregion

        #region Acceso Público

        /// <summary>
        /// Intenta añadir un invitado a la sala e indica si lo ha conseguido.
        /// </summary>
        /// <param name="user">Usuario al que se le invita.</param>
        /// <returns><see langword="true"/> si el usuario ya había sido
        /// invitado o es miembro de la sala, y <see langword="false"/> de
        /// lo contrario.</returns>
        public bool AddGuest(ChatUser user)
        {
            ArgumentNullException.ThrowIfNull(user);
            ArgumentNullException.ThrowIfNull(user.Username);

            string username = user.Username;
            if (_guests.ContainsKey(username) || _members.ContainsKey(username))
            {
                return false;
            }
            _guests[username] = user;
            return true;

        }

        /// <summary>
        /// Remueve un invitado del registro de invitados de la sala
        /// e indica si lo ha conseguido.
        /// </summary>
        /// <param name="username">Nombre del usuario.</param>
        /// <returns><see langword="true"/> si el usuario estaba en el
        /// registro de invitados y fue removido con éxito, y <see langword="false"/>
        /// de lo contrario.</returns>
        public bool RemoveGuest(string username)
        {
            ArgumentNullException.ThrowIfNull(username);

            return _guests.Remove(username);
        }

        /// <summary>
        /// Intenta añadir un usuario a la sala e indica si lo ha conseguido.
        /// </summary>
        /// <param name="username">Nombre del usuario que se unirá.</param>
        /// <returns><see langword="true"/> si el usuario estaba invitado
        /// y se pudo añadir como miembro al registro, y <see langword="false"/> de
        /// lo contrario.</returns>
        public bool AddMember(string username)
        {
            ArgumentNullException.ThrowIfNull(username);

            _guests.Remove(username, out ChatUser? user);
            
            if (user == null)
            {
                return false;
            }
            _members[username] = user;
            
            return true;
        }

        /// <summary>
        /// Intenta remover un miembro de la sala e indica si lo ha conseguido.
        /// </summary>
        /// <remarks>Desencadena el evento <see cref="EmptiedRoom"/> cuando
        /// detecta que se ha removido al último miembro de la sala.</remarks>
        /// <param name="username">Nombre del miembro que se removerá.</param>
        /// <returns><see langword="true"/> si el nombre corresponde al de un
        /// miembro registrado y se pudo eliminar, y <see langword="false"/> de
        /// lo contrario.</returns>
        public bool RemoveMember(string username)
        {
            ArgumentNullException.ThrowIfNull(username);

            bool wasRemoved = _members.Remove(username);
            if (wasRemoved && _members.Count == 0)
            {
                EmptiedRoom?.Invoke(this);
            }
            return wasRemoved;
        }

        #endregion

        #region Apoyo

        /// <summary>
        /// Valida que el nombre de la sala cumpla con el formato
        /// correcto (no nulo y dentro del rango de 1 a 16 caracteres).
        /// </summary>
        /// <param name="roomname">Nombre de la sala.</param>
        /// <exception cref="ArgumentException">Si <paramref name="roomname"/>
        /// es nulo o sale del rango de 1 a 16 caracteres.</exception>
        private void ValidateRoomname(string roomname)
        {
            if (roomname == null ||
                roomname.Length is 0 or > 16)
            {
                throw new ArgumentException(
                    "Error: El nombre de la sala es nulo o sale del rango de 1 a 16 caracteres.",
                    nameof(roomname));
            }
        }

        #endregion

    }
}
