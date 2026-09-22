using Client.Models.Definitions;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace Client.Models.Resources
{
    /// <summary>
    /// Es un contenedor de datos de los valores que le corresponden
    /// a cada tipo de clave establecidos en el protocolo, útil para
    /// realizar la serialización (para enviar mensajes), deserialización
    /// (para recibir mensajes), modificación y lectura de datos de manera
    /// sencilla.
    /// </summary>
    public class MsgData
    {

        #region Campos serializables y deserializables

        /// <summary>
        /// Clave "type" del mensaje según el protocolo.
        /// </summary>
        [JsonInclude]
        private string? type;

        /// <summary>
        /// Clave "operation" del mensaje según el protocolo.
        /// </summary>
        [JsonInclude]
        private string? operation;

        /// <summary>
        /// Clave "result" del mensaje según el protocolo.
        /// </summary>
        [JsonInclude]
        private string? result;

        /// <summary>
        /// Clave "extra" del mensaje según el protocolo.
        /// </summary>
        [JsonInclude]
        private string? extra;

        /// <summary>
        /// Clave "username" del mensaje según el protocolo.
        /// </summary>
        [JsonInclude]
        private string? username;

        /// <summary>
        /// Clave "status" del mensaje según el protocolo.
        /// </summary>
        [JsonInclude]
        private string? status;

        /// <summary>
        /// Clave "users" del mensaje según el protocolo.
        /// </summary>
        [JsonInclude]
        private Dictionary<string, string>? users;

        /// <summary>
        /// Clave "usernames" del mensaje según el protocolo.
        /// </summary>
        [JsonInclude]
        private List<string>? usernames;

        /// <summary>
        /// Clave "text" del mensaje según el protocolo.
        /// </summary>
        [JsonInclude]
        private string? text;
        #endregion

        #region Propiedades de acceso

        /// <summary>
        /// Brinda acceso y modificación a la clave "type" del mensaje
        /// según el protocolo.
        /// </summary>
        [JsonIgnore]
        public string? Type
        {
            get => type;
            set => type = value;
        }

        /// <summary>
        /// Brinda acceso y modificación a la clave "operation" del mensaje
        /// según el protocolo.
        /// </summary>
        [JsonIgnore]
        public string? Operation
        {
            get => operation;
            set => operation = value;
        }

        /// <summary>
        /// Brinda acceso y modificación a la clave "result" del mensaje
        /// según el protocolo.
        /// </summary>
        [JsonIgnore]
        public string? Result
        {
            get => result;
            set => result = value;
        }

        /// <summary>
        /// Brinda acceso y modificación a la clave "extra" del mensaje
        /// según el protocolo.
        /// </summary>
        [JsonIgnore]
        public string? Extra
        {
            get => extra;
            set => extra = value;
        }

        /// <summary>
        /// Brinda acceso y modificación a la clave "username" del mensaje
        /// según el protocolo.
        /// </summary>
        [JsonIgnore]
        public string? Username
        {
            get => username;
            set => username = value;
        }

        /// <summary>
        /// Brinda acceso y modificación a la clave "status" del mensaje
        /// según el protocolo.
        /// </summary>
        [JsonIgnore]
        public UserStatus? Status
        {
            get => TextToUserStatus(status);

            set => status = value?.ToString()?.ToUpper();
        }

        /// <summary>
        /// Brinda acceso y modificación a la clave "users" del mensaje
        /// según el protocolo.
        /// </summary>
        [JsonIgnore]
        public IReadOnlyDictionary<string, UserStatus>? Users
        {
            get
            {
                if (users == null) return null;

                Dictionary<string, UserStatus> copy = [];

                foreach (var (username, status) in users)
                {
                    copy[username] = TextToUserStatus(status)!.Value;
                }
                return copy;
            }
            set
            {
                if (value == null)
                {
                    users = null;
                    return;
                }

                users = [];

                foreach (var (username, status) in value)
                {
                    users[username] = status.ToString().ToUpper();
                }
            }
        }

        /// <summary>
        /// Brinda acceso y modificación a la clave "usernames" del mensaje
        /// según el protocolo.
        /// </summary>
        [JsonIgnore]
        public IReadOnlyList<string>? Usernames
        {
            get => usernames;
            set => usernames = value?.ToList();
        }

        /// <summary>
        /// Brinda acceso y modificación a la clave "text" del mensaje
        /// según el protocolo.
        /// </summary>
        [JsonIgnore]
        public string? Text
        {
            get => text;
            set => text = value;
        }

        #endregion

        #region Apoyo

        /// <summary>
        /// Transforma un estado del cliente en formato de texto
        /// a un elemento de la enumeración <see cref="UserStatus"/>
        /// de acuerdo al protocolo.
        /// </summary>
        /// <param name="status">Estado en formato de cadena (y en mayúsculas).
        /// </param>
        /// <returns>El <see cref="UserStatus"/> que le corresponde a
        /// <paramref name="status"/> o <see langword="null"/> si no le 
        /// corresponde ninguno.</returns>
        private static UserStatus? TextToUserStatus(string? status)
        {
            switch (status)
            {
                case "ACTIVE":
                    return UserStatus.Active;
                case "AWAY":
                    return UserStatus.Away;
                case "BUSSY":
                    return UserStatus.Bussy;
            }
            return null;
        }

        #endregion

    }
}
