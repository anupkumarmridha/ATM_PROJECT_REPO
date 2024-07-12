namespace ATMAPPAPI.Models
{
    public class CardInfo
    {
        public int CardInfoId { get; set; }
        public string CardNumber { get; set; }
        public string CVV { get; set; }
        public DateTime ExpiryDate { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
    }
}
