using Controller.Data;
using Controller.Definitions.Interfaces;
using Controller.Resources;
using Model.Definitions.Delegates;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Controller;

/// <summary>
/// Controla y coordina el flujo del servidor: su inicialización, aceptar
/// conexiones, leer información, decodificarla, traducir mensajes en acciones
/// esperadas por el cliente y ejecutar aquellas válidas, así como la desconexión
/// de los clientes.
/// </summary>
public class ServerController
{
	#region Eventos
	/// <summary>
	/// El evento ocurre cuando un mensaje es recibido por el servidor.
	/// </summary>
	public event MessageReceivedEventHandler? MessageReceived;
	/// <summary>
	/// El evento ocurre cuando un mensaje es enviado por el servidor
	/// a un cliente.
	/// </summary>
	public event MessageSentEventHandler? MessageSent;
	#endregion

	#region Campos
	/// <summary>
	/// Decodifica bytes a una cadena de caracteres y visceversa.
	/// </summary>
	private readonly IEncoder<string> _encoder = new EncoderUTF8();

	/// <summary>
	/// Socket del servidor para escuchar conexiones de clientes.
	/// </summary>
	private readonly TcpListener _serverSocket;

	/// <summary>
	/// Traduce mensajes recibidos por el servidor a estrategias de
	/// acción-respuesta.
	/// </summary>
	private readonly MsgTranslator _translator = new MsgTranslator();

	private bool _isActive = false;
	#endregion

	#region Construcción

	public ServerController(IPAddress ip, string port)
	{
		_serverSocket = new(ip, int.Parse(port));
	}

	#endregion

	#region Acceso Público

	public async Task RunAsync()
	{
		StartListening();
        _isActive = true;

        while (_isActive)
		{
			TcpClient clientSocket = await _serverSocket.AcceptTcpClientAsync();
			ClientConnection client = new(clientSocket);

			ConnectionsDAO.Instance.Connect(client);

            _ = HandleClientAsync(client);
		}

		DisconnectAll();
	}

    public void Stop()
    {
		_isActive = false;
		DisconnectAll();
    }
	#endregion

	#region Apoyo

	private void StartListening()
	{
		_serverSocket.Start();
	}

	private async Task HandleClientAsync(ClientConnection c)
	{
		try
		{
            while (c.IsConnected)
            {
                byte[] bytes = await ReceiveDataAsync(c);
                string msg = _encoder.Decode(bytes);
                IARStrategy s = _translator.Translate(msg);
                _ = s.ExecuteAsync(c);
            }
        }
		finally
		{
			DisconnectClient(c);
		}

	}
	private void DisconnectAll()
	{
		foreach (var c in ConnectionsDAO.Instance.GetAll())
		{
			DisconnectClient(c);
		}
	}
	private async Task<byte[]> ReceiveDataAsync(ClientConnection c)
	{
		byte[] buffer = new byte[1024*1024];
        int bytesRead = await c.Socket.GetStream().ReadAsync(buffer);
		return buffer[..bytesRead];
	}

	private void DisconnectClient(ClientConnection c)
	{
		c.IsConnected = false;
        c.Socket.GetStream().Close();
        c.Socket.Close();
    }
	#endregion
}
