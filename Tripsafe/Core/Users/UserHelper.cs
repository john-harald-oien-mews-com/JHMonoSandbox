using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tripsafe.Users.Data;
using Tripsafe.Users.Data.Models;

namespace Tripsafe.Users.Service.Core.Users
{
    public class UserHelper : IUserHelper
    {
        private readonly TripsafeUserContext tripsafeUserContext;

        public UserHelper(TripsafeUserContext context)
        {
            tripsafeUserContext = context;
        }
        
        public async Task CreateUserAsync(User user)
        {
            tripsafeUserContext.User.Add(user);
            await tripsafeUserContext.SaveChangesAsync();
        }

        public async Task DeleteUserAsync(User user)
        {
            tripsafeUserContext.User.Remove(user);
            await tripsafeUserContext.SaveChangesAsync();
        }

        public async Task<User?> GetUserAsync(Guid id)
        {
            return await tripsafeUserContext.User.FindAsync(id);
        }

        public async Task<IEnumerable<User>> GetUsersAsync()
        {
            return await tripsafeUserContext.User.ToListAsync();
        }

        public async Task UpdateUserAsync(User user)
        {
            tripsafeUserContext.Entry(user).State = EntityState.Modified;
            await tripsafeUserContext.SaveChangesAsync();
        }

        public bool UserExists(Guid id)
        {
            return tripsafeUserContext.User.Any(e => e.Id == id);
        }
    }
}
