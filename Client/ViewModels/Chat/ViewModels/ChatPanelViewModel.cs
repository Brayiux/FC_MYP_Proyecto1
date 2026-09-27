using Client.Models.Connection;
using Client.Models.Entities;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Client.ViewModels.Chat.ViewModels
{
    public partial class ChatPanelViewModel : ViewModelBase
    {
        #region Eventos

        public event Action<string>? MessageSent;


        #endregion

        #region Contexto

        /// <summary>
        /// Los mensajes del chat.
        /// </summary>
        [ObservableProperty]
        private ObservableCollection<ChatMsg> _messages = [];

        /// <summary>
        /// El encabezado del chat
        /// </summary>
        [ObservableProperty]
        private string _headerText = string.Empty;

        /// <summary>
        /// Contenido del mensaje que el cliente escribe para el chat.
        /// </summary>
        [ObservableProperty]
        private string _userMsg = string.Empty;

        #endregion

        #region Acceso público

        /// <summary>
        /// Modifica la lista de mensajes del panel del chat.
        /// </summary>
        /// <param name="msgs"></param>
        public void SetMsgs(IEnumerable<ChatMsg> msgs)
        {
            ArgumentNullException.ThrowIfNull(msgs);
            Messages = new(msgs);
        }

        /// <summary>
        /// Añade un mensaje a la lista de mensajes del panel.
        /// </summary>
        /// <param name="msg"></param>
        public void AddMsg(ChatMsg msg)
        {
            ArgumentNullException.ThrowIfNull(msg);
            Messages.Add(msg);
        }

        /// <summary>
        /// Elimina todos los mensajes del panel.
        /// </summary>
        public void ClearMsgs()
        {
            Messages.Clear();
        }

        #endregion

        #region Comandos

        /// <summary>
        /// Envía el mensaje a la caja y notifica que se ha enviado.
        /// </summary>
        [RelayCommand]
        public void SendMsg()
        {
            if (string.IsNullOrEmpty(UserMsg))
                return;


            AddMsg(new(
                sender: ClientConnection.Instance.User!.Username,
                text: UserMsg));
            MessageSent?.Invoke(UserMsg);
        }

        #endregion
    }
}
