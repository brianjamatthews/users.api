using Microsoft.EntityFrameworkCore;

namespace Users.Infrastructure.Data;

/// <summary>
/// Users db context
/// </summary>
public class UsersDbContext : DbContext
{
    /// <summary>
    /// Instantiates a <see cref="UsersDbContext"/>
    /// </summary>
    /// <param name="options">The <see cref="DbContextOptions{TContext}"/></param>
    public UsersDbContext(DbContextOptions<UsersDbContext> options)
        : base(options)
    {
    }
}
