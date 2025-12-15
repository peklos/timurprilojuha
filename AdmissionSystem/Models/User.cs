using System;

namespace AdmissionSystem.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }
        public string Role { get; set; } // "Admin" или "User"
        public DateTime RegistrationDate { get; set; }

        public User()
        {
            RegistrationDate = DateTime.Now;
        }
    }
}
