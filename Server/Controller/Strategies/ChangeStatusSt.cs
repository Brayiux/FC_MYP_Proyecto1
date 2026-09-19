using Controller.Data;
using Controller.Definitions.Abstracts;
using Controller.Resources;
using Model.Definitions.Enums;
using Model.Exceptions;
using System.Threading.Tasks;

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
            if (! await HandleClientIdentificationValidationAsync(c))
                return;

            try
            {
                c.User.Status = _newStatus;

                await NotifyStatusChangedAsync(c);
            } 
            catch (InvalidClientStatusException)
            {
                await ReplyInvalidAsync(c);

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
        private async Task NotifyStatusChangedAsync(ClientConnection client)
        {
            MsgBuilder mb = new();

            string notification = mb.WithType("NEW_STATUS")
                                    .WithUsername(client.User.Username)
                                    .WithStatus(client.User.Status)
                                    .Build();
            List<Task> tasks = [];

            foreach (var (_,user) in ChatData.Instance.GetAllUsers())
            {
                if (!client.Equals(user))
                {
                    tasks.Add(SendMessageAsync(user, notification));
                }
            }
            await Task.WhenAll(tasks);
        }

        #endregion
    }
}
