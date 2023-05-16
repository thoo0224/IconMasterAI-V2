using System.Text.Json;
using System.Text.Json.Serialization;

using IconMasterAI.Core.Models.Inputs;
using IconMasterAI.Core.Models.Results;
using IconMasterAI.Core.Services;
using IconMasterAI.Infrastructure.OpenAI.Image;
using IconMasterAI.Infrastructure.Serialization;

using Microsoft.Extensions.Logging;

using RestSharp;
using RestSharp.Serializers.Json;

namespace IconMasterAI.Infrastructure.Services;

internal sealed class OpenAIImageGenerationService : IImageGenerationService
{
    private readonly ILogger<OpenAIImageGenerationService> _logger;
    private readonly RestClient _client;

    public OpenAIImageGenerationService(ILogger<OpenAIImageGenerationService> logger, IHttpClientFactory httpClientFactory)
    {
        _logger = logger;
        _client = new RestClient(
            httpClientFactory.CreateClient("openai"), configureSerialization: options =>
            {
                options.UseSystemTextJson(new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                    PropertyNamingPolicy = new JsonSnakeCaseNamingPolicy()
                });
            });
    }

    // TODO: Handle errors
    public async Task<ImageGenerationResult?> CreateImageAsync(ImageGenerationInput input, CancellationToken ct = default)
    {
        var body = new OpenAICreateImageBody
        {
            Prompt = input.Prompt,
            NumImages = input.NumImages,
        };

        var request = new RestRequest("/v1/images/generations", Method.Post);
        request.AddJsonBody(body);

        var response = await _client.ExecutePostAsync<OpenAICreateImageResponse>(request, cancellationToken: ct)
            .ConfigureAwait(false);

        if (!response.IsSuccessful || response.Data == null)
        {
            _logger.LogError("Unsuccessful response from DALEE. Request={@Request} Response={Response} Exception={Exception}",
                body, response.ErrorMessage, response.ErrorException?.Message ?? "None");
            return null;
        }

        var images = response.Data.Data
            .Select(x => body.ResponseFormat == OpenAICreateImageBody.ResponseFormatBase64
                ? x.Base64!
                : x.Url!)
            .ToArray();

        var result = new ImageGenerationResult(images);
        return result;
    }
}
