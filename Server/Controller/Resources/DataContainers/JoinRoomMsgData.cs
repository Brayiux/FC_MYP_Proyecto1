using Controller.Definitions.Abstracts;
using System.Text.Json.Serialization;

namespace Controller.Resources.DataContainers
{
    /// <summary>
    /// Contenedor de datos de mensaje de tipo "JOIN_ROOM".
    /// </summary>
    public class JoinRoomMsgData : RoomnameMsgDataBase
    {

        public JoinRoomMsgData(string type, string roomname) : base(type, roomname)
        {

        }
    }
}
