using Client.Models.Definitions;
using System.Threading;
using System.Threading.Tasks;

namespace Client.Models.Connection
{
    /// <summary>
    /// Singleton que sirve como un punto de comunicación accesible
    /// con el servidor (a través de la conexión del cliente) sólo 
    /// para la recepción de mensajes de mensajes.
    /// </summary>
    public class MsgReceiver
    {
        #region Eventos

        /// <summary>
        /// Ocurre cuando ha detectado un mensaje nuevo una vez
        /// iniciada su lectura.
        /// </summary>
        public event MsgReceivedEventHandler? MsgReceived;

        #endregion

        #region Campos

        /// <summary>
        /// Instancia única del receptor de mensajes.
        /// </summary>
        private static MsgReceiver? _instance;

        /// <summary>
        /// Recurso de cancelación para la lectura en curso.
        /// </summary>
        private CancellationTokenSource? _cts;

        #endregion

        #region Propiedades

        /// <summary>
        /// Obtiene o crea la única instancia del receptor de mensajes.
        /// </summary>
        public static MsgReceiver Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new MsgReceiver();
                return _instance;
            }
        }

        /// <summary>
        /// Obtiene la instancia de la conexión con el cliente.
        /// </summary>
        private static ClientConnection Connection => ClientConnection.Instance;

        /// <summary>
        /// Indica cuando la lectura de mensajes ha comenzado.
        /// </summary>
        private bool IsReadingEnabled { get; set; } = false;

        #endregion

        #region Construcción
        /// <summary>
        /// Constructor privado para el patrón Singleton.
        /// </summary>
        private MsgReceiver() { }
        #endregion

        #region Acceso público
        
        /// <summary>
        /// Comienza la lectura de mensajes del servidor.
        /// </summary>
        public async Task StartReceivingMsgs()
        {
            if (!Connection.IsConnected || IsReadingEnabled)
                return;

            _cts?.Dispose();
            _cts = new();
            IsReadingEnabled = true;

            while (Connection.IsConnected && IsReadingEnabled)
            {
                string msg = await Connection.ReceiveMsgAsync();
                
                MsgReceived?.Invoke(msg);
            }
        }

        public async Task StopReceivingMsgs()
        {
            IsReadingEnabled = false;
            _cts?.Cancel();
        }
        

        #endregion

    }
}
