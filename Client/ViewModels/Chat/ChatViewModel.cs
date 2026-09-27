using Client.ViewModels.Chat.ViewModels;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Client.ViewModels.Chat
{
    /// <summary>
    /// Controla el flujo de todas las componentes del chat
    /// y la comunicación con el servidor, para notificarlo
    /// a la vista del chat.
    /// </summary>
    public partial class ChatViewModel : ViewModelBase
    {
        #region Componentes

        /// <summary>
        /// Panel de invitaciones.
        /// </summary>
        [ObservableProperty]
        private InvitationsPanelViewModel _invitationsPanel = new();

        /// <summary>
        /// Panel principal.
        /// </summary>
        [ObservableProperty]
        private MainPanelViewModel _mainPanel = new();

        /// <summary>
        /// Panel de salas.
        /// </summary>
        [ObservableProperty]
        private RoomsPanelViewModel _roomsPanel = new();

        /// <summary>
        /// Panel de chat.
        /// </summary>
        [ObservableProperty]
        private ChatPanelViewModel _chatPanel = new();
        
        /// <summary>
        /// Panel de usuarios.
        /// </summary>
        [ObservableProperty]
        private UsersPanelViewModel _usersPanel = new();

        #endregion
    }
}
