namespace Appointly.Domain.Entities
{
    public class User
    {
        public Guid Id { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string Email { get; private set; }
        public string PasswordHash { get; private set; }

        private User() { }

        public User(string firstName, string lastName, string email, string passwordHash)
        {
            Id = Guid.NewGuid();
            FirstName = firstName;
            LastName = lastName;
            Email = email.ToLower();
            PasswordHash = passwordHash;
        }

        public void UpdatePassword(string newPasswordHash)
        {
            if(string.IsNullOrWhiteSpace(newPasswordHash))
            {
                throw new ArgumentException("New password hash cannot be null or empty.");
            }
            PasswordHash = newPasswordHash;
        }
    }
}
