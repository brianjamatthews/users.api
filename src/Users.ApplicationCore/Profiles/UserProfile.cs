using AutoMapper;
using Users.ApplicationCore.Commands;
using Users.ApplicationCore.Entities;
using Users.ApplicationCore.Models;

namespace Users.ApplicationCore.Profiles;

/// <summary>
/// Profile for user mappings
/// </summary>
public class UserProfile : Profile
{
    /// <summary>
    /// Instantiates a <see cref="UserProfile"/>
    /// </summary>
    public UserProfile()
    {
        CreateMap<CreateUserCommand, User>(MemberList.Source);
        CreateMap<User, UserReadModel>(MemberList.Destination);
    }
}
