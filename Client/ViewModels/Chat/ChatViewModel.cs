using Client.Models.Connection;
using Client.Models.Definitions;
using Client.Models.Entities;
using Client.Models.Resources;
using Client.ViewModels.Chat.ViewModels;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Client.ViewModels.Chat
{
    /// <summary>
    /// Controla el flujo de todas las componentes del chat
    /// y la comunicación con el servidor, para notificarlo
    /// a la vista del chat.
    /// </summary>
    public partial class ChatViewModel : ViewModelBase
    {
        #region Campos

        /// <summary>
        /// Sala actualmente seleccionada.
        /// </summary>
        private ChatRoomViewModelBase? _currentRoom;

        /// <summary>
        /// Usuario actualmente seleccionado.
        /// </summary>
        private ChatUserViewModel? _currentChatUser;

        /// <summary>
        /// El usuario que usa la aplicación.
        /// </summary>
        private ChatUserViewModel? _client;


        #endregion

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

        #region Propiedades

        /// <summary>
        /// Obtiene la conexión con el servidor.
        /// </summary>
        private ClientConnection Connection => ClientConnection.Instance;

        /// <summary>
        /// Obtiene el emisor de mensajes al servidor
        /// </summary>
        private MsgSender Sender => MsgSender.Instance;

        /// <summary>
        /// Obtiene el receptor de mensajes que llegan desde el servidor.
        /// </summary>
        private MsgReceiver Receiver => MsgReceiver.Instance;

        /// <summary>
        /// Obtiene la sala principal.
        /// </summary>
        private MainChatRoomViewModel MainRoom => MainChatRoomViewModel.Instance;

        #endregion

        #region Inicialización

        public async Task InitializeAsync()
        {
            SuscribeToComponentsEvents();

            // Inicializamos y añadimos la sala principal al panel:
            ChatUserViewModel user = new(Connection.User!.Username);
            _client = user;
            MainRoom.AddMember(user);
            RoomsPanel.AddRoom(MainRoom);

            // Comenzamos a escuchar los mensajes del servidor:
            Receiver.MsgReceived += Receiver_MsgReceived;

            // Actualizamos el panel principal:
            MainPanel.ResetStatusOptions(user.Status);
            MainPanel.HeaderText = $"Chat de {user.User.Username}";

            // Solicitamos la lista de usuarios:
            await Sender.RequestUsersInChatAsync();
        }

        

        #endregion

        #region Apoyo a la inicialización

        /// <summary>
        /// Hace que se suscriba a los eventos de sus componentes
        /// necesarios para coordinar el flujo.
        /// </summary>
        private void SuscribeToComponentsEvents()
        {
            // Panel de invitaciones:
            InvitationsPanel.InvitationSelected += InvitationsPanel_InvitationSelected;

            // Panel principal:
            MainPanel.TryChangeStatus += MainPanel_TryChangeStatus;
            MainPanel.TryDisconnect += MainPanel_TryDisconnect;
            MainPanel.TryInvite += MainPanel_TryInvite;
            MainPanel.TryCreateRoom += MainPanel_TryCreateRoom;
            // Panel de salas:
            RoomsPanel.RoomSelected += RoomsPanel_RoomSelected;

            // Panel de chat:
            ChatPanel.MessageSent += ChatPanel_MessageSent;

            // Panel de usuarios
            UsersPanel.UserSelected += UsersPanel_UserSelected;
        }

        

        #endregion

        #region Apoyo a eventos de las componentes

        private void UsersPanel_UserSelected(ChatUserViewModel? obj)
        {
            throw new NotImplementedException();
        }

        private async void ChatPanel_MessageSent(ChatMsg obj)
        {
            if (_currentRoom != null)
            {
                _currentRoom.AddMsg(obj);
                ChatPanel.AddMsg(obj);
                await Sender.SendPublicTextAsync(obj.Text);
                return;
            }
            else if (_currentChatUser != null)
            {
                _currentChatUser.AddMsg(obj);
                ChatPanel.AddMsg(obj);
                await Sender.TextToAsync(_currentChatUser.User.Username, obj.Text);
            }
        }

        private void RoomsPanel_RoomSelected(ChatRoomViewModel? obj)
        {
            throw new NotImplementedException();
        }
        private void MainPanel_TryCreateRoom()
        {
            throw new NotImplementedException();
        }

        private void MainPanel_TryInvite()
        {
            throw new System.NotImplementedException();
        }

        private void MainPanel_TryDisconnect()
        {
            throw new System.NotImplementedException();
        }

        private async void MainPanel_TryChangeStatus(UserStatus obj)
        {
            await Sender.ChangeStatusAsync(obj);
            MainPanel.ResetStatusOptions(obj);
        }

        private void InvitationsPanel_InvitationSelected(ChatRoomInvitation? obj)
        {
            throw new System.NotImplementedException();
        }

        #endregion

        #region Apoyo a eventos de recepción de mensajes

        // Apoyo a los eventos del receptor de mensajes:
        private void Receiver_MsgReceived(MsgData msgData)
        {
            ArgumentNullException.ThrowIfNull(msgData);

            switch (msgData.Type)
            {
                case "NEW_USER":
                    HandleTypeNewUser(msgData.Username!);
                    break;
                case "NEW_STATUS":
                    HandleTypeNewStatus(msgData.Username!, msgData.Status!.Value);
                    break;
                case "USER_LIST":
                    HandleTypeUserList(msgData.Users!);
                    break;
                case "TEXT_FROM":
                    HandleTypeTextFrom(msgData.Username!, msgData.Text!);
                    break;
                case "PUBLIC_TEXT_FROM":
                    HandleTypePublicTextFrom(msgData.Username!, msgData.Text!);
                    break;
            }
        }

        /// <summary>
        /// Maneja al recepción del mensaje de tipo "NEW_USER"
        /// </summary>
        /// <param name="username"></param>
        private void HandleTypeNewUser(string username)
        {
            // Lo registramos en la sala principal
            ChatUserViewModel user = new(username);
            MainRoom.AddMember(user);
            ChatMsg msg = new(
                sender: MainRoom.Roomname,
                text: $"{username} se ha unido al chat, dale la bienvenida.");
            MainRoom.AddMsg(msg);

            // Actualizamos el panel de usuarios y el del chat
            if (_currentRoom == MainRoom)
            {
                UsersPanel.AddUser(user);
                ChatPanel.AddMsg(msg);
            }
        }

        /// <summary>
        /// Maneja la recepción del mensaje de tipo "NEW_STATUS"
        /// </summary>
        /// <param name="username">Nombre de usuario de quien cambió su
        /// estado.</param>
        /// <param name="status">Estado al que cambió.</param>
        private void HandleTypeNewStatus(string username, UserStatus status)
        {
            ChatUserViewModel? user = MainRoom.GetUserOrNull(username);
            if (user == null || status == user.Status)
                return;
            user.Status = status;
        }

        /// <summary>
        /// Maneja la recepción del mensaje de tipo "USER_LIST"
        /// </summary>
        /// <param name="users"></param>
        private void HandleTypeUserList(IReadOnlyDictionary<string, UserStatus> users)
        {
            if (MainRoom.Room.Members.Count > 1)
                return;
            // Llenamos la lista de usuarios de la sala principal
            foreach (var (username, status) in users)
            {
                ChatUserViewModel user = new(username);
                if (status != UserStatus.Active)
                    user.Status = status;
                MainRoom.AddMember(user);
            }

            IEnumerable<ChatUserViewModel> usrs = MainRoom.Users.Values.Where(
                (usr) => usr.Username != Connection.User!.Username);
            // Llenamos el panel de usuarios
            UsersPanel.SetUsers(usrs);

            // Actualizamos el panel del chat
            ChatPanel.HeaderText = MainRoom.Roomname;

            _currentChatUser = null;
            _currentRoom = MainRoom;
        }

        /// <summary>
        /// Maneja la recepción del mensaje de tipo "TEXT_FROM"
        /// </summary>
        /// <param name="username">Nombre del usuario que envía el texto.</param>
        /// <param name="text">El texto.</param>
        private void HandleTypeTextFrom(string username, string text)
        {
            ChatUserViewModel? user = MainRoom.GetUserOrNull(username);
            if (user == null)
                return;
            ChatMsg msg = new(
                sender: username,
                text: text);

            user!.AddMsg(msg);

            if (_currentChatUser == user)
            {
                ChatPanel.AddMsg(msg);
            }
        }

        /// <summary>
        /// Maneja la recepción del mensaje de tipo "PUBLIC_TEXT_FROM"
        /// </summary>
        /// <param name="username">Nombre del usuario que envía el texto.</param>
        /// <param name="text">El texto.</param>
        private void HandleTypePublicTextFrom(string username, string text)
        {
            ChatUserViewModel? user = MainRoom.GetUserOrNull(username);
            if (user == null)
                return;
            
            ChatMsg msg = new(
                sender: username,
                text: text);

            MainRoom.AddMsg(msg);

            if (_currentRoom == MainRoom)
            {
                ChatPanel.AddMsg(msg);
            }

        }

        #endregion

    }
}
