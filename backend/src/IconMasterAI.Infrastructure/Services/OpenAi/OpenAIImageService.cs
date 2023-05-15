using IconMasterAI.Core.Services.OpenAI.Image;
using IconMasterAI.Infrastructure.OpenAI.Image;

using Microsoft.Extensions.Logging;

using RestSharp;

namespace IconMasterAI.Infrastructure.Services.OpenAi;

internal sealed class OpenAIImageService : IOpenAIImageService
{
    private readonly ILogger<OpenAIImageService> _logger;
    private readonly RestClient _client;

    public OpenAIImageService(ILogger<OpenAIImageService> logger, IHttpClientFactory httpClientFactory)
    {
        _logger = logger;
        _client = new RestClient(
            httpClientFactory.CreateClient("openai"));
    }

    public async Task<OpenAICreateImageResponse> CreateImageAsync(OpenAICreateImageBody body, CancellationToken ct)
    {
        var request = new RestRequest("/v1/images/generations", Method.Post);
        request.AddJsonBody(body);

        var response = await _client.ExecutePostAsync<OpenAICreateImageResponse>(request, cancellationToken: ct)
            .ConfigureAwait(false);

        if (!response.IsSuccessful)
        {
            _logger.LogError("Unsuccessful response from DALEE. Request={@Request} Response={Response} Exception?={@Exception}",
                body, response.ErrorMessage, response.ErrorException);
        }

        return response.Data!;
    }
}
