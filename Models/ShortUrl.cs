using System.ComponentModel.DataAnnotations;

namespace ShortUrlPJ.Models;

public class ShortUrl
{
    public int Id { get; set; }
    [Required]
    public string LongUrl { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
