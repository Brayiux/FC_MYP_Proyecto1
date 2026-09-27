using Client.Models.Definitions;
using Client.Models.Entities;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;

namespace Client.ViewModels.Chat.ViewModels
{
    /// <summary>
    /// Sirve para la comunicación entre la vista y el modelo
    /// de un <see cref="ChatUser"/>.
    /// </summary>
    public partial class ChatUserViewModel : ObservableObject
    {
        #region Propiedades observables

        /// <summary>
        /// Nombre del usuario
        /// </summary>
        [ObservableProperty]
        private string _username = string.Empty;

        /// <summary>
        /// El color correspondiente al estado del usuario.
        /// </summary>
        [ObservableProperty]
        private Color _statusColor = StatusToColorMap[UserStatus.Active];

        /// <summary>
        /// Los mensajes del chat privado del cliente con este usuario.
        /// </summary>
        [ObservableProperty]
        private ObservableCollection<ChatMsg> _messages = [];

        #endregion

        #region Propiedades

        /// <summary>
        /// Obtiene al usuario del modelo.
        /// </summary>
        public ChatUser User { get; }

        /// <summary>
        /// Estado del usuario.
        /// </summary>
        public UserStatus Status
        {
            get => User.Status;
            set
            {
                User.Status = value;
                StatusColor = StatusToColorMap[value];
            }
        }

        #endregion

        #region Constantes

        /// <summary>
        /// Mapeo de estado de usuario a su color correspondiente.
        /// </summary>
        private static readonly IReadOnlyDictionary<UserStatus, Color> StatusToColorMap = new Dictionary<UserStatus, Color>()
        {
            { UserStatus.Active, Color.Green },
            { UserStatus.Away, Color.Orange },
            { UserStatus.Bussy, Color.Brown }
        };

        #endregion

        #region Construcción

        /// <summary>
        /// Construye un nuevo ente vista-modelo que sirve
        /// como canal de comunicación en el 
        /// </summary>
        /// <param name="username"></param>
        public ChatUserViewModel(string username)
        {
            User = new(username);
            Username = username;
        }
        #endregion

        #region Acceso público

        /// <summary>
        /// Añade un mensaje a la lista de mensajes privados.
        /// </summary>
        /// <param name="msg"></param>
        public void AddMsg(ChatMsg msg)
        {
            ArgumentNullException.ThrowIfNull(msg);
            Messages.Add(msg);
        }

        /// <summary>
        /// Limpia la lista de mensajes del chat privado.
        /// </summary>
        public void ClearMsgs()
        {
            Messages.Clear();
        }

        #endregion

        #region Apoyo

        #endregion

    }
}
