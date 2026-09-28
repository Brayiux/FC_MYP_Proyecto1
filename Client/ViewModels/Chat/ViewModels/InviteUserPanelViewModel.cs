using Client.Models.Connection;
using Client.Models.Resources;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.ViewModels.Chat.ViewModels
{
    /// <summary>
    /// Vista-modelo del panel usado para invitar usuarios
    /// por nombre de sala y nombre de usuario.
    /// </summary>
    public partial class InviteUserPanelViewModel : ClosablePanelViewModelBase
    {
        #region Eventos

        /// <summary>
        /// Ocurre cuando el cliente consiguió invitar a un usuario,
        /// pasando como parámetros los nombres de la sala y del usuario,
        /// respectivamente.
        /// </summary>
        public event Action<string, string>? UserInvited;

        #endregion

        #region Campos

        /// <summary>
        /// Nombres de salas válidas.
        /// </summary>
        private ConcurrentBag<string> _validRoomnames = [];

        /// <summary>
        /// Nombres de usuario válidos.
        /// </summary>
        private ConcurrentBag<string> _validUsernames = [];

        #endregion

        #region Contexto

        /// <summary>
        /// Indica si se muestra un mensaje de error.
        /// </summary>
        [ObservableProperty]
        private bool _showErrorMsg = false;

        /// <summary>
        /// Mensaje de error.
        /// </summary>
        [ObservableProperty]
        private string _errorMsg = string.Empty;

        /// <summary>
        /// Nombre de la sala a donde se le invita.
        /// </summary>
        [ObservableProperty]
        private string _roomname = string.Empty;

        /// <summary>
        /// Nombre del usuario al que se invita.
        /// </summary>
        [ObservableProperty]
        private string _guest = string.Empty;

        #endregion

        #region Acceso público

        /// <summary>
        /// Establece la lista de salas válidas (las que existen
        /// y de las cuales el cliente es miembro).
        /// </summary>
        /// <param name="vr"></param>
        public void SetValidRoomnames(IEnumerable<string> vr)
        {
            ArgumentNullException.ThrowIfNull(vr);
            _validRoomnames = [.. vr];
        }

        /// <summary>
        /// Establece los nombres de usuario válidos (los que
        /// están identificados en el chat).
        /// </summary>
        /// <param name="vu"></param>
        public void SetValidUsernames(IEnumerable<string> vu)
        {
            ArgumentNullException.ThrowIfNull(vu);
            _validUsernames = [.. vu];
        }
        #endregion

        #region Comandos

        /// <summary>
        /// Invita a un usuario a una sala.
        /// </summary>
        /// <returns></returns>
        [RelayCommand]
        private async Task InviteUser()
        {
            ErrorMsg = string.Empty;
            ShowErrorMsg = false;
            if (!_validRoomnames.Contains(Roomname))
            {
                ErrorMsg = $"Error: Usted no es miembro de {Roomname} o no existe";
                ShowErrorMsg = true;
                return;
            }
            if (!_validUsernames.Contains(Guest))
            {
                ErrorMsg = $"Error: El usuario {Guest} no existe.";
                ShowErrorMsg = true;
                return;
            }

            MsgReceiver.Instance.MsgReceived += Instance_MsgReceived;
            await MsgSender.Instance.InviteToRoomAsync(Roomname, Guest);
            MsgReceiver.Instance.MsgReceived -= Instance_MsgReceived;
            UserInvited?.Invoke(Roomname, Guest);
        }

        #endregion

        #region Apoyo
        /// <summary>
        /// Maneja las respuestas del servidor ante la operación
        /// "INVITE".
        /// </summary>
        /// <param name="msgData">Datos de la respuesta del servidor.</param>
        private void Instance_MsgReceived(MsgData msgData)
        {
            if (msgData == null ||
                msgData.Type != "RESPONSE" ||
                msgData.Operation != "INVITE")
                return;
            ErrorMsg = string.Empty;
            ShowErrorMsg = false;
            switch (msgData.Result)
            {
                case "NO_SUCH_ROOM":
                    ErrorMsg = $"Error: No existe una sala con el nombre {Roomname}.";
                    ShowErrorMsg = true;
                    break;
                case "NO_SUCH_USER":
                    ErrorMsg = $"Error: No existe un usuario con el nombre {Guest}.";
                    ShowErrorMsg = true;
                    break;
            }

        }
        #endregion

    }
}
