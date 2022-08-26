using System.Text.Json.Serialization;

namespace UserService.Business.DTO.Requests
{
    public class RefreshTokenRequest
    {
        [JsonPropertyName("refreshToken")]
        public string RefreshToken { get; set; }
    }
}
