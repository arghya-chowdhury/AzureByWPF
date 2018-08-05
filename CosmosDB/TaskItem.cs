using Microsoft.Azure.Documents;
using Newtonsoft.Json;

namespace CosmosDB
{
    public class TaskItem : Document
    {
        [JsonProperty(PropertyName = "Category")]
        public string Category { get; set; }

        [JsonProperty(PropertyName = "Name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "Description")]
        public string Description { get; set; }

        [JsonProperty(PropertyName = "IsComplete")]
        public bool IsComplete { get; set; }
    }
}
