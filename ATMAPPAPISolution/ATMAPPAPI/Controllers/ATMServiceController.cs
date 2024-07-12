using ATMAPPAPI.Models;
using ATMAPPAPI.Models.DTOs;
using ATMAPPAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ATMAPPAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ATMServiceController : ControllerBase
    {
        private readonly IAtmService _atmService;

        public ATMServiceController(IAtmService atmService)
        {
            _atmService = atmService;
        }
        [HttpGet]
        [ProducesResponseType(typeof(decimal), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<decimal>> CheckBalance(string accountNo)
        {
            try
            {
                var balance = await _atmService.GetBalance(accountNo);
                return Ok(balance);
            }
            catch (Exception ex)
            {
                return NotFound(new ErrorModel(404, ex.Message));
            }
        }

        [HttpPost("Deposite")]
        [ProducesResponseType(typeof(AccountDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Deposit(TransactionDTO transactionDTO)
        {
            try
            {
               var account= await _atmService.Deposit(transactionDTO.AccountNumber, transactionDTO.Amount);
                return Ok(account);
            }
            catch (Exception ex)
            {
                return NotFound(new ErrorModel(404, ex.Message));
            }
        }

        [HttpPost("Withdraw")]
        [ProducesResponseType(typeof(AccountDTO),StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Withdraw(TransactionDTO transactionDTO)
        {
            try
            {
                var account=await _atmService.Withdraw(transactionDTO.AccountNumber, transactionDTO.Amount);
                return Ok(account);
            }
            catch (Exception ex)
            {
                return NotFound(new ErrorModel(404, ex.Message));
            }
        }

        [HttpPost("validate-card")]
        [ProducesResponseType(typeof(AccountDTO),StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        public async Task<ActionResult> ValidateCardNumber([FromBody] string CardNumber)
        {
            try
            {
                var CVV = "123";
                var ExpiryDate = "2025-07-11T10:45:28.323Z";
                DateTime expiryDateTime = DateTime.Parse(ExpiryDate);
                //var accountNo = await _atmService.ValidateCard(cardInfoRequestDTO.CardNumber, cardInfoRequestDTO.CVV, cardInfoRequestDTO.ExpiryDate);
                var account = await _atmService.ValidateCard(CardNumber, CVV, expiryDateTime);
                if (account != null)
                {
                    return Ok(account);
                }
                return NotFound(new ErrorModel(404, "Invalid card details"));
            }
            catch (Exception ex)
            {
                return NotFound(new ErrorModel(404, ex.Message));
            }
        }

        [HttpPost("validate-pin")]
        [ProducesResponseType(typeof(Boolean),StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        public async Task<ActionResult> ValidatePin(PinRequestDTO pinRequestDTO)
        {
            try
            {
                var isValid = await _atmService.ValidatePin(pinRequestDTO.AccountNumber, pinRequestDTO.Pin);
                if (isValid)
                {
                    return Ok();
                }
                return NotFound(new ErrorModel(404, "Invalid pin"));
            }
            catch (Exception ex)
            {
                return NotFound(new ErrorModel(404, ex.Message));
            }
        }

    }
}

