using Controller.Definitions.Interfaces;

namespace Controller.Resources
{
    /// <summary>
    /// Traduce los mensajes decodificados y en formato JSON recibidos
    /// por el servidor a estrategias de acción-respuesta ejecutables.
    /// </summary>
    internal class MsgTranslator
    {
        public IARStrategy Translate(string msg)
        {
            throw new NotImplementedException();
        }
    }
}
