using Controller.Data;
using Controller.Definitions.Abstracts;
using Controller.Resources;
using Model.Definitions.Enums;

namespace Controller.Strategies
{
    public class UsersListSt : ARStrategyBase
    {
        public UsersListSt()
        {
            
        }
        public async override Task ExecuteAsync(ClientConnection c)
        {
            MsgBuilder mb = new();

            if (!HandleClientIdentificationValidation(c)) return;

            ReplyUsersList(c, mb);
        }

        #region Apoyo

        private void ReplyUsersList(ClientConnection client, MsgBuilder mb)
        {

            Dictionary<string, UserStatus> users = [];
            foreach (var (username, user) in ChatData.Instance.GetAllUsers())
            {
                users[username] = user.User.Status;
            } 

            string response = mb.WithType("USER_LIST")
                                .WithUsers(users)
                                .Build();

            _ = SendMessageAsync(client, response);
        }

        #endregion
    }
}
