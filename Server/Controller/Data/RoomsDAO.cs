using Model.Entities;

namespace Controller.Data
{
    /// <summary>
    /// DAO para acceder y manipular las salas creadas por usuarios.
    /// </summary>
    public class RoomsDAO
    {
        private static RoomsDAO _instance;

        public static RoomsDAO Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new RoomsDAO();
                return _instance;
            }
        }
        private RoomsDAO() { }
        public void Add(ChatRoom r)
        {
            throw new NotImplementedException();
        }

        public bool Exists(string roomname)
        {
            throw new NotImplementedException();
        }

        public bool TryAdd(ChatRoom r)
        {
            throw new NotImplementedException();
        }

        public ChatRoom GetRoom(string roomname)
        {
            throw new NotImplementedException();
        }

        public ChatRoom? GetRoomOrNull(string roomname)
        {
            throw new NotImplementedException();
        }

    }
}
