using System.Net;
using System.Net.Mime;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Users.ApplicationCore.Commands;
using Users.ApplicationCore.Models;
using Xunit;

namespace Users.IntegrationTests.Controllers;

public class GetUserShould : IClassFixture<WebApplicationFactory<Program>>
{
    private const string RequestUri = "/api/users";
    private readonly WebApplicationFactory<Program> _factory;
    private readonly HttpClient _client;
    private readonly JsonSerializerOptions _jsonSerializerOptions;

    public GetUserShould(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
        _jsonSerializerOptions = new JsonSerializerOptions()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
    }

    [Fact]
    public async Task ReturnOk()
    {
        var command = new CreateUserCommand("brian", "james", "matthews", "(555) 555-5555", $"{Guid.NewGuid()}@mydomain.com");
        var json = JsonSerializer.Serialize(command, _jsonSerializerOptions);
        using var content = new StringContent(json, Encoding.UTF8, MediaTypeNames.Application.Json);

        var postResponse = await _client.PostAsync(RequestUri, content);

        Assert.Equal(HttpStatusCode.Created, postResponse.StatusCode);

        var getResonse = await _client.GetAsync($"{RequestUri}?emailAddress={command.emailAddress}");
        var responseContent = await getResonse.Content.ReadAsStreamAsync();
        var actual = JsonSerializer.Deserialize<UserReadModel>(responseContent);

        Assert.Equal(HttpStatusCode.OK, getResonse.StatusCode);

        Assert.NotNull(actual);
        Assert.NotEqual(actual!.id, Guid.Empty);
        Assert.Equal(command.firstName, actual.firstName);
        Assert.Equal(command.middleName, actual.middleName);
        Assert.Equal(command.lastName, actual.lastName);
        Assert.Equal($"{command.firstName} {command.middleName} {command.lastName}".Trim(), actual.Name);
        Assert.Equal(command.phoneNumber, actual.phoneNumber);
        Assert.Equal(command.emailAddress, actual.emailAddress);
    }


    [Theory]
    [InlineData(null)]
    [InlineData("not a valid email address")]
    [InlineData("BrianBrianBrianBrianBrianBrianBrianBrianBrianBrianBrianBrianBrianBrianBrianBrianBrianBrianBrianBrianBrianBrianBrianBrianBrianBrianBrianBrianBrianBrianBrianBrianBrianBrianBrianBrianBrianBrianBrianBrianBrianBrianBrianBrianBrianBrianBrianBrianBrianBrianBrianBrian@mydomain.com")]
    public async Task ReturnBadRequest(string? emailAddress)
    {
        var response = await _client.GetAsync($"{RequestUri}?emailAddress={emailAddress}");

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]

    public async Task ReturnsNotFound()
    {
        var response = await _client.GetAsync($"{RequestUri}?emailAddress=myamazingemail@mydomain.com");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}
