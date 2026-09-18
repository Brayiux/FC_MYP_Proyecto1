using Controller.Definitions.Abstracts;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Controller.Resources.DataContainers
{
    /// <summary>
    /// Contenedor de datos de mensaje de tipo "IDENTIFY".
    /// </summary>
    public class IdentifyMsgData : MsgDataBase
    {
        [JsonInclude]
        private readonly string username;

        [JsonIgnore]
        public string Username => username;

        public IdentifyMsgData(string type, string username) : base(type)
        {
            this.username = username;
        }
    }
}
