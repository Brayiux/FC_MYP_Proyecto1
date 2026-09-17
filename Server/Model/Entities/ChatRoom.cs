using Model.Exceptions;

namespace Model.Entities
{
    /// <summary>
    /// Contiene y valida la información de una sala del chat, así
    /// como sus miembros, invitaciones y otra información que se
    /// requiera.
    /// </summary>
    public class ChatRoom
    {

        #region Campos

        /// <summary>
        /// Nombre de la sala.
        /// </summary>
        private string _roomname;

        /// <summary>
        /// Diccionario username-miembro que guarda a los miembros de la sala.
        /// </summary>
        private readonly Dictionary<string, ChatUser> _members = [];

        /// <summary>
        /// Diccionario de username-invitado que guarda a los usuarios invitados
        /// a la sala.
        /// </summary>
        private readonly Dictionary<string, ChatUser> _invitations = [];

        #endregion

        #region Propiedades

        /// <summary>
        /// Obtiene y cambia el nombre de la sala.
        /// </summary>
        /// <remarks>
        /// Realiza la validación del nombre.
        /// </remarks>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException">Si el valor
        /// al que se intenta modificar sale del rango de 1 a 16 caracteres.</exception>
        public string Roomname
        {
            get => _roomname;
            set
            {
                ValidateRoomname(value);
                _roomname = value;
            }
        }
        /// <summary>
        /// Obtiene al usuario dueño de la sala, el cual no puede
        /// modificarse.
        /// </summary>
        public ChatUser Owner { get; }

        /// <summary>
        /// Obtiene el diccionario de sólo lectura de los miembros de la sala.
        /// </summary>
        public IReadOnlyDictionary<string, ChatUser> Members => _members;

        #endregion

        #region Construcción

        /// <summary>
        /// Construye una nueva sala para el chat con un nombre y un
        /// dueño, a los cuales valida.
        /// </summary>
        /// <param name="roomname">Nombre de la sala.</param>
        /// <param name="owner">Dueño de la sala.</param>
        /// <exception cref="ArgumentNullException">Si algún argumento
        /// es nulo.</exception>
        /// <exception cref="RoomnameOutOfRangeException">Si el nombre
        /// de la sala sale del rango de 1 a 16 caracteres.</exception>
        public ChatRoom(string roomname, ChatUser owner)
        {
            ValidateRoomname(roomname);
            ValidateOwner(owner);
            _roomname = roomname;
            Owner = owner;
            _members[owner.Username] = owner;
        }

        #endregion

        #region Acceso Público

        /// <summary>
        /// Añade un usuario al diccionario de invitados en esta sala.
        /// </summary>
        /// <param name="cu">Usuario que es invitado.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public void AddInvitation(ChatUser cu)
        {
            ArgumentNullException.ThrowIfNull(cu);

            _invitations[cu.Username] = cu;
        }
        
        /// <summary>
        /// Remueve a un usuario del diccionario de invitados en esta sala.
        /// </summary>
        /// <param name="username">Nombre del usuario.</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <returns></returns>
        public bool RemoveInvitation(string username)
        {
            ArgumentNullException.ThrowIfNull(username);

            return _invitations.Remove(username);
        }

        /// <summary>
        /// Remueve todas las invitaciones de la sala.
        /// </summary>
        public void ClearInvitations()
        {
            _invitations.Clear();
        }

        /// <summary>
        /// Añade como miembro a un usuario que fue invitado previamente.
        /// </summary>
        /// <param name="username">Nombre del usuario que fue invitado.</param>
        /// <exception cref="UserNotInvitedException">Si no hay ningún usuario
        /// con el <paramref name="username"/> invitado en esta sala.</exception>
        public void Add(string username)
        {
            if (!_invitations.TryGetValue(username, out ChatUser user))
            {
                throw new UserNotInvitedException(
                    $"Error: No puede agregar a {username} a la sala {Roomname} porque no ha sido invitado.");
            }
            _members[user.Username] = user;
        }

        /// <summary>
        /// Remueve a un miembro de la sala.
        /// </summary>
        /// <param name="username">Nombre del miembro a remover.</param>
        /// <exception cref="UserNotFoundException">Si no hay ningún miembro
        /// de la sala con el <paramref name="username"/>.</exception>
        public void Remove(string username)
        {
            if (!_members.Remove(username))
            {
                throw new UserNotFoundException(
                    $"Error: No puede remover a {username} a la sala {Roomname} porque no es miembro.");
            }
        }

        /// <summary>
        /// Indica si un usuario ha sido invitado a esta sala.
        /// </summary>
        /// <param name="username">Nombre del usuario.</param>
        /// <returns><see langword="true"/> si el usuario con el 
        /// <paramref name="username"/> está en el diccionario de
        /// invitados de esta sala, y <see langword="false"/> si no.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public bool IsInvited(string username)
        {
            ArgumentNullException.ThrowIfNull(username);
            return _invitations.ContainsKey(username);
        }

        /// <summary>
        /// Indica si un usuario es miembro de esta sala.
        /// </summary>
        /// <param name="username">Nombre del usuario.</param>
        /// <returns><see langword="true"/> si el usuario está registrado
        /// en el diccionario de miembros y <see langword="false"/> si no.
        /// </returns>
        public bool IsMember(string username)
        {
            ArgumentNullException.ThrowIfNull(username);
            return _members.ContainsKey(username);
        }
        #endregion

        #region Apoyo

        /// <summary>
        /// Valida que el nombre de la sala se encuentre en un rango
        /// de 1 a 16 caracteres.
        /// </summary>
        /// <param name="roomname">Nombre de la sala.</param>
        /// <exception cref="ArgumentNullException">Si <paramref name="roomname"/>
        /// es <see langword="null"/>.</exception>
        /// <exception cref="RoomnameOutOfRangeException">Si la longitud
        /// de <paramref name="roomname"/> sale del rango de 1 a 16
        /// caracteres.</exception>
        private void ValidateRoomname(string roomname)
        {
            ArgumentNullException.ThrowIfNull(roomname);

            if (roomname.Length is 0 or > 16)
            {
                throw new RoomnameOutOfRangeException(
                    $"Error: El roomname {roomname} sale del rango de 1 a 16 caracteres.");
            }
        }

        /// <summary>
        /// Valida que el dueño de la sala no sea nulo.
        /// </summary>
        /// <param name="owner">Dueño de la sala.</param>
        /// <exception cref="ArgumentNullException">Si <paramref name="owner"/>
        /// es <see langword="null"/>.</exception>
        private void ValidateOwner(ChatUser owner)
        {
            ArgumentNullException.ThrowIfNull(owner);
        }

        #endregion
    }
}
