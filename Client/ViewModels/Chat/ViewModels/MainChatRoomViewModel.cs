using System.Collections.Generic;

namespace Client.ViewModels.Chat.ViewModels
{
    /// <summary>
    /// Maneja la lógica de la entidad que representa a la sala
    /// principal del chat. Utiliza el patrón Singleton.
    /// </summary>
    public partial class MainChatRoomViewModel : ChatRoomViewModelBase
    {
        #region Campos

        /// <summary>
        /// Única instancia de la sala principal.
        /// </summary>
        private static MainChatRoomViewModel? _instance;

        /// <summary>
        /// Invitados de la sala principal.
        /// </summary>
        private readonly Dictionary<string, ChatUserViewModel> _guests = [];


        private readonly Dictionary<string, ChatUserViewModel> _users = [];
        #endregion

        #region Propiedades
        /// <summary>
        /// Obtiene la única instancia de la sala principal.
        /// </summary>
        public static MainChatRoomViewModel Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new();
                return _instance;
            }
        }

        /// <summary>
        /// Lleva el registro de los usuarios del chat principal en la
        /// capa de vista-modelo.
        /// </summary>
        public IReadOnlyDictionary<string, ChatUserViewModel> Users => _users;

        #endregion

        #region Construcción

        /// <summary>
        /// Constructor privado de la sala principal para implementar
        /// el patrón Singleton.
        /// </summary>
        private MainChatRoomViewModel() : base("Sala Principal")
        {

        }

        #endregion

        #region Acceso Público

        /// <summary>
        /// Invita y añade directamente a un usuario a la sala
        /// pública.
        /// </summary>
        /// <param name="user">Usuario a añadir.</param>
        /// <returns></returns>
        public bool AddMember(ChatUserViewModel user)
        {
            if (AddGuest(user))
            {
                return AddMember(user.Username);
            }
            return false;
        }

        /// <summary>
        /// Añade a un usuario invitado a la sala pública.
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public override bool AddMember(string username)
        {
            if (Room.AddMember(username))
            {
                _users[username] = _guests[username];
                return true;
            }
            return false;
        }

        /// <summary>
        /// Remueve a un miembro de la sala pública.
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public override bool RemoveMember(string username)
        {
            if (Room.RemoveMember(username))
            {
                return _users.Remove(username);
            }
            return false;
        }

        /// <summary>
        /// Añade un usuario al registro de invitados de la sala principal
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public override bool AddGuest(ChatUserViewModel user)
        {
            if (Room.AddGuest(user.User))
            {
                _guests[user.Username] = user;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Remueve a un usuario del registro de invitados de la sala principal.
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public override bool RemoveGuest(string username)
        {
            if (Room.RemoveGuest(username))
            {
                return _guests.Remove(username);
            }
            return false;
        }

        /// <summary>
        /// Obtiene un usuario del registro de usuarios de la sala.
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public ChatUserViewModel? GetUserOrNull(string username)
        {
            _users.TryGetValue(username, out ChatUserViewModel? user);
            return user;
        }
        #endregion



    }
}
