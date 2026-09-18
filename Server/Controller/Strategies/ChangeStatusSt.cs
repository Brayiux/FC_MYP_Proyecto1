using Controller.Definitions.Abstracts;
using Controller.Definitions.Interfaces;
using Controller.Resources;
using Model.Definitions.Enums;

namespace Controller.Strategies
{
    public class ChangeStatusSt : ARStrategyBase
    {
        private UserStatus _newStatus;
        public ChangeStatusSt(UserStatus status)
        {
            _newStatus = status;
        }
        public override Task ExecuteAsync(ClientConnection c)
        {
            throw new NotImplementedException();
        }
    }
}
