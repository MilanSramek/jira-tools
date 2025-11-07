using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

using Jira.TimeConverters;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

using Refit;

namespace Jira;

public static class Registrations
{
    public static IServiceCollection AddJira(this IServiceCollection services)
    {
        var refitSettings = new RefitSettings
        {
            ContentSerializer = new SystemTextJsonContentSerializer(
                new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    Converters =
                    {
                        new DateTimeConverter()
                    }
                }),
            UrlParameterFormatter = new UrlParameterFormatter()
        };

        services.AddRefitClient<IJiraIssueApi>(refitSettings)
            .ConfigureHttpClient();

        services.AddRefitClient<IJiraUserApi>(refitSettings)
            .ConfigureHttpClient();

        return services;
    }

    private static IHttpClientBuilder ConfigureHttpClient(this IHttpClientBuilder builder)
    {
        return builder.ConfigureHttpClient((serviceProvider, httpClient) =>
        {
            var options = serviceProvider.GetRequiredService<IOptionsSnapshot<JiraOptions>>();
            httpClient.BaseAddress = new Uri(options.Value.ApiBaseUrl);

            // Jira uses Basic Authentication with email and API token
            var authToken = Convert.ToBase64String(
                Encoding.ASCII.GetBytes($"{options.Value.Email}:{options.Value.ApiKey}"));
            httpClient.DefaultRequestHeaders.Add("Authorization", $"Basic {authToken}");
        });
    }
}
