using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Client.ViewModels.Chat.ViewModels
{
    /// <summary>
    /// Contiene la información y operaciones necesarias para
    /// notificar a la vista del panel de usuarios.
    /// </summary>
    public partial class UsersPanelViewModel : ViewModelBase
    {
        #region Eventos
        /// <summary>
        /// Ocurre cuando un usuario de la lista es seleccionado.
        /// </summary>
        public event Action<ChatUserViewModel?>? UserSelected;

        #endregion

        #region Contexto

        /// <summary>
        /// Lista de usuarios del panel.
        /// </summary>
        [ObservableProperty]
        private ObservableCollection<ChatUserViewModel> _users = [];

        /// <summary>
        /// Usuario seleccionado de la lista.
        /// </summary>
        [ObservableProperty]
        private ChatUserViewModel? _selectedUser;

        #endregion

        #region Campos

        /// <summary>
        /// Identifica un <see cref="ChatUserViewModel"/> por el nombre del usuario.
        /// </summary>
        private readonly Dictionary<string, ChatUserViewModel> _usernameToUserMap = [];
        #endregion


        #region Acceso público

        /// <summary>
        /// Provee la lista de usuarios al panel.
        /// </summary>
        /// <param name="users"></param>
        public void SetUsers(IEnumerable<ChatUserViewModel> users)
        {
            Users = new(users);

            _usernameToUserMap.Clear();

            foreach (var user in users)
            {
                _usernameToUserMap[user.Username] = user;
            }
        }

        /// <summary>
        /// Permite añadir un usuario al panel, sólo si no es que ya
        /// se encuentra dentro.
        /// </summary>
        /// <param name="user">Usuario a introducir al panel.</param>
        /// <returns><see langword="true"/> si el usuario no estaba en el panel
        /// y se añadió y <see langword="false"/> si no.</returns>
        public bool AddUser(ChatUserViewModel user)
        {
            if (Users.Contains(user))
                return false;

            Users.Add(user);

            _usernameToUserMap[user.Username] = user;

            return true;
        }

        /// <summary>
        /// Remueve a un usuario del panle por su nombre, si está dentro.
        /// </summary>
        /// <param name="username"></param>
        /// <returns><see langword="true"/> si el usuario estaba dentro del panel
        /// y se pudo remover, y <see langword="false"/> si no.</returns>
        public bool RemoveUser(string username)
        {
            if (!_usernameToUserMap.Remove(username, out ChatUserViewModel? user))
                return false;
            return Users.Remove(user); ;
        }

        #endregion

        #region Apoyo

        // Notifica el evento
        partial void OnSelectedUserChanged(ChatUserViewModel? value)
        {
            UserSelected?.Invoke(value);
        }

        #endregion

    }
}
