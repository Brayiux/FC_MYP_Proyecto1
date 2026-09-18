using Controller.Definitions.Abstracts;
using System.Text.Json.Serialization;

namespace Controller.Resources.DataContainers
{
    /// <summary>
    /// Contenedor de datos de mensaje de tipo "ROOM_TEXT".
    /// </summary>
    public class RoomMsgMsgData : RoomnameMsgDataBase
    {
        
        [JsonInclude]
        private readonly string text;
        
        [JsonIgnore]
        public string Text => text;
        public RoomMsgMsgData(string type, string roomname, string text) : base(type, roomname)
        {
            this.text = text;
        }
    }
}
