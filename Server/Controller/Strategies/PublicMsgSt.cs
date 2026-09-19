using Controller.Definitions.Abstracts;
using Controller.Resources;

namespace Controller.Strategies
{
    public class PublicMsgSt : ARStrategyBase
    {
        /// <summary>
        /// El mensaje que se intenta enviar al chat público.
        /// </summary>
        private readonly string _msg;

        public PublicMsgSt(string msg)
        {
            _msg = msg;
        }
        public async override Task ExecuteAsync(ClientConnection c)
        {
            MsgBuilder mb = new();

            if (!await HandleClientIdentificationValidationAsync(c))
                return;


        }
    }
}
