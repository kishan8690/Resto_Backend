using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class DashboardController : ControllerBase
{
    private readonly IDashboardRepository _dashboardRepository;

    public DashboardController(IDashboardRepository dashboardRepository)
    {
        _dashboardRepository = dashboardRepository;
    }

    [HttpGet]
    public async Task<IActionResult> GetDashboardData()
    {
        try
        {
            var data = await _dashboardRepository.GetDashboardDataAsync();
            return Ok(new { success = true, data });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { success = false, message = ex.Message });
        }
    }
}
