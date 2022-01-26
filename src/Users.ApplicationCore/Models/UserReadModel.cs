namespace Users.ApplicationCore.Models;

/// <summary>
/// User read model
/// </summary>
/// <param name="id">Unique identifer</param>
/// <param name="firstName">First name</param>
/// <param name="middleName">Middle name</param>
/// <param name="lastName">Last name</param>
/// <param name="phoneNumber">Phone number</param>
/// <param name="emailAddress">Email address</param>
public record UserReadModel(
    Guid id,
    string firstName,
    string? middleName,
    string lastName,
    string? phoneNumber,
    string? emailAddress)
{
    /// <summary>
    /// Full name
    /// </summary>
    public string Name => $"{firstName} {middleName} {lastName}".Trim();
}
