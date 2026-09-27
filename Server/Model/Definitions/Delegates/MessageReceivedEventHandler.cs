namespace Model.Definitions.Delegates
{
    /// <summary>
    /// Maneja los eventos cuando un mensaje es recibido por el servidor.
    /// </summary>
    /// <param name="msg">Mensaje que es recibido.</param>
    public delegate void MessageReceivedEventHandler(string msg);
}
