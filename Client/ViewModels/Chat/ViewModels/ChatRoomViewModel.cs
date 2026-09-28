using System;
using Client.Models.Entities;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace Client.ViewModels.Chat.ViewModels
{
    /// <summary>
    /// Representa a una sala ordinaria creada por cualquier usuario.
    /// </summary>
    public partial class ChatRoomViewModel : ChatRoomViewModelBase
    {
        /// <summary>
        /// Crea una sala con un nombre.
        /// </summary>
        /// <param name="roomname"></param>
        public ChatRoomViewModel(string roomname) : base(roomname) { }

        #region Acceso Público

        public override bool AddMember(string username)
        {
            return Room.AddMember(username);
        }

        public override bool RemoveMember(string username)
        {
            return Room.RemoveMember(username);
        }

        public override bool AddGuest(ChatUserViewModel user)
        {
            return Room.AddGuest(user.User);
        }

        public override bool RemoveGuest(string username)
        {
            return Room.RemoveGuest(username);
        }

        #endregion
    }
}
