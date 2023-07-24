using GaiThong_API.Services;
using Microsoft.AspNetCore.Mvc;


namespace GaiThong_API.Controllers
{
    public class LineApiController : Controller
    {
     
        private readonly ILineNotifyService _lineNotifyService;
        public LineApiController(ILineNotifyService lineNotifyService)
        {
            this._lineNotifyService = lineNotifyService;
        }

        [HttpPost("LineNotify")]
        public async Task<IActionResult> LineNotify(string message)
        {
            try
            {
                var response = await _lineNotifyService.SentNotify(message);
                return Ok(new { isSuccess = response });
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
