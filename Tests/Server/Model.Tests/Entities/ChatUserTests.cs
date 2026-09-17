using Model.Definitions.Enums;
using Model.Entities;

namespace Model.Tests.Entities
{
    public class ChatUserTests
    {
        #region Validación de construcción

        [Theory]
        [InlineData("")]
        [InlineData("123456789")]
        public void UsernameOutOfRangeInConstructorThrowsArgumentOutOfRangeException(string username)
        {
            Assert.Throws<ArgumentOutOfRangeException>(
                () =>
                { ChatUser user = new(username); });
        }

        [Fact]
        public void UsernameNullInConstructorThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(
                () =>
                { ChatUser user = new(null); });
        }

        [Fact]
        public void ChatUserFieldsAreConsistentWithConstructor()
        {
            ChatUser user = new("Juan");

            Assert.Equal("Juan", user.Username);
            
        }
        [Fact]
        public void ChatUserStartsActiveAfterConstructor()
        {
            ChatUser user = new("Juan");

            Assert.Equal(UserStatus.Active, user.Status);

        }
        #endregion

        #region Validación de campos y propiedades

        [Theory]
        [InlineData("")]
        [InlineData("123456789")]
        public void SetUsernameOutOfRangeThrowsArgumentOutOfRangeException(string username)
        {
            ChatUser user = new("Juan");

            Assert.Throws<ArgumentOutOfRangeException>(
                () =>
                { user.Username = username; });
        }

        [Fact]
        public void SetUsernameNullThrowsArgumentNullException()
        {
            ChatUser user = new("Juan");
            Assert.Throws<ArgumentNullException>(
                () =>
                { user.Username = null; });
        }

        [Fact]
        public void SetStatusIsConsistent()
        {
            ChatUser user = new("Juan");

            user.Status = UserStatus.Away;

            Assert.Equal(UserStatus.Away, user.Status);
        }

        [Fact]
        public void SetTheSameStatusThrowsInvalidOperationException()
        {
            ChatUser user = new("Juan") { Status = UserStatus.Away };

            Assert.Throws<InvalidOperationException>(
                () => { user.Status = UserStatus.Away; });
        }

        #endregion
    }
}
