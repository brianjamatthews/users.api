using System.Net;
using System.Net.Mime;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Users.ApplicationCore.Commands;
using Users.ApplicationCore.Models;
using Xunit;

namespace Users.IntegrationTests.Controllers;

public class PostUserShould : IClassFixture<WebApplicationFactory<Program>>
{
    private const string RequestUri = "/api/users";
    private readonly WebApplicationFactory<Program> _factory;
    private readonly HttpClient _client;
    private readonly JsonSerializerOptions _jsonSerializerOptions;

    public PostUserShould(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
        _jsonSerializerOptions = new JsonSerializerOptions()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
    }

    [Theory]
    [InlineData("Brian", null, "Matthews", null, null)]
    [InlineData("Brian", "James", "Matthews", null, null)]
    [InlineData("Brian", "James", "Matthews", "(555) 555-5555", null)]
    [InlineData("Brian", "James", "Matthews", "(555) 555-5555", "brianmatthews@myawesomedomain.com")]
    public async Task ReturnCreated(
        string firstName,
        string? middleName,
        string lastName,
        string? phoneNumber,
        string? emailAddress)
    {
        var command = new CreateUserCommand(firstName, middleName, lastName, phoneNumber, emailAddress);
        var json = JsonSerializer.Serialize(command, _jsonSerializerOptions);
        using var content = new StringContent(json, Encoding.UTF8, MediaTypeNames.Application.Json);

        var response = await _client.PostAsync(RequestUri, content);
        var responseContent = await response.Content.ReadAsStreamAsync();
        var actual = JsonSerializer.Deserialize<UserReadModel>(responseContent);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);

        Assert.NotNull(actual);
        Assert.NotEqual(actual!.id, Guid.Empty);
        Assert.Equal(firstName, actual.firstName);
        Assert.Equal(middleName, actual.middleName);
        Assert.Equal(lastName, actual.lastName);
        Assert.Equal($"{firstName} {middleName} {lastName}".Trim(), actual.Name);
        Assert.Equal(phoneNumber, actual.phoneNumber);
        Assert.Equal(emailAddress, actual.emailAddress);
    }

    [Theory]
    [InlineData(null, null, null, null, null)]
    [InlineData("Brian", null, null, null, null)]
    [InlineData(null, null, "Matthews", null, null)]
    [InlineData("BrianBrianBrianBrianBrianBrianBrianBrianBrianBrianBrianBrianBrianBrianBrianBrianBrianBrianBrianBrianBrianBrianBrianBrianBrianBrianBrianBrianBrianBrianBrianBrianBrianBrianBrianBrianBrianBrianBrianBrianBrianBrianBrianBrianBrianBrianBrianBrianBrianBrianBrianBrian", null, "Matthews", null, null)]
    [InlineData("Brian", "BrianBrianBrianBrianBrianBrianBrianBrianBrianBrianBrianBrianBrianBrianBrianBrianBrianBrianBrianBrianBrianBrianBrianBrianBrianBrianBrianBrianBrianBrianBrianBrianBrianBrianBrianBrianBrianBrianBrianBrianBrianBrianBrianBrianBrianBrianBrianBrianBrianBrianBrianBrian", "Matthews", null, null)]
    [InlineData("Brian", null, "BrianBrianBrianBrianBrianBrianBrianBrianBrianBrianBrianBrianBrianBrianBrianBrianBrianBrianBrianBrianBrianBrianBrianBrianBrianBrianBrianBrianBrianBrianBrianBrianBrianBrianBrianBrianBrianBrianBrianBrianBrianBrianBrianBrianBrianBrianBrianBrianBrianBrianBrianBrian", null, null)]
    [InlineData("Brian", null, "Matthews", "Not a phone number", null)]
    [InlineData("Brian", null, "Matthews", null, "Not an email address")]
    public async Task ReturnBadRequest(
        string firstName,
        string? middleName,
        string lastName,
        string? phoneNumber,
        string? emailAddress)
    {
        var command = new CreateUserCommand(firstName, middleName, lastName, phoneNumber, emailAddress);
        var json = JsonSerializer.Serialize(command, _jsonSerializerOptions);
        using var content = new StringContent(json, Encoding.UTF8, MediaTypeNames.Application.Json);

        var response = await _client.PostAsync(RequestUri, content);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
}
