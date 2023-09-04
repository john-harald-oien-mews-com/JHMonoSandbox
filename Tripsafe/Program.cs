using Tripsafe.Users.Data;
using Tripsafe.Users.Service.Core.Users;
using Tripsafe.Users.Service.Swagger;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerFeatures();

builder.Services.AddAuthentication("Bearer").AddJwtBearer(options =>
{
    options.Authority = builder.Configuration["Auth0:Domain"] ?? "https://dev-l0vjntsvdd6wh811.us.auth0.com/";
    options.Audience = builder.Configuration["Auth0:Audience"] ?? "https://TripsafeApiIdentifier.local";
});

builder.Services.AddTransient<IUserHelper, UserHelper>();

DataInitializer.ConfigureServices(builder.Services, builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwaggerFeatures(app.Configuration, app.Environment);   
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
