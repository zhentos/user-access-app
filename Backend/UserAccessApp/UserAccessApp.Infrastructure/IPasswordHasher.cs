namespace UserAccessApp.Infrastructure
{
    public interface IPasswordHasher
    {
        (string hashedPassword, string salt) HashPassword(string password);
        bool VerifyPassword(string enteredPassword, string storedHash, string storedSalt);
    }
}
