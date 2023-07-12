using GaiThong_API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GaiThong_API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly IHomeService _homeService;
        public HomeController(IHomeService homeService)
        {
            this._homeService = homeService;
        }

        [HttpGet("GetAllToday")]
        public async Task<IActionResult> GetAllToday()
        {
            try
            {
                var result = await _homeService.GetAllToday();
                return Ok(new { isSuccess = true, data = result });
            }
            catch (Exception ex)
            {
                return new JsonResult(new
                {
                    isSuccess = false,
                    statusCode = 500,
                    message = ex.Message
                });
            }
        }
    }
}
