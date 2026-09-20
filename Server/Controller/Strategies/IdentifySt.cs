using Controller.Data;
using Controller.Definitions.Abstracts;
using Controller.Resources;
using Model.Exceptions;
using System.Threading.Tasks;

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
            if (c.User != null)
            {
                await ReplyInvalidAsync(c);
                c.Disconnect();
                return;
            }

            try
            {
                c.User = new(username: _username);

                bool alreadyExists = !ChatData.Instance.AddUser(c);

                if (alreadyExists)
                {
                    c.User = null;
                    await ReplyUserAlreadyExistsAsync(c);
                    return;
                }

                // Notificamos a los usuarios
                await NotifyNewUserAsync(c);

                //Notificamos al cliente
                await ReplySuccessfulyIdentifiedAsync(c);
            }
            catch (UsernameOutOfRangeException)
            {
                await ReplyInvalidAsync(c);

                c.Disconnect();
            }
        }

        #region Apoyo

        /// <summary>
        /// Notifica a todos los usuarios que hay un nuevo usuario <paramref name="client"/>
        /// que se ha identificado.
        /// </summary>
        /// <param name="client">Nuevo usuario que ingresó.</param>
        /// <returns></returns>
        private async Task NotifyNewUserAsync(ClientConnection client)
        {
            MsgBuilder mb = new();

            string notification = mb
                                    .WithType("NEW_USER")
                                    .WithUsername(_username)
                                    .Build();
            List<Task> tasks = [];

            foreach (var (_, u) in ChatData.Instance.GetAllUsers())
            {
                if (u.Equals(client))
                    continue;
                tasks.Add(SendMessageAsync(u, notification));
            }
            await Task.WhenAll(tasks);
        }

        /// <summary>
        /// Responde al <paramref name="client"/> que se su operación <i>identificar</i>
        /// ha sido exitosa.
        /// </summary>
        /// <param name="client">Cliente cuya identificación ha sido exitosa.</param>
        /// <returns></returns>
        private async Task ReplySuccessfulyIdentifiedAsync(ClientConnection client)
        {
            MsgBuilder mb = new();
            string response = mb
                                .WithType("RESPONSE")
                                .WithOperation("IDENTIFY")
                                .WithResult("SUCCESS")
                                .WithExtra(_username)
                                .Build();

            await SendMessageAsync(client, response);

        }

        /// <summary>
        /// Responde al <paramref name="client"/> que ya existe un usuario con el
        /// <i>username</i> con el que se intenta registrar.
        /// </summary>
        /// <param name="client">Cliente al que se le responde.</param>
        /// <returns></returns>
        private async Task ReplyUserAlreadyExistsAsync(ClientConnection client)
        {
            MsgBuilder mb = new();
            string response = mb
                                .WithType("RESPONSE")
                                .WithOperation("IDENTIFY")
                                .WithResult("USER_ALREADY_EXISTS")
                                .WithExtra(_username)
                                .Build();
            await SendMessageAsync(client, response);
        }

        #endregion
    }
}
