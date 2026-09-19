using Controller.Data;
using Controller.Definitions.Abstracts;
using Controller.Resources;
using Model.Entities;
using Model.Exceptions;

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
            MsgBuilder mb = new();

            if (!HandleClientIdentificationValidation(c)) return;

            ClientConnection? receiver = ChatData.Instance.GetUserOrNull(_username);

            if (receiver == null)
            {
                ReplyNoSuchUser(c, mb);
                return;
            }

            try
            {
                TextRoomRules.ValidateText(_msg);
                NotifyTextFrom(c, receiver, mb);
                
            }
            catch (InvalidTextException)
            {
                await ReplyInvalidAsync(c, mb);

                ChatData.Instance.RemoveClient(c.Id);
                ChatData.Instance.RemoveUser(c.User.Username);
                DisconnectClient(c);
            }

        }

        #region Apoyo

        /// <summary>
        /// Le responde al <paramref name="client"/> que no hay algún
        /// usuario con el username indicado.
        /// </summary>
        /// <param name="client">Cliente a quien se le responde.</param>
        /// <param name="mb">Constructor del mensaje de respuesta.</param>
        private void ReplyNoSuchUser(ClientConnection client, MsgBuilder mb)
        {
            mb.Reset();
            string response = mb.WithType("RESPONSE")
                                .WithOperation("TEXT")
                                .WithResult("NO_SUCH_USER")
                                .WithExtra(_username)
                                .Build();

            _ = SendMessageAsync(client, response);
        }

        /// <summary>
        /// Notifica al usuario receptor del mensaje de <paramref name="client"/>.
        /// </summary>
        /// <param name="client">El usuario emisor.</param>
        /// <param name="receiver">El usuario receptor.</param>
        /// <param name="mb">Constructor del mensaje de notificación.</param>
        private void NotifyTextFrom(ClientConnection client, ClientConnection receiver, MsgBuilder mb)
        {
            mb.Reset();
            string notification = mb.WithType("TEXT_FROM")
                                    .WithUsername(client.User.Username)
                                    .WithText(_msg)
                                    .Build();

            _ = SendMessageAsync(receiver, notification);

        }

        #endregion
    }
}
