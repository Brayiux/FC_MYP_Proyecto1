using Model.Entities;
using Model.Exceptions;

namespace Model.Tests.Entities
{
    public class PrivateChatRoomTests
    {
        private readonly ChatUser _member1 = new("Juan");
        private readonly ChatUser _member2 = new("Pedro");

        #region Validación de construcción

        [Theory]
        [InlineData("")]
        [InlineData("12345678901234567")]
        public void RoomNameOutOfRangeThrowsRoomnameOutOfRangeException(string roomname)
        {
            Assert.Throws<RoomnameOutOfRangeException>(
                () => { PrivateChatRoom r = new(roomname, _member1, _member2); });
        }

        [Fact]
        public void RoomNameNullInConstructorThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(
                () => { PrivateChatRoom r = new(null, _member1, _member2); });
        }

        [Fact]
        public void SomeMemberNullInConstructorThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(
                () => { PrivateChatRoom r = new("Sala", null, _member2); });
        }

        [Fact]
        public void PrivateChatRoomFieldsAreConsistentWithConstructor()
        {
            PrivateChatRoom r = new("Sala", _member1, _member2);
            Assert.Equal("Sala", r.Roomname);
        }

        #endregion

        #region Validación de campos y propiedades

        [Theory]
        [InlineData("")]
        [InlineData("12345678901234567")]
        public void SetRoomNameOutOfRangeThrowsArgumentOutOfRangeException(string roomname)
        {
            PrivateChatRoom r = new("Sala", _member1, _member2);

            Assert.Throws<RoomnameOutOfRangeException>(
                () => { r.Roomname = roomname; });
        }

        [Fact]
        public void SetRoomnameIsConsistent()
        {
            PrivateChatRoom r = new("Sala", _member1, _member2);

            Assert.Equal("Sala", r.Roomname);
        }

        #endregion
    }
}
