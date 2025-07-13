using System.Text.Json.Serialization;

namespace DinamicCharts.WEB.Models
{
    public class TokenResponseDto
    {
        [JsonPropertyName("token")]
        public string Token { get; set; }
    }
}
