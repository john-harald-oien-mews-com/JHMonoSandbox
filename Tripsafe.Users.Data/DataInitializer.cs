using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tripsafe.Users.Data;

public static class DataInitializer
{
    public static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {

        var connectionString = configuration?.GetConnectionString("TripsafeUserContext");
        if (!string.IsNullOrWhiteSpace(connectionString))
        {
            services.AddDbContext<TripsafeUserContext>(options =>
                options.UseSqlServer(connectionString));
        }
        else
        {
            services.AddDbContext<TripsafeUserContext>(j => j.UseSqlServer("Server=.\\SQLExpress;Database=Tripsafe.Users;User Id=sa;Password=Vinter001;Trusted_Connection=false;MultipleActiveResultSets=true;Trust Server Certificate=true "));
        }

        var serviceProvider = services.BuildServiceProvider();
        using (var scope = serviceProvider.CreateScope())
        {
            UpdateDatabase(scope.ServiceProvider);
        }
        
    }

    private static void UpdateDatabase(IServiceProvider serviceProvider)
    {
        var context = serviceProvider.GetRequiredService<TripsafeUserContext>();
        context.Database.Migrate();
    }
}
