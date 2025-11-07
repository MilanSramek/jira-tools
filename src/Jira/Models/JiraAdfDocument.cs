using System.Text.Json.Serialization;

namespace Jira;

/// <summary>
/// Represents an Atlassian Document Format (ADF) document for rich text content.
/// </summary>
public sealed record JiraAdfDocument
{
    [JsonPropertyName("type")]
    public string Type { get; init; } = "doc";

    [JsonPropertyName("version")]
    public int Version { get; init; } = 1;

    [JsonPropertyName("content")]
    public required List<JiraAdfNode> Content { get; init; }

    /// <summary>
    /// Creates a simple ADF document with a single paragraph of text.
    /// </summary>
    /// <param name="text">The text content.</param>
    /// <returns>An ADF document.</returns>
    public static JiraAdfDocument CreateText(string text)
    {
        return new JiraAdfDocument
        {
            Content =
            [
                new()
                {
                    Type = "paragraph",
                    Content =
                    [
                        new()
                        {
                            Type = "text",
                            Text = text
                        }
                    ]
                }
            ]
        };
    }

    public string? GetText()
    {
        return Content is [var c0] && c0.Type == "paragraph" && c0.Content is [var c1] && c1.Type == "text"
            ? c1.Text
            : null;
    }
}
