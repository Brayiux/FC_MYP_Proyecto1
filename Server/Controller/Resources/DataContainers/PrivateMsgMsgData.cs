using Controller.Definitions.Abstracts;
using System.Text.Json.Serialization;

namespace Controller.Resources.DataContainers
{
    /// <summary>
    /// Contenedor de datos de mensaje de tipo "TEXT".
    /// </summary>
    public class PrivateMsgMsgData : MsgDataBase
    {
        [JsonInclude]
        private readonly string username;

        [JsonInclude]
        private readonly string text;

        [JsonIgnore]
        public string Username => username;

        [JsonIgnore]
        public string Text => text;

        public PrivateMsgMsgData(string type, string username, string text) : base(type)
        {
            this.username = username;
            this.text = text;
        }
    }
}
