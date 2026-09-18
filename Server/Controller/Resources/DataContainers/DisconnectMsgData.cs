using Controller.Definitions.Abstracts;

namespace Controller.Resources.DataContainers
{
    /// <summary>
    /// Contenedor de datos de mensaje de tipo "DISCONNECT".
    /// </summary>
    public class DisconnectMsgData : MsgDataBase
    {

        public DisconnectMsgData(string type) : base(type)
        {
            
        }
    }
}
