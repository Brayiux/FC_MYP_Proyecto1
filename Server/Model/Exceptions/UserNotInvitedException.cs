namespace Model.Exceptions
{
    public class UserNotInvitedException(string msg) : ChatException(msg);
}
