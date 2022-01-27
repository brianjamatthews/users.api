using MediatR;
using Users.ApplicationCore.Models;

namespace Users.ApplicationCore.Queries;

/// <summary>
/// Get user query
/// </summary>
/// <param name="emailAddress">The user's email address to search by</param>
public record GetUserQuery(string emailAddress) : IRequest<UserReadModel?>;
