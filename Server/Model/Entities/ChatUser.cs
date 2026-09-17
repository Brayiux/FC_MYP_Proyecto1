using Model.Definitions.Enums;
using Model.Exceptions;

namespace Model.Entities;
/// <summary>
/// Contiene la información de un usuario conectado al chat.
/// </summary>
public class ChatUser
{
    #region Campos

    /// <summary>
    /// El estado del usuario en el chat.
    /// </summary>
    private UserStatus _status;

    /// <summary>
    /// El nombre de usuario del usuario en el chat.
    /// </summary>
    private string _username;

    #endregion

    #region Propiedades

    /// <summary>
    /// Obtiene y valida la modificación del estado actual
    /// del usuario en el chat.
    /// </summary>
    /// <exception cref="InvalidOperationException">Si se modifica
    /// al estado actual.</exception>
    public UserStatus Status
    {
        get => _status;
        set
        {
            ValidateNewStatus(value);
            _status = value;
        }
    }

    /// <summary>
    /// Obtiene y valida la modificación del nombre del usuario en el chat.
    /// </summary>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="ArgumentOutOfRangeException">Si el nuevo valor
    /// excede el rango de 1 a 8 caracteres.</exception>
    public string Username
    {
        get => _username;
        set
        {
            ValidateUsername(value);
            _username = value;
        }
    }

    #endregion

    #region Construcción

    /// <summary>
    /// Crea un usuario del chat con el nombre de usuario
    /// especificado y lo inicia en estado activo.
    /// </summary>
    /// <param name="username">Nombre del usuario.</param>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="ArgumentOutOfRangeException">Si <paramref name="username"/>
    /// excede el rango de 1 a 8 caracteres.</exception>
    public ChatUser(string username)
    {
        ValidateUsername(username);
        _username = username;
        _status = UserStatus.Active;
    }
    #endregion

    #region Apoyo

    /// <summary>
    /// Valida que el nombre de usuario no sea nulo ni salga de un rango
    /// de 1 a 8 caracteres.
    /// </summary>
    /// <param name="username">Nombre del usuario.</param>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="UsernameOutOfRangeException">Si <paramref name="username"/>
    /// sale del rango de 1 a 8 caracteres.</exception>
    private void ValidateUsername(string username)
    {
        ArgumentNullException.ThrowIfNull(username, nameof(username));

        if (username.Length is 0 or > 8)
        {
            throw new UsernameOutOfRangeException(
                $"Error: El username {username} sale del rango de 1 a 8 caracteres.");
        }
    }

    /// <summary>
    /// Valida que el estado nuevo del usuario sea diferente al actual.
    /// </summary>
    /// <param name="newStatus">Estado nuevo para el usuario.</param>
    /// <exception cref="InvalidClientStatusException">Si <paramref name="newStatus"/>
    /// coincide con el estado actual.</exception>
    private void ValidateNewStatus(UserStatus newStatus)
    {
        if (newStatus == _status)
        {
            throw new InvalidClientStatusException(
                "Error: No se puede modificar el estado de un usuario al mismo estado.");
        }
    }

    #endregion

}
