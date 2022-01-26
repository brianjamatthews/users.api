using System.ComponentModel.DataAnnotations;

namespace Users.ApplicationCore.Entities;

/// <summary>
/// Person who uses the application
/// </summary>
public class User
{
    /// <summary>
    /// Instantiates a <see cref="User"/>
    /// </summary>
    /// <param name="firstName">The person's first name</param>
    /// <param name="lastName">The person's last name</param>
    public User(string firstName, string lastName)
    {
        FirstName = firstName;
        LastName = lastName;
    }

    /// <summary>
    /// Unique identifer
    /// </summary>
    /// <example>54c4e684-0a6a-449d-b445-61ddd12ffd3d</example>
    public Guid Id { get; set; }

    /// <summary>
    /// First name
    /// </summary>
    /// <example>Matthew</example>
    [Required]
    [StringLength(256)]
    public string FirstName { get; set; }

    /// <summary>
    /// Middle name
    /// </summary>
    /// <example>Decker</example>
    [StringLength(256)]
    public string? MiddleName { get; set; }

    /// <summary>
    /// Last name
    /// </summary>
    /// <example>Lund</example>
    [Required]
    [StringLength(256)]
    public string LastName { get; set; }

    /// <summary>
    /// Phone number
    /// </summary>
    /// <example>555-555-5555</example>
    [Phone]
    [StringLength(64)]
    public string? PhoneNumber { get; set; }

    /// <summary>
    /// Email address
    /// </summary>
    /// <example>matt@awesomedomain.com</example>
    [EmailAddress]
    [StringLength(256)]
    public string? EmailAddress { get; set; }
}
