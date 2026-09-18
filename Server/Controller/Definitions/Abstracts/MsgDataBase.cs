using System.Text.Json.Serialization;

namespace Controller.Definitions.Abstracts
{
    /// <summary>
    /// Es la clase base para los contenedores de datos que sirven
    /// para la deserialización de mensajes provenientes del cliente.
    /// </summary>
    public abstract class MsgDataBase
    {
        /// <summary>
        /// Da acceso al valor de <see cref="type"/>.
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Construye un contenedor de datos de los parámetros: "type"
        /// </summary>
        /// <param name="type"></param>
        public MsgDataBase(string type)
        {
            Type = type;
        }
    }
}
