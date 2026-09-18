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
        #region Propiedades

        public Guid Id { get; }

        public bool IsConnected { get; set; } = false;

        public ChatUser User { get; set; }
        
        public TcpClient Socket { get; }

        #endregion

        #region Construcción

        public ClientConnection(TcpClient socket)
        {
            ArgumentNullException.ThrowIfNull(socket);

            Id = Guid.NewGuid();
            Socket = socket;
        }

        #endregion

    }
}
