namespace Appointly.Application.Services.Authentication.Contracts
{
    public class LoginResponse
    {
        public required Guid UserId { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Email { get; set; }
        public required string Token { get; set; }
    }
}
