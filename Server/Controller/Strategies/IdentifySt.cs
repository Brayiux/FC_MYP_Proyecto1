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

            try
            {
                c.User = new(username: _username);

                UsersDAO.Instance.Identify(c);

                string usersNotification = BuildNewUserMsg(mb);

                foreach (var u in UsersDAO.Instance.GetAll())
                {
                    if (!u.Equals(c))
                    {
                        _ = SendMessageAsync(u, usersNotification);
                    }
                }

                string clientResponse = BuildSuccessfulyIdentifiedResponse(mb);

                _ = SendMessageAsync(c, clientResponse);
            }
            catch (UsernameOutOfRangeException)
            {
                mb.Reset();
                string response = BuildInvalidResponse(mb);
                _ = SendMessageAsync(c, response);
            }
            catch (UserAlreadyExistsException)
            {
                string response = BuildUserAlreadyExistsResponse(mb);
                await SendMessageAsync(c, response);
                DisconnectClient(c);
            }
        }

        private string BuildNewUserMsg(MsgBuilder mb)
        {
            mb.Reset();
            return mb
                    .WithType("NEW_USER")
                    .WithUsername(_username)
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

        private string BuildSuccessfulyIdentifiedResponse(MsgBuilder mb)
        {
            mb.Reset();
            return mb
                    .WithType("RESPONSE")
                    .WithOperation("IDENTIFY")
                    .WithResult("SUCCESS")
                    .WithExtra(_username)
                    .Build();
        }
    }
}
