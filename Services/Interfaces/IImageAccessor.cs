using Microsoft.AspNetCore.Http;

namespace Services.Interfaces;

public interface IImageAccessor
{
    Task<string> AddImage(IFormFile File, string fileName);
    bool DeleteImage(string imageName);
}
