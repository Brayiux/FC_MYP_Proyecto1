namespace Model.Entities
{
    /// <summary>
    /// Contiene y valida la información de una sala del chat, así
    /// como sus miembros, invitaciones y otra información que se
    /// requiera.
    /// </summary>
    public class ChatRoom
    {

        #region Campos

        private readonly Dictionary<string, ChatUser> _members = [];

        #endregion

        #region Propiedades
        public string Name { get; set; }

        public ChatUser Owner { get; }

        public IReadOnlyDictionary<string, ChatUser> Members => _members;

        #endregion

        #region Construcción

        public ChatRoom(string name, ChatUser owner)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Acceso Público

        public void Add(ChatUser cu)
        {
            throw new NotImplementedException();
        }

        public void Remove(string username)
        {
            throw new NotImplementedException();
        }

        public void AddInvitation(ChatUser cu)
        {
            throw new NotImplementedException();
        }

        public bool RemoveInvitation(string username)
        {
            throw new NotImplementedException();
        }

        public bool ConsumeInvitation(string username)
        {
            throw new NotImplementedException();
        }

        public void ClearInvitations()
        {
            throw new NotImplementedException();
        }

        public bool IsMember()
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
