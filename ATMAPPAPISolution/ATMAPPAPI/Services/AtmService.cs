
using ATMAPPAPI.Models;
using ATMAPPAPI.Models.DTOs;
using ATMAPPAPI.Repositoris;
using System.Security.Principal;
using System.Text.Json;

namespace ATMAPPAPI.Services
{
    public class AtmService : IAtmService
    {
        private readonly ICardValidationService _cardValidationService;
        private readonly ICardOperations _cardOperations;
        private readonly IEmailService _emailService;
        public AtmService(ICardValidationService cardValidationService, ICardOperations cardOperations, IEmailService emailService)
        {
            _cardValidationService = cardValidationService;
            _cardOperations = cardOperations;
            _emailService = emailService;
        }

        public async Task<AccountDTO> Deposit(string accountNo, decimal amount)
        {
            if (amount > 20000)
            {
                throw new ArgumentException("Cannot deposit more than 20000 at one go.");
            }

            var cardInfo = await _cardOperations.FindCardInfoAsync("accountNumber", accountNo);
            if (cardInfo != null)
            {
                decimal.TryParse(cardInfo.Balance, out decimal parsedBalanced);
                parsedBalanced += amount;
                cardInfo.Balance = parsedBalanced.ToString();
                // Update the card info in the JSON file
                await _cardOperations.UpdateCardInfoAsync(cardInfo);
                AccountDTO accountDTO = new AccountDTO();
                accountDTO.CurrentBalance = parsedBalanced;
                accountDTO.AccountNumber = cardInfo.AccountNumber;
                await SendTransactionEmail(accountNo, "Deposit", amount);
                return accountDTO;
            }
            return null;
        }

        public async Task<AccountDTO> Withdraw(string accountNo, decimal amount)
        {
            if (amount > 10000)
            {
                throw new ArgumentException("Cannot withdraw more than 10000 at one go.");
            }

            var cardInfo = await _cardOperations.FindCardInfoAsync("accountNumber", accountNo);
            if (cardInfo != null)
            {
                var balance = decimal.TryParse(cardInfo.Balance, out decimal parsedBalanced);
                if (balance && parsedBalanced >= amount)
                {
                    parsedBalanced -= amount;
                    cardInfo.Balance = parsedBalanced.ToString();
                    // Update the card info in the JSON file
                    await _cardOperations.UpdateCardInfoAsync(cardInfo);
                    AccountDTO accountDTO = new AccountDTO();
                    accountDTO.CurrentBalance = parsedBalanced;
                    accountDTO.AccountNumber = cardInfo.AccountNumber;
                    await SendTransactionEmail(accountNo, "Withdraw", amount);
                    return accountDTO;
                }
            }
            return null;
        }



        public async Task<decimal> GetBalance(string accountNo)
        {
            var cardInfo = await _cardOperations.FindCardInfoAsync("accountNumber", accountNo);
            if (cardInfo != null)
            {
                decimal.TryParse(cardInfo.Balance, out decimal parsedBalanced);
                return parsedBalanced;
            }
            throw new InvalidOperationException("Account not found");
        }

        public async Task SendTransactionEmail(string accountNo, string transactionType, decimal amount)
        {
            var cardInfo = await _cardOperations.FindCardInfoAsync("accountNumber", accountNo);
            if (cardInfo != null)
            {
                var result = await _emailService.SendTransactionMail(cardInfo.Email, accountNo, transactionType, amount);
                if (result == "Email sent successfully.")
                {
                    return;
                }
                throw new InvalidOperationException("Error Sending Transaction Mail!");
            }
            throw new InvalidOperationException("Account not found");
        }

        public async Task<AccountDTO> ValidateCard(string cardNumber, string cvv, DateTime expiryDate)
        {
            if (await _cardValidationService.ValidateCard(cardNumber, cvv, expiryDate))
            {
                var cardInfo = await _cardOperations.FindCardInfoAsync("cardNumber", cardNumber);
               
                if (cardInfo != null)
                {
                    AccountDTO accountDTO = new AccountDTO();
                    accountDTO.AccountNumber = cardInfo.AccountNumber;
                    accountDTO.CurrentBalance = decimal.Parse(cardInfo.Balance);
                    return accountDTO;
                }
            }
            return null;
        }

        public async Task<bool> ValidatePin(string accountNo, string pin)
        {
            var cardInfo = await _cardOperations.FindCardInfoAsync("accountNumber", accountNo);
            if (cardInfo != null)
            {
                return cardInfo.Pin == pin;
            }
            return false;
        }
    }
}
