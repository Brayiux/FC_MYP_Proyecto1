using Controller.Resources;
using Model.Definitions.Enums;
using Model.Exceptions;

namespace Controller.Data
{
    /// <summary>
    /// DAO para acceder y manipular los usuarios del chat.
    /// </summary>
    public class UsersDAO
    {
        private static UsersDAO _instance;
        public static UsersDAO Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new UsersDAO();
                return _instance;
            }
        }
        private UsersDAO() { }
        public void Identify(ClientConnection c)
        {
            if (!ChatData.Instance.AddUser(c))
            {
                throw new UserAlreadyExistsException(
                    "Error: El usuario ya existe en la base de datos");
            }
        }

        public void Remove(Guid id)
        {
            if (!ChatData.Instance.RemoveClient(id))
            {
                throw new UserNotFoundException(
                    "Error: No se puede remover al cliente porque no existe");
            }
        }

        public bool Exists(string username)
        {
            return ChatData.Instance.Users.ContainsKey(username);
        }

        public IReadOnlyList<ClientConnection> GetAll()
        {
#warning TODO: mejorar para no tener que repoblar la lista.
            return [.. ChatData.Instance.Clients.Values];
        }

        public Dictionary<ClientConnection, UserStatus> GetAllAndStatus()
        {
            throw new NotImplementedException();
        }

        public ClientConnection? GetUserOrNull(string username)
        {
            return ChatData.Instance.GetUserOrNull(username);
        }

    }
}
