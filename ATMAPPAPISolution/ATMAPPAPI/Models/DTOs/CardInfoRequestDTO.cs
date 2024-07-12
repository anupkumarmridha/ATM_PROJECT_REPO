namespace ATMAPPAPI.Models.DTOs
{
    public class CardInfoRequestDTO
    {
        public string CardNumber { get; set; }
        public string CVV { get; set; }
        public DateTime ExpiryDate { get; set; }
    }
}
