using Controller.Definitions.Abstracts;
using System.Text.Json.Serialization;

namespace Controller.Resources.DataContainers
{
    /// <summary>
    /// Contenedor de datos de mensaje de tipo "LEAVE_ROOM".
    /// </summary>
    public class LeaveRoomMsgData : RoomnameMsgDataBase
    {

        public LeaveRoomMsgData(string type, string roomname) : base(type, roomname)
        {
            
        }
    }
}
