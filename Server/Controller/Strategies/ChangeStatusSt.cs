using Controller.Data;
using Controller.Definitions.Abstracts;
using Controller.Definitions.Interfaces;
using Controller.Resources;
using Model.Definitions.Enums;
using Model.Exceptions;

namespace Controller.Strategies
{
    public class ChangeStatusSt : ARStrategyBase
    {
        private UserStatus _newStatus;
        public ChangeStatusSt(UserStatus status)
        {
            _newStatus = status;
        }
        public async override Task ExecuteAsync(ClientConnection c)
        {

            MsgBuilder mb = new();

            if (!HandleClientIdentificationValidation(c)) return;

            try
            {
                c.User.Status = _newStatus;

                NotifyStatusChanged(c, mb);
            } 
            catch (InvalidClientStatusException)
            {
                await ReplyInvalidAsync(c, mb);

                ChatData.Instance.RemoveUser(c.User.Username);
                ChatData.Instance.RemoveClient(c.Id);
                DisconnectClient(c);
            }
        }

        #region Apoyo

        /// <summary>
        /// Notifica a todos los usuarios (menos a <paramref name="client"/>)
        /// que este ha cambiado de estado e indica a cuál.
        /// </summary>
        /// <param name="client">Cliente que cambió de estado.</param>
        /// <param name="mb">Constructor del mensaje de notificación.</param>
        private void NotifyStatusChanged(ClientConnection client, MsgBuilder mb)
        {
            mb.Reset();

            string notification = mb.WithType("NEW_STATUS")
                                    .WithUsername(client.User.Username)
                                    .WithStatus(client.User.Status)
                                    .Build();
            
            foreach (var (_,user) in ChatData.Instance.GetAllUsers())
            {
                if (!client.Equals(user))
                {
                    _ = SendMessageAsync(user, notification);
                }
            }
        }

        #endregion
    }
}
