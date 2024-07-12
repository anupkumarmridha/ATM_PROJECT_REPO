using System.Transactions;

namespace ATMAPPAPI.Models
{
    public class Account
    {
        public int AccountId { get; set; }
        public int UserId { get; set; }
        public decimal Balance { get; set; }
        public List<Transaction> Transactions { get; set; }
        public User User { get; set; }
    }
}
