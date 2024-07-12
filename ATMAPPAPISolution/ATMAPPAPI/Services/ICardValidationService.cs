namespace ATMAPPAPI.Services
{
    public interface ICardValidationService
    {
        public Task<bool> ValidateCard(string cardNumber, string cvv, DateTime expiryDate);
    }
}
