using System;
using System.Collections.Generic;
using System.Text;

namespace Client.Models.Entities
{
    /// <summary>
    /// Es un Singleton que representa la única instancia de la sala
    /// principal (aquella donde todos los usuarios identificados pueden
    /// mandar mensaje)
    /// </summary>
    public class MainChatRoom : ChatRoom
    {

        #region Campos
        /// <summary>
        /// La única instancia de la sala principal
        /// </summary>
        private static MainChatRoom? _instance;

        #endregion

        #region Propiedades
        /// <summary>
        /// Obtiene la única instancia de la sala principal.
        /// </summary>
        public static MainChatRoom Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new();
                return _instance;
            }
        }
        #endregion

        #region Construcción
        /// <summary>
        /// Constructor privado para patrón Singleton.
        /// </summary>
        private MainChatRoom() : base("Chat Principal") { }

        #endregion
    }
}
