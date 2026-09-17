using Controller.Data;

namespace Controller.Resources
{
    /// <summary>
    /// Envía un mensaje a un usuario conectado
    /// </summary>
    public class UsersMsgSender
    {
        
        private readonly UsersDAO _usersDAO;
        public ClientConnection UserConnection { get; set; }
        public UsersMsgSender(ClientConnection userConnection, UsersDAO usersDAO)
        {
            throw new NotImplementedException();
        }

        public void SendMsg(string msg)
        {
            throw new NotImplementedException();
        }
    }
}
