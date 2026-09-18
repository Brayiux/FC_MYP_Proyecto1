using Controller.Resources.DataContainers;
using System.Text.Json.Serialization;

namespace Controller.Definitions.Abstracts
{
    /// <summary>
    /// Contenedor de datos de mensaje de tipo "ROOM_USERS".
    /// </summary>
    public class RoomUsersMsgData : RoomnameMsgDataBase
    {
        public RoomUsersMsgData(string type, string roomname) : base(type, roomname)
        {
            
        }
    }
}
