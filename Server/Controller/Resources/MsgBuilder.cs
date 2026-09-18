using Model.Definitions.Enums;
using System.Text.Json;

namespace Controller.Resources
{
    /// <summary>
    /// Permite construir cualquier mensaje que se especifica
    /// en el protocolo, para responder a clientes.
    /// </summary>
    public class MsgBuilder
    {
        /// <summary>
        /// Almacena los datos para serializar el mensaje a JSON al final.
        /// </summary>
        private readonly Dictionary<string, object> _msgData = [];

        #region Construcción

        /// <summary>
        /// Crea un <i>builder</i> de mensajes que el servidor le envía
        /// al cliente, siguiendo un formato JSON.
        /// </summary>
        public MsgBuilder()
        {
            Reset();
        }

        /// <summary>
        /// Añade el parámetro "type" al mensaje.
        /// </summary>
        /// <param name="type"></param>
        /// <returns>Esta instancia del <i>builder</i> con la modificación
        /// del mensaje.</returns>
        public MsgBuilder WithType(string type)
        {
            _msgData[nameof(type)] = type;
            return this;
        }

        /// <summary>
        /// Añade el parámetro "operation" al mensaje.
        /// </summary>
        /// <param name="operation"></param>
        /// <returns>Esta instancia del <i>builder</i> con la modificación
        /// del mensaje.</returns>
        public MsgBuilder WithOperation(string operation)
        {
            _msgData[nameof(operation)] = operation;
            return this;
        }

        /// <summary>
        /// Añade el parámetro "result" al mensaje.
        /// </summary>
        /// <param name="result"></param>
        /// <returns>Esta instancia del <i>builder</i> con la modificación
        /// del mensaje.</returns>
        public MsgBuilder WithResult(string result)
        {
            _msgData[nameof(result)] = result;
            return this;
        }

        /// <summary>
        /// Añade el parámetro "extra" al mensaje.
        /// </summary>
        /// <param name="extra"></param>
        /// <returns>Esta instancia del <i>builder</i> con la modificación
        /// del mensaje.</returns>
        public MsgBuilder WithExtra(string extra)
        {
            _msgData[nameof(extra)] = extra;
            return this;
        }

        /// <summary>
        /// Añade el parámetro "username" al mensaje.
        /// </summary>
        /// <param name="username"></param>
        /// <returns>Esta instancia del <i>builder</i> con la modificación
        /// del mensaje.</returns>
        public MsgBuilder WithUsername(string username)
        {
            _msgData[nameof(username)] = username;
            return this;
        }

        /// <summary>
        /// Añade el parámetro "status" al mensaje.
        /// </summary>
        /// <param name="status"></param>
        /// <returns>Esta instancia del <i>builder</i> con la modificación
        /// del mensaje.</returns>
        public MsgBuilder WithStatus(UserStatus status)
        {
            _msgData[nameof(status)] = status.ToString().ToUpper();
            return this;
        }

        /// <summary>
        /// Añade el parámetro "users" al mensaje.
        /// </summary>
        /// <param name="users"></param>
        /// <returns>Esta instancia del <i>builder</i> con la modificación
        /// del mensaje.</returns>
        public MsgBuilder WithUsers(IReadOnlyDictionary<string, UserStatus> users)
        {
            Dictionary<string, string> copy = [];
            foreach ((string username, UserStatus status) in users)
            {
                copy[username] = status.ToString().ToUpper();
            }

            _msgData[nameof(users)] = copy;
            return this;
        }

        /// <summary>
        /// Añade el parámetro "text" al mensaje.
        /// </summary>
        /// <param name="text"></param>
        /// <returns>Esta instancia del <i>builder</i> con la modificación
        /// del mensaje.</returns>
        public MsgBuilder WithText(string text)
        {
            _msgData[nameof(text)] = text;
            return this;
        }
        /// <summary>
        /// Añade el parámetro "roomname" al mensaje.
        /// </summary>
        /// <param name="roomname"></param>
        /// <returns>Esta instancia del <i>builder</i> con la modificación
        /// del mensaje.</returns>
        public MsgBuilder WithRoomname(string roomname)
        {
            _msgData[nameof(roomname)] = roomname;
            return this;
        }

        /// <summary>
        /// Construye el mensaje a partir de los parámetros añadidos.
        /// </summary>
        /// <returns>El mensaje construido con este <i>builder</i></returns>
        /// <exception cref="InvalidOperationException">Si el mensaje está vacío
        /// (no se ha añadido ningún parámetro).</exception>
        public string Build()
        {
            if (_msgData.Count == 0)
                throw new InvalidOperationException(
                    "Error: No puede construir el mensaje porque está vacío.");


            return JsonSerializer.Serialize(_msgData);
        }

        /// <summary>
        /// Regresa el <i>builder</i> a su estado inicial, reiniciando los datos del
        /// mensaje.
        /// </summary>
        public void Reset()
        {
            _msgData.Clear();
        }

        #endregion
    }
}
