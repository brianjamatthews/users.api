using System.Net.Mime;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Users.ApplicationCore.Commands;
using Users.ApplicationCore.Models;

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
        return Created("", user);
    }
}
