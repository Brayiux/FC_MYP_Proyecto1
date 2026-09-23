using Client.Models.Connection;
using Client.Models.Resources;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Client.ViewModels.Login
{
    /// <summary>
    /// Se encarga de conectar el modelo con la vista, sólo
    /// en la parte de identificación.
    /// </summary>
    public partial class LoginViewModel : ViewModelBase
    {
        #region Eventos
        public event Action? LoginSuccess;
        public event Action? LoginFailed;
        public event Action? ConnectionInterrupted;
        #endregion


        #region Propiedades observables

        /// <summary>
        /// Mensaje de error.
        /// </summary>
        [ObservableProperty]
        private string _errorMsg = string.Empty;

        /// <summary>
        /// Indica si se debe mostrar un mensaje de error.
        /// </summary>
        [ObservableProperty]
        private bool _showErrorMsg = false;

        /// <summary>
        /// Nombre con el que el usuario se identifica.
        /// </summary>
        [ObservableProperty]
        private string _username = string.Empty;

        #endregion

        #region Comandos

        /// <summary>
        /// Realiza el flujo de inicio de sesión / identificación
        /// del usuario a través del modelo.
        /// </summary>
        /// <returns></returns>
        [RelayCommand]
        private async Task Login()
        {
            Debugger.Launch();
            if (!ValidateUsername())
            {
                ShowErrorMsg = true;
                return;
            }

            
            MsgReceiver.Instance.MsgReceived += MsgReceiver_MsgReceived;
            await MsgSender.Instance.IdentifyAsync(Username);

        }

        #endregion

        #region Apoyo

        /// <summary>
        /// Valida que el nombre de usuario tenga el formato adecuado.
        /// </summary>
        /// <returns></returns>
        private bool ValidateUsername()
        {
            if (string.IsNullOrEmpty(Username))
            {
                ErrorMsg = "Error: El nombre de usuario no puede estar vacío.";
                return false;
            }
            if (Username.Length > 8)
            {
                ErrorMsg = "Error: El nombre de usuario no puede exceder de 8 caracteres";
                return false;
            }
            return true;
        }

        /// <summary>
        /// Maneja el evento de la recepción de mensajes.
        /// </summary>
        /// <param name="msg"></param>
        private void MsgReceiver_MsgReceived(MsgData msg)
        {
            if (msg.Operation == null)
                return;
            switch (msg.Operation)
            {
                case "IDENTIFY":
                    if (msg.Result == "SUCCESS")
                    {
                        LoginSuccess?.Invoke();
                    }
                    else
                    {
                        ErrorMsg = "El nombre de usuario ya se encuentra en uso";
                        ShowErrorMsg = true;
                        LoginFailed?.Invoke();
                    }
                    
                    MsgReceiver.Instance.MsgReceived -= MsgReceiver_MsgReceived;
                    break;
                case "INVALID":
                    ErrorMsg = "Error: Conexión interrumpida";
                    ShowErrorMsg = true;
                    ConnectionInterrupted?.Invoke();
                    break;
            }

            
            
            
        }

        #endregion

    }
}
