using System.ComponentModel.DataAnnotations;
using domain.enums;

namespace domain.models
{
    public class User
    {
        [Required]
        public string? Username { get; set; }

        [Required]
        public string? Password { get; set; }

        [Required]
        public Role Role { get; set; }

        public PersonalInfo PersonalInfo { get; set; }
    }
}