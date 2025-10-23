using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SupportApp.Models
{
    public class SupportMessage
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Required, StringLength(80)]
        [JsonPropertyName("customerName")]
        public string CustomerName { get; set; } = string.Empty;

        [Required, EmailAddress]
        [JsonPropertyName("email")]
        public string Email { get; set; } = string.Empty;

        [Required, StringLength(50)]
        [JsonPropertyName("category")]
        public string Category { get; set; } = string.Empty;

        [Required, StringLength(2000)]
        [JsonPropertyName("message")]
        public string Message { get; set; } = string.Empty;

        [JsonPropertyName("_ts")]
        public long? Timestamp { get; set; }
    }
}