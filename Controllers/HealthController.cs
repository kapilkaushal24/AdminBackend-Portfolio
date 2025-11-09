using Microsoft.AspNetCore.Mvc;

namespace PortfolioAdmin.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HealthController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(new { 
                status = "healthy", 
                timestamp = DateTime.UtcNow,
                version = "1.0.0",
                environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Unknown"
            });
        }

        [HttpGet("detailed")]
        public IActionResult GetDetailed()
        {
            var memoryUsage = GC.GetTotalMemory(false);
            var workingSet = Environment.WorkingSet;
            
            return Ok(new { 
                status = "healthy", 
                timestamp = DateTime.UtcNow,
                version = "1.0.0",
                environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Unknown",
                memory = new
                {
                    totalMemory = memoryUsage,
                    workingSet = workingSet
                },
                uptime = Environment.TickCount64
            });
        }
    }
}