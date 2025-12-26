using Microsoft.EntityFrameworkCore;
using ShortUrlPJ.Models;
namespace ShortUrlPJ.Contexts;

public class ShortUrlDbContext : DbContext
{
    public ShortUrlDbContext(DbContextOptions<ShortUrlDbContext> options) : base(options) { }
    public DbSet<ShortUrl> ShortUrls => Set<ShortUrl>();
}
