using Client.Models.Connection;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Diagnostics;
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

        /// <summary>
        /// IP para establecer la conexión.
        /// </summary>
        [ObservableProperty]
        private string _ip = string.Empty;

        /// <summary>
        /// Número de puerto para establecer la conexión.
        /// </summary>
        [ObservableProperty]
        private string _port = string.Empty;

        /// <summary>
        /// Mensaje de error.
        /// </summary>
        [ObservableProperty]
        private string _errorMsg = string.Empty;

        /// <summary>
        /// Indica si se debe o no mostrar un mensaje de error.
        /// </summary>
        [ObservableProperty]
        private bool _showErrorMsg = false;

        #endregion

        #region Comandos

        /// <summary>
        /// Establece la conexión con el cliente y notifica
        /// mensajes de error.
        /// </summary>
        /// <returns></returns>
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

            bool established = await ClientConnection.Instance.ConnectAsync(Ip, port);
            if (!established)
            {
                ErrorMsg = $"No se pudo establecer la conexión con IP:{Ip}, Puerto:{Port}";
                ShowErrorMsg = true;
                return;
            }
            ConnectionEstablished?.Invoke();
        }

        #endregion

        #region Apoyo

        /// <summary>
        /// Valida que la IP tenga un formato adecuado.
        /// </summary>
        /// <returns><see langword="true"/> si la IP fue válida y <see langword="false"/>
        /// de lo contrario.</returns>
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

        /// <summary>
        /// Valida que el puerto tenga el formato y alcance adecuado.
        /// </summary>
        /// <param name="port">Número de puerto, parseado a entero.</param>
        /// <returns><see langword="true"/> si el puerto fue válido y <see langword="false"/>
        /// de lo contrario.</returns>
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
