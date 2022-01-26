using Microsoft.EntityFrameworkCore;
using Users.ApplicationCore.Entities;

namespace Users.ApplicationCore.Interfaces;

/// <summary>
/// User db context
/// </summary>
public interface IUsersDbContext : IDbContext
{
    /// <summary>
    /// Set of users
    /// </summary>
    DbSet<User> Users { get; }
}
