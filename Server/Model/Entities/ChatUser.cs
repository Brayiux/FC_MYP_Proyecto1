using Model.Definitions.Enums;

namespace Model.Entities;
/// <summary>
/// Contiene la información de un usuario conectado al chat.
/// </summary>
public class ChatUser
{
    #region Campos

    private ClientStatus _status;

    private string _username;

    #endregion

    #region Propiedades

    public Guid Id { get; }

    #endregion

    #region Construcción
    public ChatUser(string username)
    {
        throw new NotImplementedException();
    }
    #endregion

    #region Acceso Público

    public void SetUsername(string username)
    {
        throw new NotImplementedException();
    }

    public void SetStatus(ClientStatus st)
    {
        throw new NotImplementedException();
    }

    #endregion

}
