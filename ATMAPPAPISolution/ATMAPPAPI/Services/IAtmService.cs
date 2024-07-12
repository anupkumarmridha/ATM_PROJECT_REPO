using ATMAPPAPI.Models;
using ATMAPPAPI.Models.DTOs;

namespace ATMAPPAPI.Services
{
    public interface IAtmService
    {
        Task<AccountDTO> ValidateCard(string cardNumber, string cvv, DateTime expiryDate);
        Task<bool> ValidatePin(string accountNo, string pin);
        Task<decimal> GetBalance(string accountNo);
        Task<AccountDTO> Deposit(string accountNo, decimal amount);
        Task<AccountDTO> Withdraw(string accountNo, decimal amount);
        Task SendTransactionEmail(string accountNo, string transactionType, decimal amount);
    }
}
