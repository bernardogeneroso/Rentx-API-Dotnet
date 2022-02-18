using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Services.Interfaces;

namespace Providers.Image;

public class ImageAccessor : IImageAccessor
{
    private readonly IWebHostEnvironment _environment;
    private readonly ILogger<ImageAccessor> _logger;
    private readonly IOriginAccessor _originAccessor;
    public ImageAccessor(IWebHostEnvironment environment, IOriginAccessor originAccessor, ILogger<ImageAccessor> logger)
    {
        _originAccessor = originAccessor;
        _logger = logger;
        _environment = environment;
    }

    public async Task<string> AddImage(IFormFile File, string fileName)
    {
        var directoryPath = _environment.WebRootPath + "/images";

        if (!Directory.Exists(directoryPath)) Directory.CreateDirectory(directoryPath);

        var path = Path.Combine(_environment.WebRootPath, "images", fileName);

        using var stream = new FileStream(path, FileMode.Create);

        try
        {
            await File.CopyToAsync(stream);
            await stream.FlushAsync();

            var url = Path.Combine(_originAccessor.GetOrigin(), "images", fileName);

            return url;

        }
        catch (Exception ex)
        {
            if (_environment.IsDevelopment()) _logger.LogError(ex.Message);

            return null;
        }
        finally
        {
            stream?.Dispose();
        }
    }

    public bool DeleteImage(string imageName)
    {
        try
        {
            var path = Path.Combine(_environment.WebRootPath, "images", imageName);

            if (File.Exists(path)) File.Delete(path);

            return true;
        }
        catch (Exception ex)
        {
            if (_environment.IsDevelopment()) _logger.LogError(ex.Message);

            return false;
        }
    }
}
