using Controller.Resources;
using Model;
using Model.Definitions.Delegates;

namespace Controller.Definitions.Interfaces
{
    /// <summary>
    /// Provee una interfaz para ejecutar una acción (la cual puede
    /// incluir notificaciones y cambios dentro de la base de datos
    /// del servidor) y posiblemente una respuesta hacia el cliente
    /// que la solicitó.
    /// </summary>
    public interface IARStrategy
    {
        /// <summary>
        /// Ejecuta una acción dentro del servidor y una respuesta a
        /// un cliente que la solicita.
        /// </summary>
        /// <param name="c"></param>
        public Task ExecuteAsync(ClientConnection c);

        public event MessageSentEventHandler MessageSent;
    }
}
