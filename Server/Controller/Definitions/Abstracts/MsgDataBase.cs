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
        /// Parámetro "type" de los mensajes de entrada en el protocolo.
        /// </summary>
        [JsonInclude]
        private string type;

        /// <summary>
        /// Da acceso al valor de <see cref="type"/>.
        /// </summary>
        [JsonIgnore]
        public string Type => type;

        /// <summary>
        /// Construye un contenedor de datos de los parámetros: "type"
        /// </summary>
        /// <param name="type"></param>
        public MsgDataBase(string type)
        {
            this.type = type;
        }
    }
}
