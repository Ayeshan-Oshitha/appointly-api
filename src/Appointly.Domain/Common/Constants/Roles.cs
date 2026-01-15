namespace Appointly.Domain.Common.Constants
{
    public static class Roles
    {
        public const string Admin = "Admin";
        public const string User = "User";
        public const string Seller = "Seller";
        public const string SuperAdmin = "SuperAdmin";

        public static readonly string[] All =
        {
            Admin,
            User,   
            Seller,
            SuperAdmin
        };
    }
}
