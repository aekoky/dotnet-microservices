using System.Text.Json.Serialization;

namespace Formuler.Shared.DTO.UserService
{
    public class RefreshTokenRequestDTO
    {
        [JsonPropertyName("refreshToken")]
        public string RefreshToken { get; set; }
    }
}
