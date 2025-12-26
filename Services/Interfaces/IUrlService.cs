using ShortUrlPJ.Dtos;

namespace ShortUrlPJ.Services.Interfaces;

public interface IUrlService
{
    Task<string> CreateShortUrlAsync(UrlCreateDto createData);
    Task<string> GetShortUrlAsync(string id);
}
