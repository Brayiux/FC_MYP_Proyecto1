using Controller.Data;
using Controller.Definitions.Abstracts;
using Controller.Resources;
using Model.Entities;
using System.Threading.Tasks;

namespace Controller.Strategies
{
    public class DisconnectSt : ARStrategyBase
    {
        public DisconnectSt()
        {
            
        }
        public async override Task ExecuteAsync(ClientConnection c)
        {

            if (!ChatData.Instance.ExistsUser(c.User.Username))
            {
                await ReplyInvalidAsync(c);
            }

            c.Disconnect();
        }

    }
}
