using Client.Models.Entities;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Client.ViewModels.Chat.ViewModels
{
    /// <summary>
    /// Vista-modelo del panel de invitaciones del chat.
    /// </summary>
    public partial class InvitationsPanelViewModel : ViewModelBase
    {
        #region Eventos

        /// <summary>
        /// Ocurre cuando una invitación es seleccionada en el panel.
        /// </summary>
        public event Action<ChatRoomInvitation?>? InvitationSelected;

        #endregion

        #region Contexto

        /// <summary>
        /// Invitaciones del panel.
        /// </summary>
        [ObservableProperty]
        private ObservableCollection<ChatRoomInvitation> _invitations = [];

        /// <summary>
        /// Invitación que se encuentra seleccionada en el panel.
        /// </summary>
        [ObservableProperty]
        private ChatRoomInvitation? _selectedInvitation;
        #endregion

        #region Apoyo

        /// Notifica que la invitación fue seleccionada:
        partial void OnSelectedInvitationChanged(ChatRoomInvitation? value)
        {
            InvitationSelected?.Invoke(value);
        }

        #endregion
    }
}
