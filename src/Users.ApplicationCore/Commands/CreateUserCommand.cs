using System.ComponentModel.DataAnnotations;
using MediatR;
using Users.ApplicationCore.Models;

namespace Users.ApplicationCore.Commands;

/// <summary>
/// Command to create a new user
/// </summary>
/// <param name="firstName">First name</param>
/// <param name="middleName">Middle name</param>
/// <param name="lastName">Last name</param>
/// <param name="phoneNumber">Phone number</param>
/// <param name="emailAddress">Email address</param>
public record CreateUserCommand(
    [Required]
    [StringLength(256)]
    string firstName,
    [StringLength(256)]
    string? middleName,
    [Required]
    [StringLength(256)]
    string lastName,
    [Phone]
    [StringLength(64)]
    string? phoneNumber,
    [EmailAddress]
    [StringLength(256)]
    string? emailAddress) : IRequest<UserReadModel>;
