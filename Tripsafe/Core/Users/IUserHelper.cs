using Tripsafe.Users.Data.Models;

namespace Tripsafe.Users.Service.Core.Users
{
    public interface IUserHelper
    {
        Task<IEnumerable<User>> GetUsersAsync();
        Task<User> GetUserAsync(Guid id);
        Task CreateUserAsync(User user);
        Task UpdateUserAsync(User user);
        Task DeleteUserAsync(User user);
        bool UserExists(Guid id);
    }
}
