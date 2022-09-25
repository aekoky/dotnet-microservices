using System.Text.Json.Serialization;

namespace Formuler.Shared.DTO.UserService
{
    public class ImpersonationRequestDTO
    {
        [JsonPropertyName("username")]
        public string UserName { get; set; }
    }
}
