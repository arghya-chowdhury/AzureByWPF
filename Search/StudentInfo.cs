using Microsoft.Azure.Search;
using Microsoft.Azure.Search.Models;

namespace Search
{
    // The SerializePropertyNamesAsCamelCase attribute is defined in the Azure Search .NET SDK.
    // It ensures that Pascal-case property names in the model class are mapped to camel-case
    // field names in the index.
    [SerializePropertyNamesAsCamelCase]
    public partial class StudentInfo
    {
        [System.ComponentModel.DataAnnotations.Key]
        public string Id { get; set; }

        [IsSearchable, IsFacetable, IsSortable]
        public string Name { get; set; }

        [IsSearchable, IsFilterable, IsFacetable, IsSortable]
        public string Stream { get; set; }

        public override string ToString()
        {
            return $"Name:{Name}, Stream:{Stream}";
        }
    }
}
