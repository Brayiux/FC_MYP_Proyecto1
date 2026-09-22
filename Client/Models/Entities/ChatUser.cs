using Client.Models.Definitions;
using System;
namespace Client.Models.Entities
{
    /// <summary>
    /// Contiene los datos de un usuario en el chat y los valida.
    /// </summary>
    public class ChatUser
    {
        #region Campos

        /// <summary>
        /// Estado actual del cliente en el chat.
        /// </summary>
        private UserStatus _status;

        #endregion

        #region Propiedades
        /// <summary>
        /// Obtiene el nombre del usuario.
        /// </summary>
        public string Username { get; }

        /// <summary>
        /// Obtiene y modifica el estado actual del usuario.
        /// </summary>
        public UserStatus Status
        {
            get => _status;
            set
            {
                ValidateStatus(value);
                _status = value;
            }
        }

        #endregion

        #region Construcción

        /// <summary>
        /// Crea un nuevo usuario del chat con el nombre indicado.
        /// </summary>
        /// <param name="username">Nombre del usuario.</param>
        public ChatUser(string username)
        {
            ValidateUsername(username);
            Username = username;
        }

        #endregion


        #region Apoyo

        /// <summary>
        /// Valida que el nombre de usuario sea válido (que no sea
        /// nulo y no salga del rango de 1 a 8 caracteres).
        /// </summary>
        /// <param name="username">Nombre del usuario</param>
        /// <exception cref="ArgumentException">Si <paramref name="username"/>
        /// es nulo o tiene longitud igual a 0 o mayor a 8.</exception>
        private void ValidateUsername(string username)
        {
            if (username == null ||
                username.Length is 0 or > 8)
            {
                throw new ArgumentException(
                    "Error: Nombre de usuario nulo o sale del rango de 1 a 8 caracteres.",
                    nameof(username));
            }
        }

        /// <summary>
        /// Realiza la validación necesaria para el nuevo estado del usuario.
        /// </summary>
        /// <param name="newStatus">Estado nuevo para el usuario.</param>
        /// <exception cref="InvalidOperationException">Si <paramref name="newStatus"/>
        /// coincide con el estado actual del usuario.</exception>
        private void ValidateStatus(UserStatus newStatus)
        {
            if (newStatus == _status)
            {
                throw new InvalidOperationException(
                    "Error: Estado del usuario modificado al mismo valor.");
            }
        }

        #endregion
    }
}
