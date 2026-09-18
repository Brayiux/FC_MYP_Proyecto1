using Controller.Definitions.Abstracts;
using System.Text.Json.Serialization;

namespace Controller.Resources.DataContainers
{
    /// <summary>
    /// Contenedor de datos de mensaje de tipo "NEW_ROOM".
    /// </summary>
    public class NewRoomMsgData : RoomnameMsgDataBase
    {

        public NewRoomMsgData(string type, string roomname) : base(type, roomname)
        {

        }
    }
}
