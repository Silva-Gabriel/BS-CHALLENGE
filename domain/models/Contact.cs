
namespace domain.models
{
    public class Contact
    {
        public string Type { get; set; }

        public string Value { get; set; }

        public bool IsPrimary { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}