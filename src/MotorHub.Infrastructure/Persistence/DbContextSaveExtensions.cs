using MotorHub.Domain.Infrastructure.Exceptions;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace MotorHub.Infrastructure.Persistence
{
    internal static class DbContextSaveExtensions
    {
        /// <summary>
        /// Saves, converting a unique-index violation into a <see cref="ConflictException"/>.
        /// </summary>
        /// <remarks>
        /// Slug uniqueness is checked in the service layer before the write, but that check and
        /// this save are separate round trips, and updates can also collide with a row the check
        /// never saw. The unique index is what actually guarantees uniqueness; without this the
        /// caller would get a 500 instead of the 409 the pre-check produces.
        /// </remarks>
        public static async Task SaveChangesOrConflictAsync(
            this MotorHubDbContext dbContext,
            string conflictMessage)
        {
            try
            {
                await dbContext.SaveChangesAsync();
            }
            catch (DbUpdateException ex) when (ex.InnerException is PostgresException pgEx
                && pgEx.SqlState == PostgresErrorCodes.UniqueViolation)
            {
                throw new ConflictException(conflictMessage);
            }
        }
    }
}
