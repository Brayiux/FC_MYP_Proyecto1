using Controller.Data;
using Controller.Definitions.Abstracts;
using Controller.Resources;
using Model.Entities;

namespace Controller.Strategies
{
    public class RoomMsgSt : FindRoomARStrategyBase
    {
        private readonly string _msg;

        public RoomMsgSt(string roomname, string msg) : base(roomname, "ROOM_TEXT")
        {
            _msg = msg;
        }
        public async override Task ExecuteAsync(ClientConnection c)
        {
            if (!await HandleClientIdentificationValidationAsync(c))
                return;

            ChatRoom? room = ChatData.Instance.GetRoomOrNull(_roomname);
            
            // Si el la sala no existe:

            if (room == null)
            {
                await ReplyInvalidAsync(c);
                return;
            }




            
        }

    }
}
