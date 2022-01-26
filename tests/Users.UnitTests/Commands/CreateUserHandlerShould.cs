using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Users.ApplicationCore.Commands;
using Users.ApplicationCore.Profiles;
using Users.Infrastructure.Data;
using Xunit;

namespace Users.UnitTests.Commands;

public sealed class CreateUserHandlerShould : IDisposable
{
    private readonly UsersDbContext _dbContext;
    private readonly CreateUserHandler _handler;

    public CreateUserHandlerShould()
    {
        var options = new DbContextOptionsBuilder<UsersDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        _dbContext = new UsersDbContext(options);

        var config = new MapperConfiguration(config => config.AddProfile<UserProfile>());
        var mapper = new Mapper(config);

        var logger = Mock.Of<ILogger<CreateUserHandler>>();

        _handler = new CreateUserHandler(_dbContext, mapper, logger);
    }

    public void Dispose()
    {
        _dbContext.Dispose();
    }

    [Theory]
    [InlineData("Brian", null, "Matthews", null, null)]
    [InlineData("Brian", "James", "Matthews", null, null)]
    [InlineData("Brian", "James", "Matthews", "(555) 555-5555", null)]
    [InlineData("Brian", "James", "Matthews", "(555) 555-5555", "brianmatthews@myawesomedomain.com")]
    public async Task Succeed(
        string firstName,
        string? middleName,
        string lastName,
        string? phoneNumber,
        string? emailAddress)
    {
        var command = new CreateUserCommand(firstName, middleName, lastName, phoneNumber, emailAddress);

        var actual = await _handler.Handle(command, default);

        Assert.NotEqual(actual.id, Guid.Empty);
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
    public async Task ThrowDbUpdateException(
        string firstName,
        string? middleName,
        string lastName,
        string? phoneNumber,
        string? emailAddress)
    {
        var command = new CreateUserCommand(firstName, middleName, lastName, phoneNumber, emailAddress);

        await Assert.ThrowsAsync<DbUpdateException>(() => _handler.Handle(command, default));
    }
}
