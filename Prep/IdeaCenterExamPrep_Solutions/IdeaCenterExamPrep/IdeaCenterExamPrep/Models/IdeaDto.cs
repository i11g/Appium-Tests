using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace IdeaCenterExamPrep.Models
{
    public class IdeaDto
    {
        
        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("url")]
        public string ? Url { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        
    }
}
