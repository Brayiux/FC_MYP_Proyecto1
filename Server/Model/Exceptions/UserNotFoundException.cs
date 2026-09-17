namespace Model.Exceptions
{
    public class UserNotFoundException(string msg) : ChatException(msg);
}
