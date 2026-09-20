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
        private readonly List<string> usernames;

        [JsonIgnore]
        public List<string> Usernames => usernames;

        public InviteUsersMsgData(string type, string roomname, List<string> usernames) : base(type, roomname)
        {
            this.usernames = usernames;
        }
    }
}
