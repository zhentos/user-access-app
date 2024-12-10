using AutoMapper;
using UserAccessApp.WebApi.Dtos;
using UserAccessApp.WebApi.Models;

namespace UserAccessApp.WebApi.Mapping;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<User, UserDto>()
             .ForMember(d => d.UserName,
                        opt => opt.MapFrom(s => $"{s.FirstName} {s.LastName}"));
    }
}
