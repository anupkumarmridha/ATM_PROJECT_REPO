namespace ATMAPPAPI.Models
{
    public class UserCredentials
    {
        public int UserCredentialsId { get; set; }
        public string Pin { get; set; }
        public string Otp { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
    }
}
