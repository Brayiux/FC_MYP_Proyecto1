using Client.Models.Entities;

namespace Client.Models.Definitions
{
    /// <summary>
    /// Maneja los eventos que se invocan cuando una sala se ha vaciado.
    /// </summary>
    /// <param name="room"></param>
    public delegate void EmptiedRoomEventHandler(ChatRoom room);
}
