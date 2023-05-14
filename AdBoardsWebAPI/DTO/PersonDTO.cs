using System.Text.Json.Serialization;

namespace AdBoardsWebAPI.DTO
{
    public class PersonDTO
    {
        [JsonPropertyName("login")]
        public string? Login { get; set; }

        [JsonPropertyName("password")]
        public string? Password { get; set; }

        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("city")]
        public string? City { get; set; }

        [JsonPropertyName("birthday")]
        public DateTime? Birthday { get; set; }

        [JsonPropertyName("phone")]
        public string? Phone { get; set; }

        [JsonPropertyName("email")]
        public string? Email { get; set; }

        [JsonPropertyName("photo")]
        public byte[]? Photo { get; set; }

        [JsonPropertyName("rightId")]
        public int? RightId { get; set; }
    }
}
