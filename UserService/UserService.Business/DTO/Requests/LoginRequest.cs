using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace UserService.Business.DTO.Requests
{
    public class LoginRequest
    {
        [Required]
        [JsonPropertyName("username")]
        public string UserName { get; set; }

        [Required]
        [JsonPropertyName("password")]
        public string Password { get; set; }
    }
}
