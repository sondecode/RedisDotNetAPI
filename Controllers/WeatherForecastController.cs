using DemoRedis.Attributes;
using DemoRedis.Services;
using Microsoft.AspNetCore.Mvc;

namespace DemoRedis.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private IResponseCacheService _responseCacheService;
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IResponseCacheService responseCacheService)
        {
            _logger = logger;
            _responseCacheService = responseCacheService;
        }

        [HttpGet("getall")]
        [Cache(1000)]
        public async Task<IActionResult> GetAsync(int PageNumber, int PageSize)
        {
            var result = Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();

            return Ok(result);
        }

        [HttpGet("create")]
        public async Task<IActionResult> Create()
        {
            await _responseCacheService.RemoveCacheResponseAsync("/WeatherForecast/getall");
            return Ok();
        }
    }
}
