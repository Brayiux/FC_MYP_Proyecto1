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
            throw new NotImplementedException();
        }

        public bool Exists(string username)
        {
            return ChatData.Instance.Users.ContainsKey(username);
        }

        public IReadOnlyList<ClientConnection> GetAll()
        {
            throw new NotImplementedException();
        }

        public Dictionary<ClientConnection, UserStatus> GetAllAndStatus()
        {
            throw new NotImplementedException();
        }

        public ClientConnection GetClientOrNull(string username)
        {
            throw new NotImplementedException();
        }

    }
}
