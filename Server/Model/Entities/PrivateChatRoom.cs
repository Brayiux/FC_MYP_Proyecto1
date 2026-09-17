namespace Model.Entities
{
    /// <summary>
    /// Contiene y valida la información de una sala del chat privada,
    /// (entre dos usuarios).
    /// </summary>
    public class PrivateChatRoom
    {

        #region Campos

        private readonly Dictionary<string, ChatUser> _members = new(capacity: 2);

        #endregion

        #region Propiedades
        public IReadOnlyDictionary<string, ChatUser> Members => _members;

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
