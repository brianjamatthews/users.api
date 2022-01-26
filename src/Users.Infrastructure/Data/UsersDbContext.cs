using Microsoft.EntityFrameworkCore;
using Users.ApplicationCore.Entities;
using Users.ApplicationCore.Interfaces;

namespace Users.Infrastructure.Data;

/// <summary>
/// Users db context
/// </summary>
public class UsersDbContext : DbContext, IUsersDbContext
{
    /// <summary>
    /// Instantiates a <see cref="UsersDbContext"/>
    /// </summary>
    /// <param name="options">The <see cref="DbContextOptions{TContext}"/></param>
    public UsersDbContext(DbContextOptions<UsersDbContext> options)
        : base(options)
    {
    }

    /// <inheritdoc/>
    public DbSet<User> Users => Set<User>();
}
