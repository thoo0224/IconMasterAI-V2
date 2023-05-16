using IconMasterAI.Core.Entities;
using IconMasterAI.Core.Repositories;
using IconMasterAI.Core.Services;

namespace IconMasterAI.Persistence.Repositories;

internal sealed class IconRepository : IIconRepository
{
    private readonly IFileStorageService _fileStorageService;
    private readonly ApplicationDbContext _dbContext;
    private readonly IUnitOfWork _unitOfWork;

    public IconRepository(
        IFileStorageService fileStorageService, 
        ApplicationDbContext dbContext, 
        IUnitOfWork unitOfWork)
    {
        _fileStorageService = fileStorageService;
        _dbContext = dbContext;
        _unitOfWork = unitOfWork;
    }

    public async Task<Icon[]> CreateManyAsync(string rawPrompt, string finalPrompt, string[] images, CancellationToken ct = default)
    {
        var tasks = images.Select(image => CreateAsync(rawPrompt, finalPrompt, image, ct));
        var icons = await Task.WhenAll(tasks);
        await _unitOfWork.SaveChangedAsync(ct);

        return icons;
    }

    private async Task<Icon> CreateAsync(string rawPrompt, string finalPrompt, string image, CancellationToken ct = default)
    {
        var id = Guid.NewGuid().ToString();
        var imageUrl = await UploadImageAsync(id, image, ct).ConfigureAwait(false);
        var icon = Icon.Create(
                id,
                imageUrl,
                rawPrompt,
                finalPrompt);

        _dbContext.Set<Icon>().Add(icon);
        return icon;
    }

    private async Task<string> UploadImageAsync(string id, string base64, CancellationToken ct = default)
    {
        var data = Convert.FromBase64String(base64);
        var image = await _fileStorageService.UploadBytesAsync(
            $"icons/{id}.png",
            data,
            contentType: "image/png",
            encodingType: "base64", ct: ct).ConfigureAwait(false);

        if (image == null)
            throw new Exception("Failed to upload image to bucket.");

        return image;
    }
}
