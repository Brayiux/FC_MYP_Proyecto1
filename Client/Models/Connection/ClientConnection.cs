using Client.Models.Definitions;
using Client.Models.Entities;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Client.Models.Connection
{
    public class ClientConnection
    {
        #region Eventos
        /// <summary>
        /// Ocurre cuando la conexión del cliente con el
        /// servidor ha finalizado.
        /// </summary>
        public event Action? Disconnected;

        #endregion


        #region Campos

        /// <summary>
        /// Única instancia de este objeto.
        /// </summary>
        private static ClientConnection? _instance;

        /// <summary>
        /// Usuario de la conexión.
        /// </summary>
        private ChatUser? _user;

        /// <summary>
        /// Socket de la conexión.
        /// </summary>
        private TcpClient? _socket;

        #endregion

        #region Propiedades

        /// <summary>
        /// Obtiene la única instancia de este objeto.
        /// </summary>
        public static ClientConnection Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new();
                return _instance;
            }
        }

        /// <summary>
        /// Obtiene el usuario de la conexión.
        /// </summary>
        public ChatUser? User
        {
            get => _user;
            set
            {
                if (_user != null)
                    return;
                _user = value;
            }
        }

        /// <summary>
        /// Obtiene el socket de la conexión con el cliente.
        /// </summary>
        public TcpClient? Socket => _socket;

        /// <summary>
        /// Indica si el cliente se encuentra conectado.
        /// </summary>
        public bool IsConnected { get; private set; } = false;

        #endregion

        #region ClientConnection

        /// <summary>
        /// Constructor privado para implementar el patrón
        /// Singleton.
        /// </summary>
        private ClientConnection() { }

        #endregion


        #region Acceso Publico
        
        /// <summary>
        /// Envía un mensaje de manera asíncrona al servidorl
        /// </summary>
        /// <param name="msg">Mensaje que se envía al servidor.</param>
        /// <returns></returns>
        public async Task SendMsgAsync(string msg)
        {
            ValidateSocket();
            ValidateMessage(msg);

            try
            {
                await _socket!.GetStream().WriteAsync(Encode(msg));
            }
            finally
            {
                Disconnect();
            }
        }

        /// <summary>
        /// Realiza la lectura de mensajes que envía el servidor.
        /// </summary>
        /// <returns>Una cadena de caracteres que representa el mensaje
        /// recibido por el cliente.</returns>
        public async Task<string> ReceiveMsgAsync()
        {
            ValidateSocket();

            byte[] bytes = new byte[1024 * 1024];
            try
            {
                int bytesRead = await _socket!.GetStream().ReadAsync(bytes);
                return Decode(bytes, 0, bytesRead);

            }
            finally
            {
                Disconnect();
            }
        }

        #endregion

        #region Apoyo

        /// <summary>
        /// Realiza una conexión con el servidor en el ip
        /// y puerto indicados.
        /// </summary>
        /// <param name="ip">IP del servidor.</param>
        /// <param name="port">Puerto donde se conectará el socket.</param>
        /// <returns></returns>
        private async Task ConnectAsync(string ip, int port)
        {
            if (IsConnected)
                return;
            try
            {
                _socket = new TcpClient(ip, port);
                IsConnected = true;
            }
            catch (SocketException)
            {
                
            }
        }

        /// <summary>
        /// Desconecta al cliente del servidor.
        /// </summary>
        private void Disconnect()
        {
            _socket?.GetStream().Close();
            _socket?.Close();

            IsConnected = false;

            Disconnected?.Invoke();
        }

        private byte[] Encode(string msg)
        {
            return Encoding.UTF8.GetBytes(msg);
        }

        /// <summary>
        /// Convierte bytes en una cadena de caracteres en la posición
        /// de acuerdo al rango indicado.
        /// </summary>
        /// <param name="bytes">Mensaje codificado en bytes.</param>
        /// <param name="index">Índice de comienzo de lectura.</param>
        /// <param name="count">Número de bytes que se leerán.</param>
        /// <returns></returns>
        private string Decode(byte[] bytes, int index, int count)
        {
            return Encoding.UTF8.GetString(bytes, index, count);
        }

        /// <summary>
        /// Valida que el socket del cliente esté en el estado adecuado
        /// para comunicarse con el servidor.
        /// </summary>
        /// <exception cref="InvalidOperationException">Si el socket
        /// del cliente es nulo.</exception>
        private void ValidateSocket()
        {
            if (_socket == null || !IsConnected)
            {
                throw new InvalidOperationException(
                    "Error: El socket de ClientConnection aún no se ha asignado.");
            }
        }

        /// <summary>
        /// Valida que el mensaje que se envía al servidor sea adecuado.
        /// </summary>
        /// <param name="msg">El mensaje que se enviará al servidor.</param>
        /// <exception cref="ArgumentNullException">Si el mensaje
        /// es nulo.</exception>
        private void ValidateMessage(string msg)
        {
            if (msg == null)
            {
                throw new ArgumentNullException(nameof(msg),
                    "Error: El mensaje no puede ser nulo");
            }
        }

        #endregion

    }
}
