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

	#endregion

	#region Construcción

	public ServerController(IPAddress ip, string port)
	{
		throw new NotImplementedException();
	}

	#endregion

	#region Acceso Público

	public async Task RunAsync()
	{
		throw new NotImplementedException();
	}

    public void Stop()
    {
        throw new NotImplementedException();
    }
	#endregion
}
