using Controller.Data;
using Controller.Definitions.Abstracts;
using Controller.Resources;
using Model.Definitions.Enums;
using System.Threading.Tasks;

namespace Controller.Strategies
{
    public class UsersListSt : ARStrategyBase
    {
        public UsersListSt()
        {
            
        }
        public async override Task ExecuteAsync(ClientConnection c)
        {
            if (! await HandleClientIdentificationValidationAsync(c)) return;

            await ReplyUsersListAsync(c);
        }

        #region Apoyo
        /// <summary>
        /// Envía el diccionario usuario-estado al usuario que lo solicitó.
        /// </summary>
        /// <param name="client">Usuario a quien se le envía el diccionario.</param>
        /// <returns></returns>
        private async Task ReplyUsersListAsync(ClientConnection client)
        {
            MsgBuilder mb = new();
            Dictionary<string, UserStatus> users = [];
            foreach (var (username, user) in ChatData.Instance.GetAllUsers())
            {
                users[username] = user.User.Status;
            } 

            string response = mb.WithType("USER_LIST")
                                .WithUsers(users)
                                .Build();

            await SendMessageAsync(client, response);
        }

        #endregion
    }
}
