using Client.Models.Definitions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Client.ViewModels.Chat.ViewModels
{
    /// <summary>
    /// Representa una opción elegible para modificar el estado
    /// del usuario.
    /// </summary>
    public partial class StatusOptionViewModel : ViewModelBase
    {
        #region Propiedades

        /// <summary>
        /// Obtiene el nombre del estado.
        /// </summary>
        public string StatusName
        {
            get
            {
                switch (Status)
                {
                    case UserStatus.Active:
                        return "Active";
                    case UserStatus.Bussy:
                        return "Bussy";
                    case UserStatus.Away:
                        return "Away";
                }
                return string.Empty;
            }
        }

        /// <summary>
        /// Obtiene el estado de la opción.
        /// </summary>
        public UserStatus Status { get; set; }

        /// <summary>
        /// Indica si esta opción está habilitada.
        /// </summary>
        public bool IsEnabled { get; set; } = true;

        #endregion
    }
}
