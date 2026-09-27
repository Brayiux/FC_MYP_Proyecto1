using Client.ViewModels.Chat.ViewModels;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Client.ViewModels.Chat
{
    public partial class ChatViewModel : ViewModelBase
    {
        #region Componentes

        /// <summary>
        /// Panel de usuarios.
        /// </summary>
        [ObservableProperty]
        private UsersPanelViewModel _usersPanel = new();

        /// <summary>
        /// Panel de salas.
        /// </summary>
        [ObservableProperty]
        private RoomsPanelViewModel _roomsPanel = new();

        /// <summary>
        /// Panel de invitaciones.
        /// </summary>
        [ObservableProperty]
        private InvitationsPanelViewModel _invitationsPanel = new();

        #endregion
    }
}
