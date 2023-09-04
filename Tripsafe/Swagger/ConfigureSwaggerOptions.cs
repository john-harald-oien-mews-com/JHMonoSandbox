using IdentityModel.Client;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Tripsafe.Users.Service.Swagger;

public class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
{
    private readonly IConfiguration _config;
    
    public ConfigureSwaggerOptions(IConfiguration config)
    {
        _config = config;
    }

    public void Configure(SwaggerGenOptions options)
    {
        var disco = GetDiscoveryDocument();

        /*************************************************************
         * This is needed for scopes or privileges comming from Auth0
         * Comment this in to use scopes / privileges and comment out the following AddSecurityDefinition

        var apiScope = _config.GetValue<string>("Auth0:Audience");
        var scopes = apiScope?.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).ToList();

        var addScopes = _config["Auth0:AdditionalScopes"] ?? "openid";
        var additionalScopes = addScopes?.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).ToList();
        scopes?.AddRange(additionalScopes ?? new List<string>());

        var oauthScopeDic = new Dictionary<string, string>();
        foreach (var scope in scopes)
        {
            oauthScopeDic.Add(scope, $"Resource access: {scope}");
        }

        options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
        {
            Type = SecuritySchemeType.OAuth2,
            Flows = new OpenApiOAuthFlows
            {
                AuthorizationCode = new OpenApiOAuthFlow
                {
                    AuthorizationUrl = new Uri(disco.AuthorizeEndpoint),
                    TokenUrl = new Uri(disco.TokenEndpoint),
                    Scopes = oauthScopeDic
                }
            }
        });
        ******************************************************************/

        //Comment this out 
        options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
        {
            Type = SecuritySchemeType.OAuth2,
            Flows = new OpenApiOAuthFlows
            {
                AuthorizationCode = new OpenApiOAuthFlow
                {
                    AuthorizationUrl = new Uri(disco.AuthorizeEndpoint),
                    TokenUrl = new Uri(disco.TokenEndpoint)
                }
            }
        });

        options.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "oauth2" }
                },
                new List<string>()
                
                // exchange the empty list above with this to have scopes
                // ,oauthScopeDic.Keys.ToArray()
            }
        });
    }

    private DiscoveryDocumentResponse GetDiscoveryDocument()
    {
        var client = new HttpClient();
        var authority = _config.GetValue<string>("Auth0:Domain");
        return client.GetDiscoveryDocumentAsync(authority)
            .GetAwaiter()
            .GetResult();
    }
}
