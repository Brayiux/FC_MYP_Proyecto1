namespace Model.Exceptions
{
    public class RoomNotFoundException(string msg) : ChatException(msg);
}
