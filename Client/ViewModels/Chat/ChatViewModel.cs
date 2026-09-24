using Client.Models.Entities;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Client.ViewModels.Chat
{
    public partial class ChatViewModel : ViewModelBase
    {
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

    }
}
