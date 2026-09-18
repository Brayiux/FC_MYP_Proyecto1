using Controller.Definitions.Abstracts;
using System.Text.Json.Serialization;

namespace Controller.Resources.DataContainers
{
    /// <summary>
    /// Contenedor de datos de mensaje de tipo "PUBLIC_TEXT".
    /// </summary>
    public class PublicMsgMsgData : MsgDataBase
    {
        [JsonInclude]
        private string text;

        [JsonIgnore]
        public string Text => text;

        public PublicMsgMsgData(string type, string text) : base(type)
        {
            this.text = text;
        }
    }
}
