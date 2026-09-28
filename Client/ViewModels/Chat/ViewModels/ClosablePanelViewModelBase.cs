using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;

namespace Client.ViewModels.Chat.ViewModels
{
    public partial class ClosablePanelViewModelBase : ViewModelBase
    {
        #region Eventos

        /// <summary>
        /// Ocurre cuando el usuario cierra el panel.
        /// </summary>
        public event Action? PanelClosed;

        #endregion

        #region Contexto
        /// <summary>
        /// Indica si el panel está cerrado.
        /// </summary>
        [ObservableProperty]
        private bool _isPanelClosed = true;

        #endregion

        #region Comandos y Acceso público

        [RelayCommand]
        /// <summary>
        /// Cierra el panel.
        /// </summary>
        public void Close()
        {
            IsPanelClosed = true;
            PanelClosed?.Invoke();
        }

        public void Open()
        {
            IsPanelClosed = false;
        }

        #endregion

    }
}
