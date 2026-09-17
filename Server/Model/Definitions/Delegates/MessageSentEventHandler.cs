namespace Model.Definitions.Delegates
{
    /// <summary>
    /// Maneja los eventos cuando un mensaje es enviado por el servidor.
    /// </summary>
    /// <param name="msg">Mensaje enviado a un cliente por el servidor.</param>
    public delegate void MessageSentEventHandler(string msg);
}
