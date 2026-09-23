using Client.Models.Definitions;
using Client.Models.Resources;
using System;
using System.Text.Json;
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

        public event Action? ReceptionInterrupted;

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
        public bool IsReadingEnabled { get; private set; } = false;

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

            try
            {
                while (Connection.IsConnected && IsReadingEnabled)
                {
                    string msg = await Connection.ReceiveMsgAsync(_cts.Token);
                    MsgData? msgData = DeserializeMsg(msg);
                    if (msgData != null)
                        MsgReceived?.Invoke(msgData);
                }
            }
            catch (OperationCanceledException)
            {
                
            }
            catch (Exception)
            {
                Connection.Disconnect();
            }
            finally
            {
                IsReadingEnabled = false;
                ReceptionInterrupted?.Invoke();
            }
        }

        /// <summary>
        /// Hace que el receptor de mensajes para de recibirlos.
        /// </summary>
        /// <returns></returns>
        public async Task StopReceivingMsgs()
        {
            if (!IsReadingEnabled)
                return;
            IsReadingEnabled = false;
            _cts?.Cancel();
        }


        #endregion

        #region Apoyo
        /// <summary>
        /// Deserializa un mensaje recibido por el servidor en un
        /// contenedor de datos del mensaje (<see cref="MsgData"/>)
        /// </summary>
        /// <param name="msg">Mensaje a deserializar.</param>
        /// <returns>Un contenedor de datos de mensajes del protocolo
        /// si es posible deserializarlo a uno y <see langword="null"/>
        /// de lo contrario.</returns>
        private MsgData? DeserializeMsg(string msg)
        {
            return JsonSerializer.Deserialize<MsgData>(msg);
        }

        #endregion

    }
}
