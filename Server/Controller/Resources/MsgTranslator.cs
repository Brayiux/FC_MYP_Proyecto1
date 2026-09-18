using Controller.Definitions.Abstracts;
using Controller.Definitions.Interfaces;
using Controller.Resources.DataContainers;
using Controller.Strategies;
using System.Collections;
using System.Text.Json;

namespace Controller.Resources
{
    /// <summary>
    /// Traduce los mensajes decodificados y en formato JSON recibidos
    /// por el servidor a estrategias de acción-respuesta ejecutables.
    /// </summary>
    internal class MsgTranslator
    {
        /// <summary>
        /// Traduce un mensaje recibido por el servidor en una estrategia
        /// de acción-reacción ejecutable, de acuerdo a los doce tipos de
        /// mensajes (y formatos no válidos de mensajes) que el servidor
        /// puede recibir para doce operaciones distintas.
        /// </summary>
        /// <param name="msg">Mensaje a traducir.</param>
        /// <returns>Una estrategia de acción-respuesta que corresponde
        /// en la cual se traduce el mensaje.</returns>
        public IARStrategy Translate(string msg)
        {
            ArgumentNullException.ThrowIfNull(msg);
            JsonSerializerOptions jso = new() { PropertyNameCaseInsensitive = true };

            MsgDataBase? typeData = JsonSerializer.Deserialize<TypeMsgData>(msg, jso);

            if (typeData == null)
            {
                return new DisconnectSt();
            }

            switch (typeData.Type)
            {
                case "IDENTIFY":
                    {
                        IdentifyMsgData data = JsonSerializer.Deserialize<IdentifyMsgData>(msg, jso)!;
                        return new IdentifySt(username: data.Username);
                    }
                case "STATUS":
                    {
                        ChangeStatusMsgData data = JsonSerializer.Deserialize<ChangeStatusMsgData>(msg)!;
                        return new ChangeStatusSt(status: data.Status);
                    }
                case "USERS":
                    {
                        return new UsersListSt();
                    }
                case "TEXT":
                    {
                        PrivateMsgMsgData data = JsonSerializer.Deserialize<PrivateMsgMsgData>(msg)!;
                        return new PrivateMsgSt(username: data.Username, msg: data.Text);
                    }
                case "PUBLIC_TEXT":
                    {
                        PublicMsgMsgData data = JsonSerializer.Deserialize<PublicMsgMsgData>(msg)!;
                        return new PublicMsgSt(msg: data.Text);
                    }
                case "NEW_ROOM":
                    {
                        NewRoomMsgData data = JsonSerializer.Deserialize<NewRoomMsgData>(msg)!;
                        return new NewRoomSt(roomname: data.Roomname);
                    }
                case "INVITE":
                    {
                        InviteUsersMsgData data = JsonSerializer.Deserialize<InviteUsersMsgData>(msg)!;

                        return new InviteUsersSt(roomname: data.Roomname, users: data.Usernames);
                    }
                case "JOIN_ROOM":
                    {
                        JoinRoomMsgData data = JsonSerializer.Deserialize<JoinRoomMsgData>(msg)!;
                        return new JoinRoomSt(roomname: data.Roomname);
                    }
                case "ROOM_USERS":
                    {
                        RoomUsersMsgData data = JsonSerializer.Deserialize<RoomUsersMsgData>(msg)!;
                        return new RoomUsersSt(roomname: data.Roomname);
                    }
                case "ROOM_TEXT":
                    {
                        RoomMsgMsgData data = JsonSerializer.Deserialize<RoomMsgMsgData>(msg)!;
                        return new RoomMsgSt(roomname: data.Roomname, msg: data.Text);
                    }
                case "LEAVE_ROOM":
                    {
                        LeaveRoomMsgData data = JsonSerializer.Deserialize<LeaveRoomMsgData>(msg)!;
                        return new LeaveRoomSt(roomname: data.Roomname);
                    }
                case "DISCONNECT":
                    {
                        return new DisconnectSt();
                    }
            }

            return new DisconnectSt();
        }
    }
}
