using ATMAPPAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace ATMAPPAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        private readonly IEmailService _emailService;

        public EmailController(IEmailService emailService)
        {
            _emailService = emailService;
        }

        [HttpPost("SendOTP")]
        [ProducesResponseType(typeof(OkResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(JsonResult), StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<string>> SendOTP(string accountNo)
        {
            try
            {
                var result = await _emailService.SendOTPMail(accountNo);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return UnprocessableEntity(new JsonResult(new { 
                    Error_Message = ex.Message,
                    code = 422
                }));
            }
        }

        [HttpPost("VerifyOTP")]
        [ProducesResponseType(typeof(OkResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(JsonResult), StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<string>> VerifyOTP(string accountNo, string otp)
        {
            try
            {
                var result = await _emailService.VerifyOtp(accountNo, otp);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return UnprocessableEntity(new JsonResult(new
                {
                    Error_Message = ex.Message,
                    code = 422
                }));
            }
        }

    }
}
