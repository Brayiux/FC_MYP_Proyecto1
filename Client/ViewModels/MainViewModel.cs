using Client.Models.Connection;
using Client.ViewModels.Chat;
using Client.ViewModels.Connection;
using Client.ViewModels.Login;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Client.ViewModels;

/// <summary>
/// La conexión entre la vista y el modelo mediante el patrón MVVM,
/// para la pantalla principal.
/// </summary>
/// <remarks>
/// Se compone de tres vistas diferentes e independientes, para:
/// establecer la conexión con el servidor, realizar la identificación
/// del usuario y mostrar el Chat.
/// </remarks>
public partial class MainViewModel : ViewModelBase
{
    #region Manejo de vistas

    /// <summary>
    /// Indica cuando la vista para la conexión con el servidor
    /// es visible.
    /// </summary>
    [ObservableProperty]
    private bool _isConnectionVisible = true;

    /// <summary>
    /// Indica cuando la vista para la identificación del usuario
    /// es visible.
    /// </summary>
    [ObservableProperty]
    private bool _isLoginVisible = false;

    /// <summary>
    /// Indica cuando la vista del chat (la función principal de
    /// la aplicación) es visible.
    /// </summary>
    [ObservableProperty]
    private bool _isChatVisible = false;

    #endregion

    #region Componentes de Vista Modelo

    /// <summary>
    /// Componente para comunicar la vista y modelo de la
    /// conexión con el servidor
    /// </summary>
    [ObservableProperty]
    private ConnectionViewModel _connectionVM = new();

    /// <summary>
    /// Componente para comunicar la vista y modelo de la
    /// identificación del usuario.
    /// </summary>
    [ObservableProperty]
    private LoginViewModel _loginVM = new();

    /// <summary>
    /// Componente para comunicar la vista y modelo del chat.
    /// </summary>
    [ObservableProperty]
    private ChatViewModel _chatVM = new();

    #endregion

    #region Construcción

    public MainViewModel()
    {
        _connectionVM.ConnectionEstablished += ConnectionVM_ConnectionEstablished;
        _loginVM.LoginSuccess += LoginVM_LoginSuccess;
        _loginVM.ConnectionInterrupted += LoginVM_ConnectionInterrupted;
    }

    



    #endregion

    #region Apoyo

    private void ConnectionVM_ConnectionEstablished()
    {
        IsConnectionVisible = false;
        _ = MsgReceiver.Instance.StartReceivingMsgs();
        IsLoginVisible = true;
    }

    private void LoginVM_LoginSuccess()
    {
        IsLoginVisible = false;
        IsChatVisible = true;
    }

    private void LoginVM_ConnectionInterrupted()
    {
        IsLoginVisible = false;
        IsConnectionVisible = true;
    }

    #endregion

}
