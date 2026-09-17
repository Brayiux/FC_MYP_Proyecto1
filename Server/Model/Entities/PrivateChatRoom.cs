namespace Model.Entities
{
    /// <summary>
    /// Contiene y valida la información de una sala del chat privada,
    /// (entre dos usuarios).
    /// </summary>
    public class PrivateChatRoom
    {

        #region Campos

        private readonly ChatUser[] _members = new ChatUser[2];

        #endregion

        #region Propiedades
        public IReadOnlyList<ChatUser> Members => _members;

        public string Roomname { get; set; }

        #endregion

        #region Construcción

        public PrivateChatRoom(string name)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
