using System.ComponentModel.DataAnnotations;

namespace MotorHub.Infrastructure.Services.Constants
{
    public class JwtSettings
    {
        public const string SectionName = "JwtSettings";

        /// <summary>
        /// HMAC-SHA256 signing key. Must be at least 32 characters (256 bits).
        /// Supply via user secrets or the JwtSettings__Secret environment variable.
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessage = "JwtSettings:Secret is required.")]
        [MinLength(32, ErrorMessage = "JwtSettings:Secret must be at least 32 characters (256 bits) for HMAC-SHA256.")]
        public string Secret { get; init; } = null!;

        [Required(AllowEmptyStrings = false, ErrorMessage = "JwtSettings:Issuer is required.")]
        public string Issuer { get; init; } = null!;

        [Required(AllowEmptyStrings = false, ErrorMessage = "JwtSettings:Audience is required.")]
        public string Audience { get; init; } = null!;

        [Range(1, 1440, ErrorMessage = "JwtSettings:ExpiryMinutes must be between 1 and 1440.")]
        public int ExpiryMinutes { get; init; }
    }
}
