using Controller.Data;
using Controller.Definitions.Abstracts;
using Controller.Resources;
using Model.Exceptions;

namespace Controller.Strategies
{
    
    public class IdentifySt : ARStrategyBase
    {
        private readonly string _username;
        public IdentifySt(string username)
        {
            _username = username;
        }
        public async override Task ExecuteAsync(ClientConnection c)
        {
            MsgBuilder mb = new();


            if (UsersDAO.Instance.Exists(_username))
            {
                string response = BuildUserAlreadyExistsResponse(mb);
                await SendMessageAsync(c, response);
                return;
            }

            try
            {
                c.User = new(username: _username);
            }
            catch (UsernameOutOfRangeException)
            {
                mb.Reset();
                string response = BuildInvalidResponse(mb);
                await SendMessageAsync(c, response);
            }
        }

        private string BuildInvalidResponse(MsgBuilder mb)
        {
            return mb
                    .WithType("RESPONSE")
                    .WithOperation("IDENTIFY")
                    .WithResult("USER_ALREADY_EXISTS")
                    .WithExtra(_username)
                    .Build();
        }

        private string BuildUserAlreadyExistsResponse(MsgBuilder mb)
        {
            mb.Reset();
            return mb
                    .WithType("RESPONSE")
                    .WithOperation("IDENTIFY")
                    .WithResult("USER_ALREADY_EXISTS")
                    .WithExtra(_username)
                    .Build();
        }
    }
}
