
namespace MotorHub.Domain.Entities
{
    public class User
    {
        public Guid Id { get; private set; }
        public Guid IdentityUserId { get; private set; }
        public string FirstName { get; private set; } = null!;
        public string LastName { get; private set; } = null!;
        public string PhoneNumber { get; private set; } = null!;
        public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; private set; }

        private User() { }

        public User(
            Guid identityUserId,
            string firstName,
            string lastName,
            string phoneNumber
            )
        {
            Id = Guid.NewGuid();
            IdentityUserId = identityUserId;
            FirstName = firstName;
            LastName = lastName;
            PhoneNumber = phoneNumber;
        }
    }
}
