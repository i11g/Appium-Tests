using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace IdeaCenterExamPrep.Models
{
    public class ApiResponseDto
    {
        [JsonPropertyName("msg")]
        public string ? Msg { get; set; }

        [JsonPropertyName("id")]
        public string ? IdeaId { get; set; } // This will be null for responses that don't include a idea ID.
    }
}
