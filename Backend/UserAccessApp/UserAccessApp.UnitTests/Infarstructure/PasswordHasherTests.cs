using UserAccessApp.Infrastructure;

namespace UserAccessApp.UnitTests.Infarstructure
{
    public class PasswordHasherTests
    {
        private readonly IPasswordHasher _passwordHasher;

        public PasswordHasherTests()
        {
            _passwordHasher = new PasswordHasher();
        }

        [Fact]
        public void HashPassword_ShouldReturnDifferentHashes_ForSamePassword()
        {
            // Arrange
            var password = "MySecurePassword";

            // Act
            var hash1 = _passwordHasher.HashPassword(password);
            var hash2 = _passwordHasher.HashPassword(password);

            // Assert
            Assert.NotEqual(hash1.hashedPassword, hash2.hashedPassword); // Different hashes due to different salts
        }

        [Fact]
        public void VerifyPassword_ShouldReturnTrue_ForCorrectPassword()
        {
            // Arrange
            var password = "MySecurePassword";
            var (hashedPassword, salt) = _passwordHasher.HashPassword(password);

            // Act
            var result = _passwordHasher.VerifyPassword(password, hashedPassword, salt);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void VerifyPassword_ShouldReturnFalse_ForIncorrectPassword()
        {
            // Arrange
            var password = "MySecurePassword";
            var wrongPassword = "WrongPassword";
            var (hashedPassword, salt) = _passwordHasher.HashPassword(password);

            // Act
            var result = _passwordHasher.VerifyPassword(wrongPassword, hashedPassword, salt);

            // Assert
            Assert.False(result);
        }
    }
}
