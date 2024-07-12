namespace ATMAPPAPI.Services
{
    public interface IEmailService
    {
        Task<string> SendOTPMail(string accountNo);

        Task<string> VerifyOtp(string accountNo, string enteredOtp);

        Task<string> SendTransactionMail(string toEmailAddress, string accountNo, string transactionType, decimal amount);
    }
}
