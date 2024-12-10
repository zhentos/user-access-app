using UserAccessApp.WebApi.Dtos;
using UserAccessApp.WebApi.Models;

namespace UserAccessApp.WebApi.Repositories
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAll();
        Task<User> GetByEmail(string email);
        Task<int> UpdateUsersBatch(IEnumerable<UserUpdateDto> updates);
    }
}
