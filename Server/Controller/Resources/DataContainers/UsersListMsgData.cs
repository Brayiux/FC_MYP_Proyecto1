using Controller.Definitions.Abstracts;

namespace Controller.Resources.DataContainers
{
    /// <summary>
    /// Contenedor de datos de mensaje de tipo "USERS".
    /// </summary>
    public class UsersListMsgData : MsgDataBase
    {
        public UsersListMsgData(string type) : base(type)
        {
            
        }
    }
}
