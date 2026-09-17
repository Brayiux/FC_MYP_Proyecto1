using Model.Exceptions;

namespace Model.Entities
{
    /// <summary>
    /// Contiene y valida la información de una sala del chat privada,
    /// (entre dos usuarios diferentes).
    /// </summary>
    public class PrivateChatRoom
    {

        #region Campos

        /// <summary>
        /// Diccionario que contiene a los dos miembros de esta sala.
        /// </summary>
        private readonly Dictionary<string, ChatUser> _members = new(capacity: 2);

        /// <summary>
        /// Nombre de la sala privada.
        /// </summary>
        private string _roomname;

        #endregion

        #region Propiedades

        /// <summary>
        /// Obtiene el diccionario de miembros de la sala privada.
        /// </summary>
        public IReadOnlyDictionary<string, ChatUser> Members => _members;

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

        #endregion

        #region Construcción

        /// <summary>
        /// Construye una sala privada entre dos miembros del chat.
        /// </summary>
        /// <param name="roomname">Nombre de la sala privada.</param>
        /// <param name="member1">Un miembro de la sala privada.</param>
        /// <param name="member2">El otro miembro.</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException">Si el nombre de la
        /// sala se sale del rango de 1 a 16 caracteres.</exception>
        /// <exception cref="InvalidOperationException">Si ambos miembros son
        /// el mismo.
        /// </exception>
        public PrivateChatRoom(string roomname, ChatUser member1, ChatUser member2)
        {
            ValidateRoomname(roomname);
            ValidateMembers(member1, member2);

            _roomname = roomname;
            _members[member1.Username] = member1;
            _members[member2.Username] = member2;
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
        /// Valida que ninguno de los miembros del chat sean nulos
        /// </summary>
        /// <param name="member1">Uno de los miembros.</param>
        /// <param name="member2">El otro miembro.</param>
        /// <exception cref="ArgumentNullException">Si alguno de los miembros es
        /// nulo.</exception>
        /// <exception cref="InvalidOperationException">Si ambos miembros son el mismo.
        /// </exception>
        private void ValidateMembers(ChatUser member1, ChatUser member2)
        {
            ArgumentNullException.ThrowIfNull(member1);
            ArgumentNullException.ThrowIfNull(member2);

            if (member1.Equals(member2))
            {
                throw new InvalidOperationException(
                    "Error: Dos miembros iguales no pueden estar en una sala privada.");
            }
        }

        #endregion
    }
}
