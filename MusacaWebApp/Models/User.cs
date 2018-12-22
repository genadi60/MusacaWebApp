namespace MusacaWebApp.Models
{
    using System.Collections.Generic;

    public class User
    {
        public User()
        {
            UserReceipts = new HashSet<UserReceipt>();
        }

        public string Id { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public string Email { get; set; }

        public Role Role { get; set; }

        public ICollection<UserReceipt> UserReceipts { get; set; }
    }
}
