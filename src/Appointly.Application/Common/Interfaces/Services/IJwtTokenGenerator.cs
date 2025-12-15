namespace Appointly.Application.Common.Interfaces.Services
{
    public interface IJwtTokenGenerator
    {
        string GenerateAccessToken(Guid userId, string firstName, string lastName, string email);
    }
}
