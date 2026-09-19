using Controller.Data;
using Controller.Definitions.Abstracts;
using Controller.Resources;
using Model.Entities;
using Model.Exceptions;

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
            if (!await HandleClientIdentificationValidationAsync(c))
                return;

            try
            {
                TextRoomRules.ValidateText(_msg);
                NotifyPublicTextFrom(c);

            }
            catch (InvalidTextException)
            {
                await ReplyInvalidAsync(c);

                ChatData.Instance.RemoveClient(c.Id);
                ChatData.Instance.RemoveUser(c.User.Username);
                DisconnectClient(c);
            }

        }

        #region Apoyo

        /// <summary>
        /// Notifica a varios usuarios de un mensaje de texto enviado
        /// al chat principal por <paramref name="client"/>.
        /// </summary>
        /// <param name="client">Usuario que envía el texto público.</param>
        private void NotifyPublicTextFrom(ClientConnection client)
        {
            MsgBuilder mb = new();
            string notification = mb.WithType("PUBLIC_TEXT_FROM")
                                    .WithUsername(client.User.Username)
                                    .WithText(_msg)
                                    .Build();

            foreach (var (_, user) in ChatData.Instance.GetAllUsers())
            {
                if (!user.Equals(client))
                {
                    _ = SendMessageAsync(client, notification);
                }
            }
        }

        #endregion
    }
}
