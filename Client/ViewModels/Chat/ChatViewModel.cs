using Client.Models.Connection;
using Client.Models.Definitions;
using Client.Models.Entities;
using Client.Models.Resources;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SkiaSharp;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace Client.ViewModels.Chat
{
    public partial class ChatViewModel : ViewModelBase
    {
        #region Campos

        private readonly Dictionary<string, ChatUser> _usersRegistry = [];

        private readonly Dictionary<string, ChatRoom> _roomsRegistry = [];

        #endregion

        #region Propiedades observables

        [ObservableProperty]
        private ObservableCollection<ChatRoom> _rooms = [];

        [ObservableProperty]
        private ObservableCollection<ChatUser> _users = [];

        [ObservableProperty]
        private ObservableCollection<ChatMsg> _chatMsgs = [];

        [ObservableProperty]
        private ChatRoom? _selectedRoom;

        [ObservableProperty]
        private ChatUser? _selectedUser;

        [ObservableProperty]
        private string _textInstructions = string.Empty;

        [ObservableProperty]
        private string _userText = string.Empty;

        #endregion

        #region Propiedades

        private MsgSender Sender => MsgSender.Instance;

        private MsgReceiver Receiver => MsgReceiver.Instance;

        #endregion

        #region Construcción

        public ChatViewModel() : base()
        {
            _ = Initialize();
        }

        #endregion

        #region Comandos

        [RelayCommand]
        private void CreateRoom()
        {

        }

        [RelayCommand]
        private void InviteUser()
        {
            
        }

        [RelayCommand]
        private void SendText()
        {

        }

        [RelayCommand]
        private void Disconnect()
        {

        }

        [RelayCommand]
        private async Task UpdateUsersList()
        {
            
        }

        #endregion

        #region Detección

        partial void OnSelectedRoomChanged(ChatRoom? value)
        {
            throw new System.NotImplementedException();
        }

        partial void OnSelectedUserChanged(ChatUser? value)
        {
            throw new System.NotImplementedException();
        }

        #endregion

        #region Apoyo

        private async Task Initialize()
        {
            Receiver.MsgReceived += Receiver_MsgReceived;
            // Registrar al cliente
            ChatUser currentUser = ClientConnection.Instance.User!;
            _usersRegistry[currentUser.Username] = currentUser;

            // Registrar la sala principal
            ChatRoom mainRoom = MainChatRoom.Instance;
            mainRoom.AddGuest(currentUser);
            mainRoom.AddMember(currentUser.Username);
            RegisterRoom(mainRoom);

            _ = Sender.RequestUsersInChatAsync();
        }

        private void Receiver_MsgReceived(MsgData msgData)
        {
            switch (msgData.Operation)
            {
                case "NEW_USER":
                    HandleNewUser(msgData.Username!);
                    break;
                case "NEW_STATUS":
                    HandleNewStatus(msgData.Username!, msgData.Status!.Value);
                    break;
                case "USER_LIST":
                    HandleUserList(msgData.Users!);
                    break;
                case "TEXT_FROM":
                    HandleTextFrom(msgData.Username!, msgData.Text!);
                    break;
                case "PUBLIC_TEXT_FROM":
                    HandlePublicTextFrom(msgData.Username!, msgData.Text!);
                    break;
                case "JOINED_ROOM":
                    HandleJoinedRoom(msgData.Roomname!, msgData.Username!);
                    break;
                case "ROOM_USER_LIST":
                    break;
                case "ROOM_TEXT_FROM":
                    HandleRoomTextFrom(msgData.Roomname!, msgData.Username!, msgData.Text!);
                    break;
                case "LEFT_ROOM":
                    HandleLeftRoom(msgData.Roomname!, msgData.Username!);
                    break;
                case "DISCONNECTED":
                    RemoveUser(msgData.Username!);
                    break;

            }
        }

        #endregion


        #region Apoyo

        private void HandleNewUser(string username)
        {
            ChatUser user = new(username)
            {
                Status = UserStatus.Active
            };
            RegisterUser(user);
        }

        private void HandleNewStatus(string username, UserStatus status)
        {
            _usersRegistry[username].Status = status;
        }

        private void HandleUserList(IReadOnlyDictionary<string, UserStatus> users)
        {
            if (Users.Count <= 1)
            {
                foreach (var (username, status) in users)
                {
                    if (username.Equals(ClientConnection.Instance.User!.Username))
                        continue;
                    ChatUser user = new(username)
                    {
                        Status = status
                    };
                    RegisterUser(user);
                }
            }
        }

        private void HandleTextFrom(string username, string text)
        {
            ChatUser sender = _usersRegistry[username];
            sender.RegisterMsg(new(sender: sender.Username, text: text));
        }

        private void HandlePublicTextFrom(string username, string text)
        {
            MainChatRoom.Instance.RegisterMsg(new(
                sender: username,
                text: text));
        }

        private void HandleJoinedRoom(string roomname, string username)
        {
            if (_roomsRegistry.TryGetValue(roomname, out ChatRoom? room))
            {
                room.RegisterMsg(new(sender: roomname, text: $"Se ha unido {username}."));
            }
        }

        private void HandleRoomTextFrom(string roomname, string username, string text)
        {
            if (_roomsRegistry.TryGetValue(roomname, out ChatRoom? room))
            {
                room.RegisterMsg(new(sender: username, text: text));
            }
        }

        private void HandleLeftRoom(string roomname, string username)
        {
            if (_roomsRegistry.TryGetValue(roomname, out ChatRoom? room))
            {
                room.RemoveMember(username);
            }
        }
        
        // Apoyo General:

        private void RegisterUser(ChatUser user)
        {
            _usersRegistry[user.Username] = user;
            Users.Add(user);
        }

        private void RegisterRoom(ChatRoom room)
        {
            _roomsRegistry[room.Roomname] = room;
            Rooms.Add(room);
        }

        private void RemoveUser(string username)
        {
            _usersRegistry.Remove(username, out ChatUser? user);
            if (user != null)
                Users.Remove(user);
        }

        #endregion
    }
}
