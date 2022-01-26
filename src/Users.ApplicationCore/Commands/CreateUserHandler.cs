using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Users.ApplicationCore.Entities;
using Users.ApplicationCore.Interfaces;
using Users.ApplicationCore.Models;

namespace Users.ApplicationCore.Commands;

/// <summary>
/// Handles a <see cref="CreateUserCommand"/>
/// </summary>
public class CreateUserHandler : IRequestHandler<CreateUserCommand, UserReadModel>
{
    private readonly IUsersDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly ILogger<CreateUserHandler> _logger;

    /// <summary>
    /// Instantiates a <see cref="CreateUserHandler"/>
    /// </summary>
    /// <param name="dbContext">The <see cref="IUsersDbContext"/></param>
    /// <param name="mapper">The <see cref="IMapper"/></param>
    /// <param name="logger">The <see cref="ILogger{TCategoryName}"/></param>
    public CreateUserHandler(
        IUsersDbContext dbContext,
        IMapper mapper,
        ILogger<CreateUserHandler> logger)
    {
        _dbContext = dbContext;
        _mapper = mapper;
        _logger = logger;
    }

    /// <summary>
    /// Creates a new user
    /// </summary>
    /// <param name="request">The <see cref="CreateUserCommand"/></param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/></param>
    /// <returns>The created user</returns>
    public async Task<UserReadModel> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var user = _mapper.Map<User>(request);
        _dbContext.Users.Add(user);

        await _dbContext.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Created user with id {UserId}", user.Id);

        return _mapper.Map<UserReadModel>(user);
    }
}
