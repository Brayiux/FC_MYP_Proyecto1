using Client.Models.Definitions;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Client.ViewModels.Chat.ViewModels
{
    /// <summary>
    /// Representa el panel
    /// </summary>
    public partial class ChangeStatusPanelViewModel : ViewModelBase
    {
        #region Contexto

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
        public void Reset(UserStatus currentStatus)
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
    }
}
