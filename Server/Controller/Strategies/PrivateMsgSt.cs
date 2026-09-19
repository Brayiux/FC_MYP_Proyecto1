using Controller.Data;
using Controller.Definitions.Abstracts;
using Controller.Resources;
using Model.Entities;
using Model.Exceptions;
using System.Threading.Tasks;

namespace Controller.Strategies
{
    public class PrivateMsgSt : ARStrategyBase
    {
        /// <summary>
        /// Nombre de usuario a quien se quiere enviar el texto privado.
        /// </summary>
        private readonly string _username;

        /// <summary>
        /// Texto que se le intenta enviar al receptor.
        /// </summary>
        private readonly string _msg;

        public PrivateMsgSt(string username, string msg)
        {
            _username = username;
            _msg = msg;
        }
        public async override Task ExecuteAsync(ClientConnection c)
        {

            if (!await HandleClientIdentificationValidationAsync(c)) 
                return;

            ClientConnection? receiver = ChatData.Instance.GetUserOrNull(_username);

            if (receiver == null)
            {
                await ReplyNoSuchUserAsync(c);
                return;
            }

            try
            {
                TextRoomRules.ValidateText(_msg);
                await NotifyTextFromAsync(c, receiver);
                
            }
            catch (InvalidTextException)
            {
                await ReplyInvalidAsync(c);
                c.Disconnect();
            }

        }

        #region Apoyo

        /// <summary>
        /// Le responde al <paramref name="client"/> que no hay algún
        /// usuario con el username indicado.
        /// </summary>
        /// <param name="client">Cliente a quien se le responde.</param>
        /// <returns></returns>
        private async Task ReplyNoSuchUserAsync(ClientConnection client)
        {
            MsgBuilder mb = new();
            string response = mb.WithType("RESPONSE")
                                .WithOperation("TEXT")
                                .WithResult("NO_SUCH_USER")
                                .WithExtra(_username)
                                .Build();

            await SendMessageAsync(client, response);
        }

        /// <summary>
        /// Notifica al usuario receptor del mensaje de <paramref name="client"/>.
        /// </summary>
        /// <param name="client">El usuario emisor.</param>
        /// <param name="receiver">El usuario receptor.</param>
        /// <returns></returns>
        private async Task NotifyTextFromAsync(ClientConnection client, ClientConnection receiver)
        {
            MsgBuilder mb = new();
            string notification = mb.WithType("TEXT_FROM")
                                    .WithUsername(client.User.Username)
                                    .WithText(_msg)
                                    .Build();
            await SendMessageAsync(receiver, notification);

        }

        #endregion
    }
}
