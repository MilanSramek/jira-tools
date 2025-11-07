using System.Text.Json;
using System.Text.Json.Serialization;

using Clockify.TimeConverters;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

using Refit;

namespace Clockify;

public static class Registrations
{
    public static IServiceCollection AddClockify(this IServiceCollection services)
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
                        new ISO8601DateTimeConverter(),
                        new ISO8601NullableDateTimeConverter()
                    }
                }),
            UrlParameterFormatter = new ISO8601UrlParameterFormatter()
        };

        services.AddRefitClient<IClockifyProjectApi>(refitSettings)
            .ConfigureHttpClient();
        services.AddRefitClient<IClockifyTimeEntryApi>(refitSettings)
            .ConfigureHttpClient();
        services.AddRefitClient<IClockifyUserApi>(refitSettings)
            .ConfigureHttpClient();
        services.AddRefitClient<IClockifyTaskApi>(refitSettings)
            .ConfigureHttpClient();

        return services;
    }

    private static IHttpClientBuilder ConfigureHttpClient(this IHttpClientBuilder builder)
    {
        return builder.ConfigureHttpClient((serviceProvider, httpClient) =>
        {
            var options = serviceProvider.GetRequiredService<IOptionsSnapshot<ClockifyOptions>>();
            httpClient.BaseAddress = new Uri(options.Value.ApiBaseUrl);
            httpClient.DefaultRequestHeaders.Add("X-Api-Key", options.Value.ApiKey);
        });
    }
}