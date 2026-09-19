using Controller.Data;
using Model.Entities;
using System.Net.Sockets;

namespace Controller.Resources
{
    /// <summary>
    /// Representa la conexión de un cliente con el servidor y
    /// provee un medio de comunicación entre sí.
    /// </summary>
    public class ClientConnection
    {

        #region Eventos

        public event Action Disconnected;

        #endregion

        #region Campos

        private readonly HashSet<ChatRoom> _rooms = [];

        private readonly HashSet<ClientConnection> _privateChats = [];

        private bool _isConnected;

        #endregion

        #region Propiedades

        public Guid Id { get; }

        public bool IsConnected => _isConnected;

        public ChatUser User { get; set; }
        
        public TcpClient Socket { get; }

        public IReadOnlySet<ChatRoom> Rooms => _rooms;

        public IReadOnlySet<ClientConnection> PrivateChats => _privateChats;

        #endregion

        #region Construcción

        public ClientConnection(TcpClient socket)
        {
            ArgumentNullException.ThrowIfNull(socket);

            Id = Guid.NewGuid();
            Socket = socket;
            _isConnected = true;
        }

        #endregion


        #region Acceso Público

        public bool AddRoom(ChatRoom room)
        {
            return _rooms.Add(room);
        }

        public bool RemoveRoom(ChatRoom room)
        {
            return _rooms.Remove(room);
        }

        public void ClearRooms()
        {
            _rooms.Clear();
        }


        public bool AddPrivateChat(ClientConnection client)
        {
            return _privateChats.Add(client);
        }
        public bool RemovePrivateChat(ClientConnection client)
        {
            return _privateChats.Remove(client);
        }
        public void ClearPrivateChats()
        {
            _privateChats.Clear();
        }

        public void Disconnect()
        {
            Socket.GetStream().Close();
            Socket.Close();
            _isConnected = false;
            Disconnected?.Invoke();
        }

        #endregion

    }
}
