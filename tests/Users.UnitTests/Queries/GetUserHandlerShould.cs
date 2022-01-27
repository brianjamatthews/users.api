using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Users.ApplicationCore.Entities;
using Users.ApplicationCore.Profiles;
using Users.ApplicationCore.Queries;
using Users.Infrastructure.Data;
using Xunit;

namespace Users.UnitTests.Queries;

public sealed class GetUserHandlerShould : IDisposable
{
    private readonly UsersDbContext _dbContext;
    private readonly GetUserHandler _handler;
    private readonly User _user;

    public GetUserHandlerShould()
    {
        var options = new DbContextOptionsBuilder<UsersDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        _dbContext = new UsersDbContext(options);

        var config = new MapperConfiguration(config => config.AddProfile<UserProfile>());
        var mapper = new Mapper(config);

        var logger = Mock.Of<ILogger<GetUserHandler>>();

        _handler = new GetUserHandler(_dbContext, mapper, logger);

        var user = new User("Brian", "Matthews")
        {
            MiddleName = "James",
            PhoneNumber = "(555) 555-5555",
            EmailAddress = "brianmatthews@myawesomedomain.com"
        };
        _dbContext.Users.Add(user);
        _dbContext.SaveChanges();

        _user = user;
    }

    public void Dispose()
    {
        _dbContext.Dispose();
    }

    [Fact]
    public async Task ReturnUser()
    {
        var query = new GetUserQuery(_user.EmailAddress!);

        var actual = await _handler.Handle(query, default);

        Assert.NotNull(actual);
        Assert.Equal(_user.Id, actual!.id);
        Assert.Equal(_user.FirstName, actual.firstName);
        Assert.Equal(_user.MiddleName, actual.middleName);
        Assert.Equal(_user.LastName, actual.lastName);
        Assert.Equal($"{_user.FirstName} {_user.MiddleName} {_user.LastName}".Trim(), actual.Name);
        Assert.Equal(_user.PhoneNumber, actual.phoneNumber);
        Assert.Equal(_user.EmailAddress, actual.emailAddress);
    }

    [Fact]
    public async Task ReturnNull()
    {
        var query = new GetUserQuery("test@mydomain.com");

        var actual = await _handler.Handle(query, default);

        Assert.Null(actual);
    }
}
