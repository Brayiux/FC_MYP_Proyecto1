using Client.Models.Definitions;
using Client.Models.Entities;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.ObjectModel;

namespace Client.ViewModels.Chat.ViewModels
{
    public abstract partial class ChatRoomViewModelBase : ObservableObject
    {
        #region Eventos

        public event EmptiedRoomEventHandler EmptiedRoom
        {
            add => Room.EmptiedRoom += value;
            remove => Room.EmptiedRoom -= value;
        }

        #endregion

        #region Contexto

        /// <summary>
        /// El nombre de la sala.
        /// </summary>
        [ObservableProperty]
        private string _roomname = string.Empty;

        /// <summary>
        /// Los mensajes que existen en la sala.
        /// </summary>
        [ObservableProperty]
        private ObservableCollection<ChatMsg> _messages = [];

        #endregion

        #region Propiedades

        public ChatRoom Room { get; }

        #endregion

        #region Construcción

        public ChatRoomViewModelBase(string roomname)
        {
            Room = new(roomname);
            Roomname = roomname;
        }

        #endregion

        #region Acceso público

        /// <summary>
        /// Añade un mensaje a la lista de mensajes de la sala.
        /// </summary>
        /// <param name="msg"></param>
        public void AddMsg(ChatMsg msg)
        {
            ArgumentNullException.ThrowIfNull(msg);

            Messages.Add(msg);
        }

        /// <summary>
        /// Elimina todos los mensajes de la sala.
        /// </summary>
        public void ClearMsgs()
        {
            Messages.Clear();
        }

        #endregion

        #region Abstractos
        /// <summary>
        /// Añade a un miembro previamente invitado a la sala.
        /// </summary>
        /// <param name="username">Nombre de usuario del invitado.</param>
        /// <returns><see langword="true"/> si el miembro estaba invitado
        /// y pudo ser añadido, y <see langword="false"/> de lo contrario.</returns>
        public abstract bool AddMember(string username);

        /// <summary>
        /// Remueve a un miembro de la sala.
        /// </summary>
        /// <param name="username">Nombre de usuario del miembro.</param>
        /// <returns><see langword="true"/> si el usuario era miembro y pudo
        /// ser removido, y <see langword="false"/> de lo contrario.</returns>
        public abstract bool RemoveMember(string username);

        /// <summary>
        /// Añade una invitación a un usuario.
        /// </summary>
        /// <param name="user">Usuario que se invita.</param>
        /// <returns></returns>
        public abstract bool AddGuest(ChatUserViewModel user);

        /// <summary>
        /// Remueve una invitación.
        /// </summary>
        /// <param name="username">Usuario cuya incitación se remueve.</param>
        /// <returns></returns>
        public abstract bool RemoveGuest(string username);

        #endregion
    }
}
