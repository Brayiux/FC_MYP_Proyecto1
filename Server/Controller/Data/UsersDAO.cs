using Controller.Resources;
using Model.Definitions.Enums;

namespace Controller.Data
{
    /// <summary>
    /// DAO para acceder y manipular los usuarios del chat.
    /// </summary>
    public class UsersDAO
    {
        public void Identify(ClientConnection c)
        {
            throw new NotImplementedException();
        }

        public void Remove(Guid id)
        {
            throw new NotImplementedException();
        }

        public bool Exists(string username)
        {
            throw new NotImplementedException();
        }

        public List<ClientConnection> GetAll()
        {
            throw new NotImplementedException();
        }

        public Dictionary<ClientConnection, ClientStatus> GetAllAndStatus()
        {
            throw new NotImplementedException();
        }

        public ClientConnection GetClientOrNull(string username)
        {
            throw new NotImplementedException();
        }

    }
}
