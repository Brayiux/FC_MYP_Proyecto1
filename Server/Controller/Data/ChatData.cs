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
            return _clients.Remove(id, out _);
        }
        public bool ExistsClient(Guid id)
        {
            return _clients.ContainsKey(id);
        }
        public ClientConnection? GetClientOrNull(Guid id)
        {
            _clients.TryGetValue(id, out ClientConnection? c);
            return c;
        }
        public void ClearClients()
        {
            _clients.Clear();
        }
        #endregion

        #region Operaciones para usuarios

        public bool AddUser(ClientConnection c)
        {
            return _users.TryAdd(c.User.Username, c);
        }
        public bool RemoveUser(string username)
        {
            return _users.Remove(username, out _);
        }
        public bool ExistsUser(string username)
        {
            return _users.ContainsKey(username);
        }
        public ClientConnection? GetUserOrNull(string username)
        {
            _users.TryGetValue(username, out ClientConnection? c);
            return c;
        }
        public void ClearUsers()
        {
            _users.Clear();
        }

        #endregion


    }
}
