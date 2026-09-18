using System.Text.Json.Serialization;

namespace Controller.Definitions.Abstracts
{
    /// <summary>
    /// Es la clase base para los contenedores de datos que sirven
    /// para la deserialización de mensajes provenientes del cliente,
    /// incluyendo parámetros "type" y "roomname".
    /// </summary>
    public class RoomnameMsgDataBase : MsgDataBase
    {
        /// <summary>
        /// Parámetro "roomname" de los mensajes de entrada al servidor
        /// del protocolo.
        /// </summary>
        [JsonInclude]
        private readonly string roomname;

        /// <summary>
        /// Da acceso al valor de <see cref="roomname"/>.
        /// </summary>
        [JsonIgnore]
        public string Roomname => roomname;

        /// <summary>
        /// Construye un contenedor de datos de los parámetros:
        /// "type", "roomname".
        /// </summary>
        /// <param name="type"></param>
        /// <param name="roomname"></param>
        public RoomnameMsgDataBase(string type, string roomname) : base(type)
        {
            this.roomname = roomname;
        }
    }
}
