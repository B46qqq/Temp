using System.Security.Principal;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace webapi.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : WebControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<WeatherForecastController> _logger;

    public WeatherForecastController(ILogger<WeatherForecastController> logger)
    {
        _logger = logger;
    }

    //[AllowAnonymous]
    [HttpGet(Name = "GetWeatherForecast")]
    public IEnumerable<WeatherForecast> Get()
    {
        var p = HttpContext.User;
        var userInfo = p?.Identity as WindowsIdentity;
        var roles = GetRoles();

        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        })
        .ToArray();
    }

    [AllowAnonymous]
    [HttpPost(Name = "NoAuthNeeded")]
    public IActionResult NoAuthNeeded()
    {
        return Ok("hello world");
    }

    private IEnumerable<string> GetRoles()
    {
        var u = HttpContext.User?.Identity as WindowsIdentity;
        if (u == null) return Array.Empty<string>();

        return u.Groups
            .Select(g => g.Translate(typeof(NTAccount)).Value)
            .ToArray();
    }
}
