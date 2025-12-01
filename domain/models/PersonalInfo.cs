using System.ComponentModel.DataAnnotations;

namespace domain.models
{
    public class PersonalInfo
    {
        [Required]
        public string FullName { get; set; }

        [Required]
        [StringLength(11)]
        public string DocumentNumber { get; set; }

        public int GenderId { get; set; }
        
        public DateTime BirthDate { get; set; }

        public IEnumerable<Contact> Contacts { get; set; }

        public IEnumerable<Address> Addresses { get; set; }
    }
}