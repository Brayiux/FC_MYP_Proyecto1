using Controller.Data;
using Controller.Definitions.Abstracts;
using Controller.Resources;
using Model.Exceptions;

namespace Controller.Strategies
{
    
    public class IdentifySt : ARStrategyBase
    {
        /// <summary>
        /// Nombre de usuario que que la estrategia usa para el intento de identificación
        /// del cliente.
        /// </summary>
        private readonly string _username;
        public IdentifySt(string username)
        {
            _username = username;
        }
        public async override Task ExecuteAsync(ClientConnection c)
        {
            try
            {
                c.User = new(username: _username);

                bool alreadyExists = !ChatData.Instance.AddUser(c);

                if (alreadyExists)
                {
                    ReplyUserAlreadyExists(c);

                    ChatData.Instance.RemoveClient(c.Id);
                    DisconnectClient(c);
                    return;
                }

                // Notificamos a los usuarios
                NotifyToAllUsersANewUserWasIdentified(c);

                //Notificamos al cliente
                ReplySuccessfulyIdentified(c);
            }
            catch (UsernameOutOfRangeException)
            {
                await ReplyInvalidAsync(c);

                ChatData.Instance.RemoveClient(c.Id);
                ChatData.Instance.RemoveUser(c.User.Username);
                DisconnectClient(c);
            }
        }

        #region Apoyo

        /// <summary>
        /// Notifica a todos los usuarios que hay un nuevo usuario <paramref name="client"/>
        /// que se ha identificado.
        /// </summary>
        /// <param name="client">Nuevo usuario que ingresó.</param>
        private void NotifyToAllUsersANewUserWasIdentified(ClientConnection client)
        {
            MsgBuilder mb = new();

            string notification = mb
                                    .WithType("NEW_USER")
                                    .WithUsername(_username)
                                    .Build();

            foreach (var (_, u) in ChatData.Instance.GetAllUsers())
            {
                if (!u.Equals(client))
                {
                    _ = SendMessageAsync(u, notification);
                }
            }
        }

        /// <summary>
        /// Responde al <paramref name="client"/> que se su operación <i>identificar</i>
        /// ha sido exitosa.
        /// </summary>
        /// <param name="client">Cliente cuya identificación ha sido exitosa.</param>
        private void ReplySuccessfulyIdentified(ClientConnection client)
        {
            MsgBuilder mb = new();
            string response = mb
                                .WithType("RESPONSE")
                                .WithOperation("IDENTIFY")
                                .WithResult("SUCCESS")
                                .WithExtra(_username)
                                .Build();

            _ = SendMessageAsync(client, response);

        }

        /// <summary>
        /// Responde al <paramref name="client"/> que ya existe un usuario con el
        /// <i>username</i> con el que se intenta registrar.
        /// </summary>
        /// <param name="client">Cliente al que se le responde.</param>
        private void ReplyUserAlreadyExists(ClientConnection client)
        {
            MsgBuilder mb = new();
            string response = mb
                                .WithType("RESPONSE")
                                .WithOperation("IDENTIFY")
                                .WithResult("USER_ALREADY_EXISTS")
                                .WithExtra(_username)
                                .Build();
            _ = SendMessageAsync(client, response);
        }

        #endregion
    }
}
