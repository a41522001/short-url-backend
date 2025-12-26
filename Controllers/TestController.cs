using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ShortUrlPJ.Controllers;

[Route("[controller]")]
[ApiController]
public class TestController(IConfiguration configuration) : ControllerBase
{
    [HttpGet]
    public IActionResult GetVariable()
    {
        var c = configuration["Info:FirstName"]+ configuration["Info:LastName"];
        return Ok(c);
    }
}
