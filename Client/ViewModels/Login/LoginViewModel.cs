using Client.Models.Connection;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Threading.Tasks;

namespace Client.ViewModels.Login
{
    public partial class LoginViewModel : ViewModelBase
    {

        #region Propiedades observables

        [ObservableProperty]
        private string _errorMsg = string.Empty;

        [ObservableProperty]
        private bool _showErrorMsg = false;

        [ObservableProperty]
        private string _username = string.Empty;

        #endregion

        #region Comandos

        [RelayCommand]
        private async Task Login()
        {
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

        private void MsgReceiver_MsgReceived(string msg)
        {
            
        }

        #endregion

    }
}
