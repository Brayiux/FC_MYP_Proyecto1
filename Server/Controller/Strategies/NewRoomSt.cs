using Controller.Data;
using Controller.Definitions.Abstracts;
using Controller.Resources;
using Model.Entities;
using Model.Exceptions;
using System.Threading.Tasks;

namespace Controller.Strategies
{
    public class NewRoomSt : ARStrategyBase
    {
        /// <summary>
        /// Nombre de la sala que el cliente intenta crear.
        /// </summary>
        private readonly string _roomname;

        public NewRoomSt(string roomname)
        {
            _roomname = roomname;
        }
        public async override Task ExecuteAsync(ClientConnection c)
        {
            if (!await HandleClientIdentificationValidationAsync(c))
                return;

            try
            {
                ChatRoom room = new(_roomname, c.User);

                if (ChatData.Instance.ExistsRoom(_roomname))
                {
                    ReplyRoomAlreadyExists(c);
                    return;
                }

                var replyTask = ReplyRoomSuccessfullyCreatedAsync(c);
                c.AddRoom(room);
                ChatData.Instance.AddRoom(room);
                await replyTask;
            } 
            catch (Exception)
            {
                var replyTask =  ReplyInvalidAsync(c);

                ChatData.Instance.RemoveUser(c.User.Username);
                ChatData.Instance.RemoveClient(c.Id);

                await replyTask;

                DisconnectClient(c);
            }

        }

        #region Apoyo

        /// <summary>
        /// Le responde al <paramref name="client"/> que ya existe una
        /// sala con el nombre de la que intentó crear.
        /// </summary>
        /// <param name="client">Usuario al que se le responde.</param>
        private void ReplyRoomAlreadyExists(ClientConnection client)
        {
            MsgBuilder mb = new();

            string response = mb.WithType("RESPONSE")
                                .WithOperation("NEW_ROOM")
                                .WithResult("ROOM_ALREADY_EXISTS")
                                .WithExtra(_roomname)
                                .Build();

            _ = SendMessageAsync(client, response);
        }

        private async Task ReplyRoomSuccessfullyCreatedAsync(ClientConnection client)
        {
            MsgBuilder mb = new();
            string response = mb.WithType("RESPONSE")
                                .WithOperation("NEW_ROOM")
                                .WithResult("SUCCESS")
                                .WithExtra(_roomname)
                                .Build();
            await SendMessageAsync(client, response);
        }

        #endregion
    }
}
