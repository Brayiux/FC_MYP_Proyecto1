namespace Model.Exceptions
{
    public class UserAlreadyExistsException(string msg) : ChatException(msg); 
}
