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

        public string Username { get; set; }

        public IdentifyMsgData(string type, string username) : base(type)
        {
            Username = username;
        }
    }
}
