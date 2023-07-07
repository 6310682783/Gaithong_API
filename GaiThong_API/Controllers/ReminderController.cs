using Microsoft.AspNetCore.Mvc;
using GaiThong_API.Models;
using GaiThong_API.Services;

namespace GaiThong_API.Controllers
{
    [Route("[controller]")]
    public class ReminderController : Controller
    {
        private readonly IReminderService _reminderService;
        public ReminderController(IReminderService reminderService)
        {
            this._reminderService = reminderService;
        }
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var result = await _reminderService.GetAll();
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

        [HttpGet("GetAllFromNow")]
        public async Task<IActionResult> GetAllFromNow()
        {
            try
            {
                var result = await _reminderService.GetAllFromNow();
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

        [HttpGet("GetById/{Id}")]
        public async Task<IActionResult> GetById(int Id)
        {
            try
            {
                var result = await _reminderService.GetById(Id);
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
        [HttpPost("Add")]
        public async Task<IActionResult> Add([FromForm] Reminder model)
        {
            try
            {
                var result = await _reminderService.Add(model);
                return Ok(new { isSuccess = true });
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
        [HttpDelete("Delete/{Id}")]
        public async Task<IActionResult> Delete(int Id)
        {
            try
            {
                var result = await _reminderService.Delete(Id);
                return Ok(new { isSuccess = result });
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
        [HttpPatch("Update")]
        public async Task<IActionResult> Update([FromForm] Reminder model)
        {
            try
            {
                var result = await _reminderService.Update(model);
                return Ok(new { isSuccess = true });
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

