using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Users.ApplicationCore.Interfaces;
using Users.ApplicationCore.Models;

namespace Users.ApplicationCore.Queries;

/// <summary>
/// Handles a <see cref="GetUserQuery"/>
/// </summary>
public class GetUserHandler : IRequestHandler<GetUserQuery, UserReadModel?>
{
    private readonly IUsersDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly ILogger<GetUserHandler> _logger;

    /// <summary>
    /// Instantiates a <see cref="GetUserHandler"/>
    /// </summary>
    /// <param name="dbContext">The <see cref="IUsersDbContext"/></param>
    /// <param name="mapper">The <see cref="IMapper"/></param>
    /// <param name="logger">The <see cref="ILogger{TCategoryName}"/></param>
    public GetUserHandler(
        IUsersDbContext dbContext,
        IMapper mapper,
        ILogger<GetUserHandler> logger)
    {
        _dbContext = dbContext;
        _mapper = mapper;
        _logger = logger;
    }

    /// <summary>
    /// Gets a user by email address
    /// </summary>
    /// <param name="request">The <see cref="GetUserQuery"/></param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/></param>
    /// <returns>The user</returns>
    public async Task<UserReadModel?> Handle(GetUserQuery request, CancellationToken cancellationToken)
    {
        var user = await _dbContext.Users
            .Where(user => user.EmailAddress != null &&
                user.EmailAddress!.Equals(request.emailAddress, StringComparison.OrdinalIgnoreCase))
            .ProjectTo<UserReadModel>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(cancellationToken);

        if (user is not null)
        {
            _logger.LogInformation("Retrieved user with email address {EmailAddress}", user.emailAddress);
        }

        return user;
    }
}
