using Microsoft.EntityFrameworkCore;
using ShortUrlPJ.Contexts;
using ShortUrlPJ.Dtos;
using ShortUrlPJ.Models;
using ShortUrlPJ.Services.Interfaces;
using ShortUrlPJ.Utils;

namespace ShortUrlPJ.Services;

public class UrlService : IUrlService
{
    private readonly ShortUrlDbContext _db;
    private readonly string _baseUrl;
    public UrlService(ShortUrlDbContext db, IConfiguration configuration)
    {
        _db = db;
        _baseUrl = configuration["AppSettings:BaseUrl"] ?? "";
    }

    public async Task<string> CreateShortUrlAsync(UrlCreateDto createData)
    {
        var shortUrl = new ShortUrl
        {
            LongUrl = createData.Url,
        };
        await _db.ShortUrls.AddAsync(shortUrl);
        await _db.SaveChangesAsync();
        var base62Encode = Base62.Encode(shortUrl.Id);
        return base62Encode;
    }

    public async Task<string> GetShortUrlAsync(string id)
    {
        var shortUrl = await _db.ShortUrls.FirstOrDefaultAsync(item => item.Id == Base62.Decode(id));
        return shortUrl?.LongUrl ?? string.Empty;
    }
}
