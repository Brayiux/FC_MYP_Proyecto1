using System;
using System.Collections.Generic;
using System.Text;

namespace Client.Models.Entities
{
    /// <summary>
    /// Contiene la información de una invitación a una sala al cliente.
    /// </summary>
    public class ChatRoomInvitation
    {
        #region Propiedades

        /// <summary>
        /// Obtiene el nombre de la habitación a donde se le invita al cliente.
        /// </summary>
        public string Roomname { get; }

        #endregion

        #region Construcción

        /// <summary>
        /// Crea una invitación a una sala por su nombre.
        /// </summary>
        /// <param name="roomname">Nombre de la sala a donde se invita.</param>
        public ChatRoomInvitation(string roomname)
        {
            ArgumentNullException.ThrowIfNull(roomname);

            Roomname = roomname;
        }

        #endregion
    }
}
