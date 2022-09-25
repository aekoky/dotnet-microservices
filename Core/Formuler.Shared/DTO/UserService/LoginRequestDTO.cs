using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Formuler.Shared.DTO.UserService
{
    public class LoginRequestDTO
    {
        [Required]
        [JsonPropertyName("username")]
        public string UserName { get; set; }

        [Required]
        [JsonPropertyName("password")]
        public string Password { get; set; }
    }
}
