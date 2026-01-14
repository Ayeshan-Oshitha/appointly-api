namespace Appointly.Application.Common.Interfaces.Services
{
    public interface IJwtTokenGenerator
    {
        Task<string> GenerateAccessToken(Guid userId, Guid identityUserId, string firstName, string lastName, string email);
    }
}
