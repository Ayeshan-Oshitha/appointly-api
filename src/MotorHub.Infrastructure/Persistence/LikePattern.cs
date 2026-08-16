namespace MotorHub.Infrastructure.Persistence
{
    /// <summary>
    /// Helpers for building LIKE/ILIKE patterns out of user-supplied search terms.
    /// </summary>
    internal static class LikePattern
    {
        /// <summary>Escape character to pass as ILike's third argument.</summary>
        public const string EscapeCharacter = "\\";

        /// <summary>
        /// Escapes the LIKE metacharacters in a raw search term so it matches literally.
        /// </summary>
        /// <remarks>
        /// Without this, '%' and '_' typed by a user act as wildcards: searching "100%" matches
        /// every row beginning "100", and a lone "%" matches the entire table. The backslash is
        /// escaped first, otherwise it would double-escape the sequences added after it.
        /// </remarks>
        public static string EscapeWildcards(string term)
        {
            return term
                .Replace("\\", "\\\\")
                .Replace("%", "\\%")
                .Replace("_", "\\_");
        }
    }
}
