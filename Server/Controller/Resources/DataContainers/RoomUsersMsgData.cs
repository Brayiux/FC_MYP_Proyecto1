using Controller.Definitions.Abstracts;

namespace Controller.Resources.DataContainers
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
