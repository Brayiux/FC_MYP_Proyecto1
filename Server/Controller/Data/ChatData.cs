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

        #endregion

        #region Construcción
        private ChatData() { }
        #endregion

        #region Operaciones para clientes
        public bool AddClient(ClientConnection client)
        {
            if (_clients.ContainsKey(client.Id)) return false;

            _clients[client.Id] = client;
            return true;
        }
        public bool RemoveClient(Guid id)
        {
            return _clients.Remove(id, out _);
        }
        public void ClearClients()
        {
            _clients.Clear();
        }
        public IReadOnlyDictionary<Guid, ClientConnection> GetAllClients()
        {
            return _clients;
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
        public void ClearUsers()
        {
            _users.Clear();
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
        public IReadOnlyDictionary<string, ClientConnection> GetAllUsers()
        {
            return _users;
        }

        #endregion

        #region Operaciones para habitaciones

        public bool AddRoom(ChatRoom room)
        {
            if (_rooms.TryAdd(room.Roomname, room))
            {
                // Se auto-elimina al vaciarse
                room.EmptiedRoom += Room_EmptiedRoom;
                return true;
            }
            return false;
        }

        public bool RemoveRoom(string roomname)
        {
            return _rooms.Remove(roomname, out _);
        }
        public void ClearRooms()
        {
            _rooms.Clear();
        }
        public bool ExistsRoom(string roomname)
        {
            return _rooms.ContainsKey(roomname);
        }

        public ChatRoom? GetRoomOrNull(string roomname)
        {
            _rooms.TryGetValue(roomname, out ChatRoom? room);
            return room;
        }

        public IReadOnlyDictionary<string, ChatRoom> GetAllRooms()
        {
            return _rooms;
        }

        #endregion

        #region Apoyo

        /// <summary>
        /// Se ejecuta cuando una sala se vacía. La elimina del
        /// registro de salas.
        /// </summary>
        /// <param name="room"></param>
        private void Room_EmptiedRoom(ChatRoom room)
        {
            room.EmptiedRoom -= Room_EmptiedRoom;
            RemoveRoom(room.Roomname);
        }

        #endregion
    }

}
