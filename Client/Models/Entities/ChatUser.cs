using System;
namespace Client.Models.Entities
{
    /// <summary>
    /// Contiene los datos de un usuario en el chat y los valida.
    /// </summary>
    public class ChatUser
    {
        #region Propiedades
        /// <summary>
        /// Obtiene el nombre del usuario.
        /// </summary>
        public string Username { get; }
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

        #endregion
    }
}
