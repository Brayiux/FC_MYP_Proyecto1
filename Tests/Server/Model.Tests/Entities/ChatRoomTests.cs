using Model.Entities;
using Model.Exceptions;

namespace Model.Tests.Entities
{
    public class ChatRoomTests
    {
        private ChatUser _owner = new("Juan");


        #region Validación de construcción

        [Theory]
        [InlineData("")]
        [InlineData("12345678901234567")]
        public void RoomNameOutOfRangeThrowsArgumentOutOfRangeException(string roomname)
        {
            Assert.Throws<RoomnameOutOfRangeException>(
                () => { ChatRoom r = new(roomname, _owner); });
        }

        [Fact]
        public void RoomNameNullInConstructorThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(
                () => { ChatRoom r = new(null, _owner); });
        }

        [Fact]
        public void OwnerNullInConstructorThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(
                () => { ChatRoom r = new("Sala", null); });
        }

        [Fact]
        public void ChatRoomFieldsAreConsistentWithConstructor()
        {
            ChatRoom r = new("Sala", _owner);
            Assert.True((r.Roomname.Equals("Sala")) &&
                (_owner.Equals(r.Owner)));
        }

        #endregion

        #region Validación de campos y propiedades

        [Theory]
        [InlineData("")]
        [InlineData("12345678901234567")]
        public void SetRoomNameOutOfRangeThrowsArgumentOutOfRangeException(string roomname)
        {
            ChatRoom r = new("Sala", _owner);

            Assert.Throws<RoomnameOutOfRangeException>(
                () => { r.Roomname = roomname; });
        }

        [Fact]
        public void SetRoomnameIsConsistent()
        {
            ChatRoom r = new("Sala", _owner);

            Assert.Equal("Sala", r.Roomname);
        }

        #endregion

        #region Validación al añadir y eliminar miembros


        [Fact]
        public void TryAddUserNotInvitedToARoomThrowsUserNotInvitedException()
        {
            ChatRoom r = new("Sala", _owner);
            ChatUser u = new("Pedro");

            Assert.Throws<UserNotInvitedException>(
                () => { r.Add("Pedro"); });
        }

        [Fact]
        public void AddUserIsConsistent()
        {
            ChatRoom r = new("Sala", _owner);
            ChatUser u = new("Pedro");
            r.AddInvitation(u);
            r.Add("Pedro");

            Assert.True(r.Members.TryGetValue("Pedro", out ChatUser p) && p.Equals(u));

        }


        [Fact]
        public void TryRemoveUserNotMemberFromARoomThrowsUserNotFoundException()
        {
            ChatRoom r = new("Sala", _owner);

            Assert.Throws<UserNotFoundException>(
                () => { r.Remove("Pedro"); });

        }

        [Fact]
        public void RemoveUserIsConsistent()
        {
            ChatRoom r = new("Sala", _owner);
            r.AddInvitation(new ChatUser("Pedro"));
            r.Add("Pedro");
            r.Remove("Pedro");

            Assert.False(r.Members.ContainsKey("Pedro"));
        }

        [Fact]
        public void RemoveInvitationIsConsistent()
        {
            ChatRoom r = new("Sala", _owner);
            ChatUser u = new("Pedro");

            r.AddInvitation(u);

            bool wasRemoved = r.RemoveInvitation(u.Username);
            bool couldntBeAdded = false;
            
            try
            {
                r.Add(u.Username);
            }
            catch (UserNotInvitedException e)
            {
                couldntBeAdded = true;
            }

            Assert.True(wasRemoved && couldntBeAdded);
        }

        [Fact]
        public void IsInvitedIsConsistent()
        {
            ChatRoom r = new("Sala", _owner);
            ChatUser u = new("Pedro");

            r.AddInvitation(u);

            Assert.True(r.IsInvited("Pedro"));
        }

        [Fact]
        public void OwnerIsTheFirstMemberInARoom()
        {
            ChatRoom r = new("Sala", _owner);

            Assert.True(r.IsMember(_owner.Username));
        }

        #endregion
    }
}
