using Microsoft.EntityFrameworkCore;
using Tripsafe.Users.Data.Models;

namespace Tripsafe.Users.Data;

public class TripsafeUserContext : DbContext
{
    public TripsafeUserContext(DbContextOptions<TripsafeUserContext> options)
        : base(options)
    {
    }

    public DbSet<User> User { get; set; } = default!;
}

