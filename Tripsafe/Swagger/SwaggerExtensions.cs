using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Tripsafe.Users.Service.Swagger;

public static class SwaggerExtensions
{
    public static IServiceCollection AddSwaggerFeatures(this IServiceCollection services)
    {
        services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
        services.AddSwaggerGen();

        return services;
    }

    public static IApplicationBuilder UseSwaggerFeatures(this IApplicationBuilder app, IConfiguration config,
        IWebHostEnvironment env)
    {
        if (!env.IsDevelopment())
        {
            return app;
        }

        var clientId = config["Auth0:ClientId"]??"";
        app
            .UseSwagger()
            .UseSwaggerUI(options =>
            {
                options.DocumentTitle = "Tripsafe Documentation";
                options.OAuthClientId(clientId);
                options.OAuthAppName("TripsafeSAP");
                options.OAuthUsePkce();
                options.OAuthAdditionalQueryStringParams(new Dictionary<string, string>
                {
                    { "audience", config["Auth0:Audience"]??"" }
                });
            });

        return app;
    }
}

