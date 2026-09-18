using Controller.Definitions.Abstracts;
using Model.Definitions.Enums;
using System.Text.Json.Serialization;

namespace Controller.Resources.DataContainers
{
    /// <summary>
    /// Contenedor de datos de mensaje de tipo "STATUS".
    /// </summary>
    public class ChangeStatusMsgData : MsgDataBase
    {
        [JsonInclude]
        private readonly string status;

        [JsonIgnore]
        public UserStatus Status
        {
            get
            {
                switch (status)
                {
                    case "ACTIVE":
                        return UserStatus.Active;
                    case "AWAY":
                        return UserStatus.Away;
                    case "BUSSY":
                        return UserStatus.Bussy;
                }
                throw new Exception();
            }
        }
        public ChangeStatusMsgData(string type, string status) : base(type)
        {
            this.status = status;
        }
    }
}
