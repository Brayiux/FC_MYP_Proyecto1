using Model.Definitions.Enums;
using Model.Entities;
using Model.Exceptions;

namespace Model.Tests.Entities
{
    public class ChatUserTests
    {
        #region Validación de construcción

        [Theory]
        [InlineData("")]
        [InlineData("123456789")]
        public void UsernameOutOfRangeInConstructorThrowsUsernameOutOfRangeException(string username)
        {
            Assert.Throws<UsernameOutOfRangeException>(
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
        public void SetUsernameOutOfRangeThrowsUsernameOutOfRangeException(string username)
        {
            ChatUser user = new("Juan");

            Assert.Throws<UsernameOutOfRangeException>(
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
        public void SetTheSameStatusThrowsInvalidClientStatusException()
        {
            ChatUser user = new("Juan") { Status = UserStatus.Away };

            Assert.Throws<InvalidClientStatusException>(
                () => { user.Status = UserStatus.Away; });
        }

        #endregion
    }
}
