using System.Text.Json.Serialization;

namespace IconMasterAI.Infrastructure.OpenAI.Image;

internal sealed class OpenAICreateImageBody
{
    public const string ImageSize256 = "256x256";
    public const string ImageSize512 = "512x512";
    public const string ImageSize1024 = "1024x1024";

    public const string ResponseFormatUrl = "url";
    public const string ResponseFormatBase64 = "b64_json";

    public required string Prompt { get; set; }

    [JsonPropertyName("n")]
    public int NumImages { get; set; } = 1;

    public string Size { get; set; } = ImageSize1024;

    public string ResponseFormat { get; set; } = ResponseFormatBase64;

    public string? User { get; set; }
}
