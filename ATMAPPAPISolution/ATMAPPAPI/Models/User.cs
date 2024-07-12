using System.Dynamic;
using System.Security.Principal;

namespace ATMAPPAPI.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public CardInfo CardInfo { get; set; }
        public UserCredentials Credentials { get; set; }
        public Account Account { get; set; }
    }
}
