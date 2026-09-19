using Controller.Definitions.Abstracts;
using System.Text.Json.Serialization;

namespace Controller.Resources.DataContainers
{
    /// <summary>
    /// Contenedor de datos de mensaje de tipo "INVITE".
    /// </summary>
    public class InviteUsersMsgData : RoomnameMsgDataBase
    {

        [JsonInclude]
        private readonly List<string> users;

        [JsonIgnore]
        public List<string> Users => users;

        public InviteUsersMsgData(string type, string roomname, List<string> users) : base(type, roomname)
        {
            this.users = users;
        }
    }
}
