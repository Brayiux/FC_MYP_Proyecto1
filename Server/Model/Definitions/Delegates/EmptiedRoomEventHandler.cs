using Model.Entities;

namespace Model.Definitions.Delegates
{
    /// <summary>
    /// Maneja los eventos cuando una sala se vacía por completo.
    /// </summary>
    /// <param name="room">Sala que se ha vaciado.</param>
    public delegate void EmptiedRoomEventHandler(ChatRoom room);
}
