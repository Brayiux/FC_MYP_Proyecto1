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
            ChatData.Instance.AddClient(c);
        }

        public void Disconnect(Guid idClient)
        {
            ChatData.Instance.RemoveClient(idClient);
        }

        public IReadOnlyList<ClientConnection> GetAll()
        {
#warning TODO: Mejorar para devolver la lista de valores sin repoblar
            return [.. ChatData.Instance.Clients.Values];
        }

        public void Clear()
        {
            ChatData.Instance.ClearClients();
        }
    }
}
