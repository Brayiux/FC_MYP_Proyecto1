using Client.Models.Definitions;
using Client.Models.Entities;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
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

        #endregion

        #region Propiedades

        /// <summary>
        /// Estado del usuario.
        /// </summary>
        public UserStatus Status
        {
            get => _user.Status;
            set
            {
                _user.Status = value;
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

        #region Campos

        /// <summary>
        /// Usuario del modelo.
        /// </summary>
        private readonly ChatUser _user;

        #endregion

        #region Construcción

        /// <summary>
        /// Construye un nuevo ente vista-modelo que sirve
        /// como canal de comunicación en el 
        /// </summary>
        /// <param name="username"></param>
        public ChatUserViewModel(string username)
        {
            _user = new(username);
            Username = username;
        }
        #endregion

        #region Apoyo

        // El nombre de usuario no se puede modificar
        partial void OnUsernameChanged(string value)
        {
            throw new InvalidOperationException(
                "Error: El nombre de usuario no puede ser modificado");
        }
        #endregion

    }
}
