using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace Client.ViewModels.Chat.ViewModels
{
    /// <summary>
    /// Vista-modelo correspondiente al panel de las funciones
    /// principales del chat.
    /// </summary>
    public partial class MainPanelViewModel : ViewModelBase
    {
        #region Eventos
        /// <summary>
        /// Ocurre cuando el usuario intenta desconectare
        /// </summary>
        public event Action? TryDisconnect;

        /// <summary>
        /// Ocurre cuando el usuario intenta cambiar de estado.
        /// </summary>
        public event Action? TryChangeStatus;

        /// <summary>
        /// Ocurre cuando el usuario intenta invitar a otro a una sala.
        /// </summary>
        public event Action? TryInvite;

        #endregion

        #region Contexto

        /// <summary>
        /// El encabezado que se muestra en el panel.
        /// </summary>
        [ObservableProperty]
        private string _headerText = string.Empty;

        #endregion

        #region Comandos
        /// <summary>
        /// Notifica el intento de desconexión del cliente.
        /// </summary>
        [RelayCommand]
        private void NotifyDisconnect()
        {
            TryDisconnect?.Invoke();
        }

        /// <summary>
        /// Notifica el intento del cliente de cambiar su estado actual.
        /// </summary>
        [RelayCommand]
        private void NotifyChangeStatus()
        {
            TryChangeStatus?.Invoke();
        }

        /// <summary>
        /// Notifica el intento del cliente de invitar a un usuario a una sala.
        /// </summary>
        [RelayCommand]
        private void NotifyInvite()
        {
            TryInvite?.Invoke();
        }

        #endregion
    }
}
