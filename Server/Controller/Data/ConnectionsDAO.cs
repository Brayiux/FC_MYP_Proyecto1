using Controller.Resources;

namespace Controller.Data
{
    /// <summary>
    /// DAO para acceder y manipular los clientes conectados en el servidor.
    /// </summary>
    public class ConnectionsDAO
    {
        private static ConnectionsDAO _instance;

        public static ConnectionsDAO Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new ConnectionsDAO();
                return _instance;
            }
        }

        private ConnectionsDAO() { }

        public void Connect(ClientConnection c)
        {
            throw new NotImplementedException();
        }

        public void Disconnect(Guid idClient)
        {
            throw new NotImplementedException();
        }
    }
}
