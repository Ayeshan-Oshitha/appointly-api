namespace Appointly.Application.Services.Admin.Contracts
{
    public class UserResponse
    {
        public Guid Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public IList<string> Roles { get; set; } = new List<string>();
    }
}
