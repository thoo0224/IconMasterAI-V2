namespace IconMasterAI.Core.Services;

public interface IFileStorageService
{
    Task<string?> UploadBytesAsync(
        string fileName,
        byte[] data,
        string? contentType = null,
        string? encodingType = null,
        CancellationToken ct = default);

    Task<string?> UploadContentAsync(
        string fileName,
        string content,
        string? contentType = null,
        string? encodingType = null,
        CancellationToken ct = default);
}
