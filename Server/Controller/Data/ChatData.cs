using Controller.Resources;
using Model.Entities;
using System.Collections.Concurrent;

namespace Controller.Data
{
    /// <summary>
    /// Almacena los datos de clientes, usuarios, salas grupales y salas privadas
    /// dentro del servidor, de manera segura ante la manipulación y lectura
    /// concurrente.
    /// </summary>
    public class ChatData
    {
        #region Campos

        private static ChatData _instance;

        private readonly ConcurrentDictionary<Guid, ClientConnection> _clients = [];

        private readonly ConcurrentDictionary<string, ClientConnection> _users = [];

        private readonly ConcurrentDictionary<string, ChatRoom> _rooms = [];

        private readonly ConcurrentDictionary<string, PrivateChatRoom> _privateRooms = [];

        #endregion

        #region Propiedades

        public static ChatData Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new ChatData();
                return _instance;
            }
        }

        public IReadOnlyDictionary<Guid, ClientConnection> Clients => _clients;
        public IReadOnlyDictionary<string, ClientConnection> Users => _users;
        public IReadOnlyDictionary<string, ChatRoom> Rooms => _rooms;
        public IReadOnlyDictionary<string, PrivateChatRoom> PrivateRooms => _privateRooms;

        #endregion

        #region Construcción
        private ChatData() { }
        #endregion

        #region Operaciones para clientes
        public bool AddClient(ClientConnection c)
        {
            if (_clients.ContainsKey(c.Id)) return false;

            _clients[c.Id] = c;
            return true;
        }
        public bool RemoveClient(Guid id)
        {
            throw new NotImplementedException();
        }
        public bool ExistsClient(Guid id)
        {
            throw new NotImplementedException();
        }
        public ClientConnection? GetClientOrNull(Guid id)
        {
            throw new NotImplementedException();
        }
        public void ClearClients()
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Operaciones para usuarios

        public bool AddUser(ClientConnection c)
        {
            return _users.TryAdd(c.User.Username, c);
        }
        public bool RemoveUser(Guid id)
        {
            throw new NotImplementedException();
        }
        public bool ExistsUser(Guid id)
        {
            throw new NotImplementedException();
        }
        public ClientConnection? GetUserOrNull(Guid id)
        {
            throw new NotImplementedException();
        }
        public void ClearUsers()
        {
            throw new NotImplementedException();
        }

        #endregion



    }
}
