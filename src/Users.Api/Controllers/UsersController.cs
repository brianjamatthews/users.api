using System.ComponentModel.DataAnnotations;
using System.Net.Mime;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Users.ApplicationCore.Commands;
using Users.ApplicationCore.Models;
using Users.ApplicationCore.Queries;

namespace Users.Api.Controllers;

/// <summary>
/// User endpoints
/// </summary>
[Route("api/[controller]")]
[ApiController]
[Produces(MediaTypeNames.Application.Json)]
public class UsersController : ControllerBase
{
    private readonly IMediator _mediater;

    /// <summary>
    /// Instantiates a <see cref="UsersController"/>
    /// </summary>
    /// <param name="mediator">The <see cref="IMediator"/></param>
    public UsersController(IMediator mediator)
    {
        _mediater = mediator;
    }

    /// <summary>
    /// Creates a new user
    /// </summary>
    /// <param name="command">The <see cref="CreateUserCommand"/></param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/></param>
    /// <returns>The created user</returns>
    /// <remarks>
    /// Sample request:
    ///
    ///     POST /api/users
    ///     {
    ///        "firstName": "Brian",
    ///        "lastName": Matthews
    ///     }
    ///
    /// </remarks>
    /// <response code="201">Returns the newly created user</response>
    /// <response code="400">If the request is bad</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<UserReadModel>> Post(
        CreateUserCommand command,
        CancellationToken cancellationToken = default)
    {
        var user = await _mediater.Send(command, cancellationToken);
        return CreatedAtRoute(nameof(GetUser), new { emailAddress = user.emailAddress }, user);
    }

    /// <summary>
    /// Gets a user by email address
    /// </summary>
    /// <param name="emailAddress">The users email address to search by</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/></param>
    /// <returns>The user</returns>
    /// <response code="200">Returns the newly created user</response>
    /// <response code="400">If the request is bad</response>
    /// <response code="404">If the user isn't found</response>
    [HttpGet(Name = nameof(GetUser))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<UserReadModel>> GetUser(
        [EmailAddress]
        [StringLength(256)]
        string emailAddress,
        CancellationToken cancellationToken = default)
    {
        var query = new GetUserQuery(emailAddress);
        var user = await _mediater.Send(query, cancellationToken);

        if (user is null)
        {
            return NotFound();
        }

        return user;
    }
}
