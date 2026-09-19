using Controller.Data;
using Controller.Definitions.Abstracts;
using Controller.Resources;
using Model.Entities;

namespace Controller.Strategies
{
    public class LeaveRoomSt : FindRoomARStrategyBase
    {
        public LeaveRoomSt(string roomname) : base(roomname, "LEAVE_ROOM")
        {

        }
        public async override Task ExecuteAsync(ClientConnection c)
        {
            if (!await HandleClientIdentificationValidationAsync(c))
                return;

            ChatRoom? room = ChatData.Instance.GetRoomOrNull(_roomname);

            // Si la sala no existe:

            if (room == null)
            {
                await ReplyNoSuchRoomAsync(c);
                return;
            }

        }
    }
}
