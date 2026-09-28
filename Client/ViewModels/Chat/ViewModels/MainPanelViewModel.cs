using Client.Models.Definitions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        public event Action<UserStatus>? TryChangeStatus;

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

        /// <summary>
        /// Contiene los estados a los que puede cambiar el usuario.
        /// </summary>
        [ObservableProperty]
        private ObservableCollection<StatusOptionViewModel> _statusOptions = [];

        /// <summary>
        /// Estado seleccionado por el usuario.
        /// </summary>
        [ObservableProperty]
        private StatusOptionViewModel? _selectedOption;

        #endregion

        #region Acceso público
        /// <summary>
        /// Resetea la lista de opciones que están habilitadas
        /// con base al estado actual del usuario.
        /// </summary>
        /// <param name="currentStatus">Estado actual del usuario.</param>
        public void ResetStatusOptions(UserStatus currentStatus)
        {
            StatusOptions.Clear();

            foreach (UserStatus status in Enum.GetValues<UserStatus>())
            {
                StatusOptionViewModel option = new()
                {
                    Status = status,
                    IsEnabled = status != currentStatus
                };
                StatusOptions.Add(option);
            }
        }
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
            if (SelectedOption != null)
                TryChangeStatus?.Invoke(SelectedOption.Status);
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
