using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
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
        try
        {
            if (!Directory.Exists(_environment.WebRootPath + "/images"))
            {
                Directory.CreateDirectory(_environment.WebRootPath + "/images");
            }

            var path = Path.Combine(_environment.WebRootPath, "images", fileName);

            using (var stream = new FileStream(path, FileMode.Create))
            {
                await File.CopyToAsync(stream);
                await stream.FlushAsync();

                var url = Path.Combine(_originAccessor.GetOrigin(), "images", fileName);

                return url;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return null;
        }
    }

    public Task<bool> DeleteImage(string publicId)
    {
        throw new NotImplementedException();
    }
}
