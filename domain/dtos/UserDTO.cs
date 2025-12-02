using System.Text.Json.Serialization;
using domain.enums;

namespace domain.dtos
{
    public class UserDTO
    {
        public string Username { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public ClientStatus Status { get; set; }
    }
}