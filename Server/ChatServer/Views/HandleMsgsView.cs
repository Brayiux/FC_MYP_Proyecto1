using Controller;

namespace ChatServer.Views
{
    /// <summary>
    /// Maneja la vista en consola de mensajes recibidos y enviados
    /// usando el controlador del servidor.
    /// </summary>
    public class HandleMsgsView : IDisposable
    {

        private readonly ServerController _sc;

        public HandleMsgsView(ServerController sc)
        {
            ArgumentNullException.ThrowIfNull(sc);

            _sc = sc;

            _sc.MessageReceived += Sc_MessageReceived;
            _sc.MessageSent += Sc_MessageSent;
        }

        private void Sc_MessageSent(string msg)
        {
            Console.WriteLine($">> {msg}");
        }

        private void Sc_MessageReceived(string msg, string from)
        {
            Console.WriteLine($"<< {msg}");
        }

        public void Dispose()
        {
            _sc.MessageReceived -= Sc_MessageReceived;
            _sc.MessageSent -= Sc_MessageSent;

            GC.SuppressFinalize(this);
        }
    }
}
