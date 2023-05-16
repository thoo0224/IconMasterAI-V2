using System.Net;

using Amazon.S3;
using Amazon.S3.Model;

using IconMasterAI.Core.Services;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace IconMasterAI.Persistence.Services;

// TODO: Validations
internal sealed class FileStorageService : IFileStorageService
{
    private readonly ILogger<FileStorageService> _logger;
    private readonly IConfiguration _configuration;
    private readonly IAmazonS3 _amazonS3;

    public FileStorageService(
        ILogger<FileStorageService> logger, 
        IConfiguration configuration, 
        IAmazonS3 amazonS3)
    {
        _logger = logger;
        _configuration = configuration;
        _amazonS3 = amazonS3;
    }

    public Task<string?> UploadBytesAsync(
        string fileName,
        byte[] data,
        string? contentType = null,
        string? encodingType = null,
        CancellationToken ct = default)
    {
        var fileStream = new MemoryStream(data);
        fileStream.Seek(0, SeekOrigin.Begin);

        var request = new PutObjectRequest
        {
            Key = fileName,
            InputStream = fileStream,
            ContentType = contentType,
            DisablePayloadSigning = true,
            Headers =
            {
                ContentEncoding = encodingType
            }
        };

        return UploadAsync(request, ct);
    }

    public Task<string?> UploadContentAsync(
        string fileName, 
        string content, 
        string? contentType = null, 
        string? encodingType = "",
        CancellationToken ct = default)
    {
        var request = new PutObjectRequest
        {
            Key = fileName,
            ContentBody = content,
            ContentType = contentType,
            DisablePayloadSigning = true,
            Headers =
            {
                ContentEncoding = encodingType
            }
        };

        return UploadAsync(request, ct);
    }

    private async Task<string?> UploadAsync(PutObjectRequest request, CancellationToken ct = default)
    {
        try
        {
            var bucketName = _configuration["Aws:BucketName"]
                             ?? throw new Exception("No AWS S3 bucket name in configuration.");
            request.BucketName = bucketName;

            var response = await _amazonS3.PutObjectAsync(request, ct).ConfigureAwait(false);
            if (response.HttpStatusCode != HttpStatusCode.OK)
                return null;

            return GetObjectUrl(request.Key);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "{Message}", e.Message);
            return null;
        }
    }

    private static string GetObjectUrl(string key)
    {
        return $"https://cdn.iconmasterai.com/{key}";
    }
}
