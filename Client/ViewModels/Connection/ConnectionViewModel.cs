using Client.Models.Connection;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Client.ViewModels.Connection
{
    public partial class ConnectionViewModel : ViewModelBase
    {
        #region Eventos

        public Action? ConnectionEstablished;

        #endregion


        #region Propiedades observables

        [ObservableProperty]
        private string _ip = string.Empty;

        [ObservableProperty]
        private string _port = string.Empty;

        [ObservableProperty]
        private string _errorMsg = string.Empty;

        [ObservableProperty]
        private bool _showErrorMsg = false;

        #endregion

        #region Comandos

        [RelayCommand]
        private async Task Connect()
        {
            if (!ValidateIp())
            {
                ShowErrorMsg = true;
                return;
            }
            if (!ValidatePort(out int port))
            {
                ShowErrorMsg = true;
                return;
            }

            ShowErrorMsg = false;

            try
            {
                await ClientConnection.Instance.ConnectAsync(Ip, port);
                ConnectionEstablished?.Invoke();
            }
            catch (SocketException)
            {
                ErrorMsg = $"No se pudo establecer la conexión con IP:{Ip}, Puerto:{Port}";
                ShowErrorMsg = true;
            }
        }

        #endregion

        #region Apoyo

        private bool ValidateIp()
        {
            if (string.IsNullOrEmpty(Ip))
            {
                Console.WriteLine($"La Ip es:{Ip}");
                ErrorMsg = "Error: La IP no puede estar vacía.";
                return false;
            }

            if (!IPAddress.TryParse(Ip, out _))
            {
                ErrorMsg = "Error: La IP no tiene el formato adecuado.";
                return false;
            }
            ErrorMsg = string.Empty;
            return true;
        }

        private bool ValidatePort(out int port)
        {
            port = -1;
            if (!int.TryParse(Port, out int p))
            {
                ErrorMsg = "Error: El puerto no cuenta con el formato válido.";
                return false;
            }

            if (p is < 1 or > 65535)
            {
                ErrorMsg = "Error: Sólo puede ingresar un puerto entre 1 y 65535.";
                return false;
            }

            port = p;
            ErrorMsg = string.Empty;
            return true;
        }

        #endregion

    }
}
