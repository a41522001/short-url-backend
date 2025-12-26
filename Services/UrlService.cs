using Microsoft.EntityFrameworkCore;
using ShortUrlPJ.Contexts;
using ShortUrlPJ.Dtos;
using ShortUrlPJ.Models;
using ShortUrlPJ.Services.Interfaces;
using ShortUrlPJ.Utils;
using StackExchange.Redis;

namespace ShortUrlPJ.Services;

public class UrlService : IUrlService
{
    private readonly ShortUrlDbContext _db;
    private readonly string _baseUrl;
    private readonly IDatabase _redis;
    public UrlService(ShortUrlDbContext db, IConfiguration configuration, IConnectionMultiplexer redis)
    {
        _db = db;
        _baseUrl = configuration["AppSettings:BaseUrl"] ?? "";
        _redis = redis.GetDatabase();
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
        var decodeID = Base62.Decode(id);
        var redisUrl = await _redis.StringGetAsync($"url:{decodeID}");
        if(!redisUrl.IsNull)
        {
            return redisUrl;
        }
        var result = await _db.ShortUrls.FirstOrDefaultAsync(item => item.Id == decodeID);
        var shortUrl = result?.LongUrl ?? string.Empty;
        if(shortUrl != "")
        {
            await _redis.StringSetAsync($"url:{decodeID}", shortUrl, TimeSpan.FromHours(24));
        }

        return shortUrl;
    }
}
