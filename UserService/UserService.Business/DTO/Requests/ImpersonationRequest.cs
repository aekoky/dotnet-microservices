using System.Text.Json.Serialization;

namespace UserService.Business.DTO.Requests
{
    public class ImpersonationRequest
    {
        [JsonPropertyName("username")]
        public string UserName { get; set; }
    }
}
