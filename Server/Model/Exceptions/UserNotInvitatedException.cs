namespace Model.Exceptions
{
    public class UserNotInvitatedException(string msg) : ChatException(msg);
}
