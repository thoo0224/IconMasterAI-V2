using System.Text.Json.Serialization;

namespace IconMasterAI.Infrastructure.OpenAI.Image;

internal sealed class OpenAICreateImageBody
{
    public const string ImageSize256 = "256x256";
    public const string ImageSize512 = "256x256";
    public const string ImageSize1024 = "256x256";

    public required string Prompt { get; set; }

    [JsonPropertyName("n")]
    public int NumImages { get; set; } = 1;

    public string Size { get; set; } = ImageSize1024;

    [JsonPropertyName("response_format")]
    public string? ResponseFormat { get; set; }

    public string? User { get; set; }
}
