using Microsoft.AspNetCore.Mvc;
using ShortUrlPJ.Contexts;
using ShortUrlPJ.Dtos;
using ShortUrlPJ.Services.Interfaces;

namespace ShortUrlPJ.Controllers;

[ApiController]
[Route("[controller]")]
public class UrlController : ControllerBase
{
    private readonly ShortUrlDbContext _db;
    private readonly IUrlService _urlService;
    public UrlController(ShortUrlDbContext db, IUrlService urlService)
    {
        _db = db;
        _urlService = urlService;
    }
    [HttpGet("{id}")]
    public async Task<ActionResult> GetUrl([FromRoute] string id)
    {
        var url = await _urlService.GetShortUrlAsync(id);
        if(String.IsNullOrEmpty(url))
        {
            return NotFound("找不到此網址");
        }
        return Ok(url);
    }
    [HttpPost]
    public async Task<ActionResult> CreateUrl([FromBody] UrlCreateDto req)
    {
        string? origin = Request.Headers["Origin"];
        var base62Encode = await _urlService.CreateShortUrlAsync(req);
        return Ok($"{origin}/{base62Encode}");
    }
}
